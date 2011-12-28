using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli
{
    using System.Text;
    using Mi.PE.Cli.CodedIndices;
    using Mi.PE.Cli.Tables;
    using Mi.PE.Internal;
    using Mi.PE.PEFormat;

    public sealed class ClrModuleReader
    {
        public const uint ClrHeaderSize = 72;

        readonly BinaryStreamReader m_Binary;
        readonly ClrModule module;
        TableStream tableStream;
        
        Guid[] guids;
        byte[] blobHeap;
        byte[] stringHeap;
        readonly Dictionary<uint, string> stringHeapCache = new Dictionary<uint, string>();

        private ClrModuleReader(BinaryStreamReader binaryReader, ClrModule module)
        {
            this.m_Binary = binaryReader;
            this.module = module;
        }

        public static void Read(BinaryStreamReader reader, ClrModule module)
        {
            var modReader = new ClrModuleReader(reader, module);
            modReader.Read();
        }

        void Read()
        {
            // CLR header
            uint cb = this.Binary.ReadUInt32();

            if (cb < ClrHeaderSize)
                throw new BadImageFormatException(
                    "Unexpectedly short CLR header structure " + cb + " reported by Cb field " +
                    "(expected at least " + ClrHeaderSize + ").");

            this.module.RuntimeVersion = new Version(this.Binary.ReadUInt16(), this.Binary.ReadUInt16());

            var metadataDir = new DataDirectory();
            metadataDir.Read(this.Binary);

            this.module.ImageFlags = (ClrImageFlags)this.Binary.ReadInt32();

            uint entryPointToken = this.Binary.ReadUInt32();

            var resourcesDir = new DataDirectory();
            resourcesDir.Read(this.Binary);

            var strongNameSignatureDir = new DataDirectory();
            strongNameSignatureDir.Read(this.Binary);

            var codeManagerTableDir = new DataDirectory();
            codeManagerTableDir.Read(this.Binary);

            var vtableFixupsDir = new DataDirectory();
            vtableFixupsDir.Read(this.Binary);

            var exportAddressTableJumpsDir = new DataDirectory();
            exportAddressTableJumpsDir.Read(this.Binary);

            var managedNativeHeaderDir = new DataDirectory();
            managedNativeHeaderDir.Read(this.Binary);



            // CLR metadata
            this.Binary.Position = metadataDir.VirtualAddress;

            var mdSignature = (ClrMetadataSignature)this.Binary.ReadUInt32();
            if (mdSignature != ClrMetadataSignature.Signature)
                throw new InvalidOperationException("Invalid CLR metadata signature field " + ((uint)mdSignature).ToString("X") + "h.");

            this.module.MetadataVersion = new Version(
                this.Binary.ReadUInt16(),
                this.Binary.ReadUInt16());

            uint mdReserved = this.Binary.ReadUInt32();

            int versionLength = this.Binary.ReadInt32();
            string versionString = this.Binary.ReadFixedZeroFilledAsciiString(versionLength);

            this.module.MetadataVersionString = versionString;

            short mdFlags = this.Binary.ReadInt16();

            ushort streamCount = this.Binary.ReadUInt16();
            var streamHeaders = new StreamHeader[streamCount];

            for (int i = 0; i < streamHeaders.Length; i++)
            {
                streamHeaders[i] = new StreamHeader();
                streamHeaders[i].Read(this.Binary);
            }

            this.guids = null;
            this.blobHeap = null;
            this.stringHeap = null;

            StreamHeader tableStreamHeader = null;
            foreach (var sh in streamHeaders)
            {
                this.Binary.Position = metadataDir.VirtualAddress + sh.Offset;

                switch (sh.Name)
                {
                    case "#GUID":
                        this.guids = new Guid[sh.Size / 16];
                        ReadGuids(this.Binary, this.guids);
                        break;

                    case "#Strings":
                        this.stringHeap = ReadBinaryHeap(this.Binary, sh.Size);
                        break;

                    case "#US": // user strings
                        break;

                    case "#Blob":
                        this.blobHeap = ReadBinaryHeap(this.Binary, sh.Size);
                        break;

                    case "#~":
                    case "#-":
                        tableStreamHeader = sh;
                        break;

                    default:
                        break;
                }
            }

            this.Binary.Position = metadataDir.VirtualAddress + tableStreamHeader.Offset;
            this.ReadTableStream();

            this.LoadModule();
            this.LoadTypes();
        }

        public BinaryStreamReader Binary { get { return m_Binary; } }

        public string ReadString()
        {
            uint pos;
            if(this.stringHeap.Length<ushort.MaxValue)
                pos = this.Binary.ReadUInt16();
            else
                pos = this.Binary.ReadUInt32();

            string result;
            if(pos == 0 )
            {
                result = null;
            }
            else if(!stringHeapCache.TryGetValue(pos, out result))
            {
                int length = 0;
                while(pos + length < stringHeap.Length)
                {
                    if(stringHeap[pos + length]==0)
                        break;
                    else
                        length ++;
                }

                result = Encoding.UTF8.GetString(stringHeap, (int)pos, length);

                stringHeapCache[pos] = result;
            }

            return result;
        }

        public Guid? ReadGuid()
        {
            uint index;

            if (this.guids.Length <= ushort.MaxValue)
                index = this.Binary.ReadUInt16();
            else
                index = this.Binary.ReadUInt32();

            if (index == 0)
                return null;

            return guids[(index-1)/16];
        }

        public byte[] ReadBlob()
        {
            uint index;

            if (this.blobHeap.Length <= ushort.MaxValue)
                index = this.Binary.ReadUInt16();
            else
                index = this.Binary.ReadUInt32();

            if (index == 0)
                return null;

            uint length;

            byte b0 = this.blobHeap[index];
            if (b0 <= sbyte.MaxValue)
            {
                length = b0;
            }
            else if ((b0 & 0xC0) == sbyte.MaxValue + 1)
            {
                byte b2 = this.blobHeap[index + 1];
                length = unchecked((uint)(((b0 & 0x3F) << 8) | b2));
            }
            else
            {
                byte b2 = this.blobHeap[index + 1];
                byte b3 = this.blobHeap[index + 2];
                byte b4 = this.blobHeap[index + 3];
                length = unchecked((uint)(((b0 & 0x3F) << 24) + (b2 << 16) + (b3 << 8) + b4));
            }

            byte[] result = new byte[length];
            Array.Copy(
                this.blobHeap, (int)index,
                result, 0,
                (int)length);

            return result;
        }

        public Signature ReadSignature()
        {
            uint index;

            if (this.blobHeap.Length <= ushort.MaxValue)
                index = this.Binary.ReadUInt16();
            else
                index = this.Binary.ReadUInt32();

            if (index == 0)
                return null;

            return new Signature((int)index, this.blobHeap);


            //byte[] blob = this.ReadBlob();
            //return new Signature(blob);
        }

        public Version ReadVersion()
        {
            ushort major = this.Binary.ReadUInt16();
            ushort minor = this.Binary.ReadUInt16();
            ushort buildNumber = this.Binary.ReadUInt16();
            ushort revisionNumber = this.Binary.ReadUInt16();
            return new Version(major, minor, buildNumber, revisionNumber);
        }

        public uint ReadTableIndex(TableKind table)
        {
            ushort mask = ushort.MaxValue;

            int length = this.tableStream.Tables[(int)table].Length;

            uint index;
            if ((length & ~mask) == 0)
                index = this.Binary.ReadUInt16();
            else
                index = this.Binary.ReadUInt32();

            if (index > length + 1)
                throw new FormatException("Index " + index + " is out of range [0.." + length + "] for " + table + " table.");

            return index;
        }

        public CodedIndex<TCodedIndexDefinition> ReadCodedIndex<TCodedIndexDefinition>()
            where TCodedIndexDefinition : struct, ICodedIndexDefinition
        {
            var def = default(TCodedIndexDefinition);

            ushort mask = (ushort)(ushort.MaxValue >> CodedIndex<TCodedIndexDefinition>.TableKindBitCount);

            int length = 0;
            foreach (var tab in def.Tables)
            {
                if ((int)tab == ushort.MaxValue)
                    continue;

                var table = this.tableStream.Tables[(int)tab];
                
                length = Math.Max(length, table==null ? 0 : table.Length);
            }

            uint result;

            if ((length & ~mask) == 0)
                result = this.Binary.ReadUInt16();
            else
                result = this.Binary.ReadUInt32();

            var typedResult = (CodedIndex<TCodedIndexDefinition>)result;

            if (typedResult.Index > this.tableStream.Tables[(int)typedResult.TableKind].Length)
            {
                throw new FormatException(
                    "Coded index " + typedResult + " is out of bound " +
                    "(0.." + this.tableStream.Tables[(int)typedResult.TableKind].Length + ") " +
                    "for " + typeof(TCodedIndexDefinition).Name + " " +
                    "(" + string.Join(",", def.Tables.Select(t => t.ToString()).ToArray()) + ").");
            }

            return typedResult;
        }

        static void ReadGuids(BinaryStreamReader reader, Guid[] guids)
        {
            byte[] buf = new byte[16];
            for (int i = 0; i < guids.Length; i++)
            {
                reader.ReadBytes(buf, 0, buf.Length);
                guids[i] = new Guid(buf);
            }
        }

        static byte[] ReadBinaryHeap(BinaryStreamReader reader, uint size)
        {
            byte[] heapBytes = new byte[size];
            reader.ReadBytes(heapBytes, 0, (int)size);
            return heapBytes;
        }

        void ReadTableStream()
        {
            tableStream = new TableStream();
            tableStream.Read(this);
            this.module.TableStreamVersion = this.tableStream.Version;
        }

        void LoadModule()
        {
            ModuleEntry moduleEntry;
            {
                var moduleEntries = (ModuleEntry[])tableStream.Tables[(int)TableKind.Module];
                if (moduleEntries == null || moduleEntries.Length == 0)
                {
                    this.module.Name = null;
                    this.module.Mvid = null;
                    this.module.Generation = 0;
                    this.module.EncId = null;
                    this.module.EncBaseId = null;
                    return;
                }

                moduleEntry = moduleEntries[0];
            }

            this.module.Name = moduleEntry.Name;
            this.module.Mvid = moduleEntry.Mvid;
            this.module.Generation = moduleEntry.Generation;
            this.module.EncId = moduleEntry.EncId;
            this.module.EncBaseId = moduleEntry.EncBaseId;
        }

        void LoadTypes()
        {
            var typeDefEntries = (TypeDefEntry[])tableStream.Tables[(int)TableKind.TypeDef];
            if (typeDefEntries == null || typeDefEntries.Length == 0)
            {
                this.module.Types = null;
                return;
            }

            bool isFirstTypeModuleStub;

            if (typeDefEntries[0].Extends.Index == 0
                && (typeDefEntries[0].Flags & TypeAttributes.Interface) == 0)
                isFirstTypeModuleStub = true;
            else
                isFirstTypeModuleStub = false;

            {
                int typeCount = isFirstTypeModuleStub ? typeDefEntries.Length - 1 : typeDefEntries.Length;
                if (this.module.Types == null
                    || this.module.Types.Length != typeCount)
                    this.module.Types = new ClrType[typeCount];
            }

            var fieldDefEntries = (FieldEntry[])this.tableStream.Tables[(int)TableKind.Field];

            for (int i = isFirstTypeModuleStub ? 1 : 0; i < typeDefEntries.Length; i++)
            {
                int typeIndex = isFirstTypeModuleStub ? i-1 : i;
                var typeDefEntry = typeDefEntries[i];

                var type = this.module.Types[typeIndex] ?? (this.module.Types[typeIndex] = new ClrType());

                type.Name = typeDefEntry.TypeName;
                type.Namespace = typeDefEntry.TypeNamespace;
                type.Attributes = typeDefEntry.Flags;

                SetBaseType(typeDefEntries, isFirstTypeModuleStub, typeDefEntry, type);

                SetFields(typeDefEntries, fieldDefEntries, i, typeDefEntry, type);
            }
        }

        static void SetFields(TypeDefEntry[] typeDefEntries, FieldEntry[] fieldDefEntries, int typeDefIndex, TypeDefEntry typeDefEntry, ClrType type)
        {
            if (fieldDefEntries == null)
            {
                type.Fields = null;
            }
            else
            {
                uint firstFieldIndex = typeDefEntry.FieldList;
                if (firstFieldIndex > fieldDefEntries.Length)
                {
                    type.Fields = null;
                }
                else
                {
                    uint fieldCount;
                    if (typeDefIndex < typeDefEntries.Length - 1)
                    {
                        fieldCount = typeDefEntries[typeDefIndex + 1].FieldList - typeDefEntry.FieldList;
                    }
                    else
                    {
                        fieldCount = 0;
                    }

                    type.Fields = new ClrField[fieldCount];

                    if (firstFieldIndex + fieldCount > fieldDefEntries.Length)
                        fieldCount = (uint)fieldDefEntries.Length - firstFieldIndex;

                    for (uint i = 0; i < fieldCount; i++)
                    {
                        uint fieldIndex = typeDefEntry.FieldList + i;

                        if (fieldIndex == 0)
                        {
                            type.Fields[i] = null;
                            continue;
                        }

                        fieldIndex--;

                        var fieldDefEntry = fieldDefEntries[fieldIndex];

                        var field = new ClrField();
                        field.Name = fieldDefEntry.Name;

                        type.Fields[i] = field;
                    }
                }
            }
        }

        void SetBaseType(TypeDefEntry[] typeDefEntries, bool isFirstTypeModuleStub, TypeDefEntry typeDefEntry, ClrType type)
        {
            var extends = typeDefEntry.Extends;
            if (extends.Index == 0)
            {
                type.BaseType = null;
            }
            else
            {
                if (extends.TableKind == TableKind.TypeRef)
                {
                    var typeRefEntries = (TypeRefEntry[])this.tableStream.Tables[(int)TableKind.TypeRef];
                    if (typeRefEntries == null || extends.Index >= typeRefEntries.Length)
                    {
                        type.BaseType = null;
                    }
                    else
                    {
                        var externalBaseTypeRefEntry = typeRefEntries[extends.Index];
                        var externalTypeReference = new TypeReference.External();
                        externalTypeReference.Name = externalBaseTypeRefEntry.TypeName;
                        externalTypeReference.Namespace = externalBaseTypeRefEntry.TypeNamespace;
                    }
                }
                else if (extends.TableKind == TableKind.TypeDef)
                {
                    if (typeDefEntries == null || extends.Index >= typeDefEntries.Length)
                    {
                        type.BaseType = null;
                    }
                    else
                    {
                        uint baseTypeIndex = extends.Index - 1;
                        if (isFirstTypeModuleStub)
                            baseTypeIndex--;

                        type.BaseType = this.module.Types[baseTypeIndex] ?? (this.module.Types[extends.Index] = new ClrType());
                    }
                }
            }
        }
    }
}