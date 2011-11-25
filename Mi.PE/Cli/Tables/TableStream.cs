using System;
using System.Collections.Generic;
using System.Linq;
using Mi.PE.Internal;

namespace Mi.PE.Cli.Tables
{
    public sealed class TableStream
    {
        public Version Version;

        public AssemblyEntry[] AssemblyEntries;
        public AssemblyOSEntry[] AssemblyOSEntries;
        public AssemblyProcessorEntry[] AssemblyProcessorEntries;
        public AssemblyRefEntry[] AssemblyRefEntries;
        public AssemblyRefOSEntry[] AssemblyRefOSEntries;
        public AssemblyRefProcessorEntry[] AssemblyProcessorEntries;
        public ClassLayoutEntry[] ClassLayoutEntries;

        public ModuleEntry[] ModuleEntries;
        public TypeRefEntry[] TypeRefEntries;
        public TypeDefEntry[] TypeDefEntries;
        public FieldEntry[] FieldEntries;
        public MethodDefEntry[] MethodDefEntries;
        public ParamEntry[] ParamEntries;

        public void Read(BinaryStreamReader reader)
        {
            int tsReserved0 = reader.ReadInt32();
            byte tsMajorVersion = reader.ReadByte();
            byte tsMinorVersion = reader.ReadByte();

            this.Version = new Version(tsMajorVersion, tsMinorVersion);

            byte heapSizes = reader.ReadByte();
            byte reserved1 = reader.ReadByte();
            ulong valid = reader.ReadUInt64();
            ulong sorted = reader.ReadUInt64();

            ReadAndInitializeRowCounts(reader, valid);
        }

        private void ReadAndInitializeRowCounts(BinaryStreamReader reader, ulong validMask)
        {
            if ((validMask & (1 << (int)TableKind.Module)) != 0)
                this.ModuleEntries = new ModuleEntry[reader.ReadUInt32()];

            if ((validMask & (1 << (int)TableKind.TypeRef)) != 0)
                this.TypeRefEntries = new TypeRefEntry[reader.ReadUInt32()];

            if ((validMask & (1 << (int)TableKind.TypeDef)) != 0)
                this.TypeDefEntries = new TypeDefEntry[reader.ReadUInt32()];

            if ((validMask & (1 << (int)TableKind.Field)) != 0)
                this.FieldEntries = new FieldEntry[reader.ReadUInt32()];

            if ((validMask & (1 << (int)TableKind.MethodDef)) != 0)
                this.MethodDefEntries = new MethodDefEntry[reader.ReadUInt32()];

            if ((validMask & (1 << (int)TableKind.Param)) != 0)
                this.ParamEntries = new ParamEntry[reader.ReadUInt32()];
        }
    }
}