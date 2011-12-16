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

            if (index > length)
                throw new FormatException("Index "+index+" is out of range [0.."+length+"] for "+table+" table.");

            return index;
        }

        public CodedIndex<TCodedIndexDefinition> ReadCodedIndex<TCodedIndexDefinition>()
            where TCodedIndexDefinition : struct, ICodedIndexDefinition
        {
            var tables = default(TCodedIndexDefinition).Tables;

            ushort mask = (ushort)(ushort.MaxValue >> tables.Length);

            int length = 0;
            foreach (var tab in tables)
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
                var def = default(TCodedIndexDefinition);
                throw new FormatException(
                    "Coded index "+typedResult+" is out of bound for "+typeof(TCodedIndexDefinition).Name+" ("+string.Join(",", def.Tables.Select(t => t.ToString()).ToArray())+").");
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
        }
    }
}