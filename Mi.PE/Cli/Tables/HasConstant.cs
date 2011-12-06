using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    public struct HasConstant
    {
        public enum TableKind
        {
            Field = 0,
            Param = 1,
            Property = 2
        }

        public const int LowBitCount = 2;
        
        const uint WideKindMask = uint.MaxValue << LowBitCount;
        const ushort NarrowKindMask = unchecked((ushort)(ushort.MaxValue << LowBitCount));

        readonly uint value;

        private HasConstant(uint value)
        {
            this.value = value;
        }

        public TableKind Kind { get { return (TableKind)(value & (1U << LowBitCount)); } }
        public uint Index { get { return (uint)(value >> LowBitCount); } }

        public static explicit operator HasConstant(uint value)
        {
            return new HasConstant(value);
        }

        public static explicit operator uint(HasConstant value)
        {
            return value.value;
        }
    }
}