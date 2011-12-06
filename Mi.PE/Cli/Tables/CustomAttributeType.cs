using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    public struct CustomAttributeType
    {
        public enum TableKind
        {
            Not_used_0 = 0,
            Not_used_1 = 1,
            MethodDef = 2,
            MemberRef = 3,
            Not_used_4 = 4
        }

        public const int LowBitCount = 3;

        const uint WideKindMask = uint.MaxValue << LowBitCount;
        const ushort NarrowKindMask = unchecked((ushort)(ushort.MaxValue << LowBitCount));

        readonly uint value;

        private CustomAttributeType(uint value)
        {
            this.value = value;
        }

        public TableKind Kind { get { return (TableKind)(value & (1U << LowBitCount)); } }
        public uint Index { get { return (uint)(value >> LowBitCount); } }

        public static explicit operator CustomAttributeType(uint value)
        {
            return new CustomAttributeType(value);
        }

        public static explicit operator uint(CustomAttributeType value)
        {
            return value.value;
        }
    }
}