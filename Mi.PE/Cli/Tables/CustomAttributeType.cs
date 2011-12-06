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

        public const int HighBitCount = 3;

        const uint WideKindMask = uint.MaxValue << HighBitCount;
        const ushort NarrowKindMask = unchecked((ushort)(ushort.MaxValue << HighBitCount));

        readonly uint value;

        private CustomAttributeType(uint value)
        {
            this.value = value;
        }

        public TableKind Kind { get { return (TableKind)(value >> (32 - HighBitCount)); } }
        public uint Index { get { return value & ~WideKindMask; } }

        public static explicit operator CustomAttributeType(uint value)
        {
            return new CustomAttributeType(value);
        }

        public static explicit operator CustomAttributeType(ushort value)
        {
            ushort high = (ushort)(value & NarrowKindMask);
            ushort low = (ushort)(value & ~NarrowKindMask);

            uint extended = (uint)((high << 16) | low);

            return new CustomAttributeType(extended);
        }

        public static explicit operator uint(CustomAttributeType value)
        {
            return value.value;
        }

        public static explicit operator ushort(CustomAttributeType value)
        {
            ushort high = (ushort)(value.value >> 16);
            ushort low = (ushort)(value.value & ushort.MaxValue);

            ushort compacted = (ushort)(high | low);

            return compacted;
        }
    }
}