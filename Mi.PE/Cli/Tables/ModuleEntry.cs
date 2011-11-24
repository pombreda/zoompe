using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.Cli.Tables
{
    public sealed class ModuleEntry
    {
        public ushort Generation;
        public string Name; // -> StringHeap
        public Guid? Mvid; // -> GuidHeap
        public Guid? EncId; // -> GuidHeap
        public Guid? EncBaseId; // -> GuidHeap

        public void Read(ClrModuleReader reader)
        {
            this.Generation = reader.Binary.ReadUInt16();
            this.Name = reader.ReadString();
            this.Mvid = reader.ReadGuid();
            this.EncId = reader.ReadGuid();
            this.EncBaseId = reader.ReadGuid();
        }
    }
}
