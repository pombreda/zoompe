using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    public struct HasSemantics
    {
        public enum TableKind
        {
            Event = 0,
            Property = 1
        }

        public const int LowBitCount = 1;

        const uint WideKindMask = uint.MaxValue << LowBitCount;
        const ushort NarrowKindMask = unchecked((ushort)(ushort.MaxValue << LowBitCount));

        readonly uint value;

        private HasSemantics(uint value)
        {
            this.value = value;
        }

        public TableKind Kind { get { return (TableKind)(value & (1U << LowBitCount)); } }
        public uint Index { get { return (uint)(value >> LowBitCount); } }

        public static explicit operator HasSemantics(uint value)
        {
            return new HasSemantics(value);
        }

        public static explicit operator uint(HasSemantics value)
        {
            return value.value;
        }
    }
}