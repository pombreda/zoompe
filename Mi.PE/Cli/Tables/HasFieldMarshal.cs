using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    public struct HasFieldMarshal
    {
        public enum TableKind
        {
            Field = 0,
            Param = 1
        }

        public const int LowBitCount = 1;

        const uint WideKindMask = uint.MaxValue << LowBitCount;
        const ushort NarrowKindMask = unchecked((ushort)(ushort.MaxValue << LowBitCount));

        readonly uint value;

        private HasFieldMarshal(uint value)
        {
            this.value = value;
        }

        public TableKind Kind { get { return (TableKind)(value & (1U << LowBitCount)); } }
        public uint Index { get { return (uint)(value >> LowBitCount); } }

        public static explicit operator HasFieldMarshal(uint value)
        {
            return new HasFieldMarshal(value);
        }

        public static explicit operator uint(HasFieldMarshal value)
        {
            return value.value;
        }
    }
}