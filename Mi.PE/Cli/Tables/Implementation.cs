using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    public struct Implementation
    {
        public enum TableKind
        {
            File = 0,
            AssemblyRef = 1,
            ExportedType = 2
        }

        public const int LowBitCount = 2;

        const uint WideKindMask = uint.MaxValue << LowBitCount;
        const ushort NarrowKindMask = unchecked((ushort)(ushort.MaxValue << LowBitCount));

        readonly uint value;

        private Implementation(uint value)
        {
            this.value = value;
        }

        public TableKind Kind { get { return (TableKind)(value & (1U << LowBitCount)); } }
        public uint Index { get { return (uint)(value >> LowBitCount); } }

        public static explicit operator Implementation(uint value)
        {
            return new Implementation(value);
        }

        public static explicit operator uint(Implementation value)
        {
            return value.value;
        }
    }
}