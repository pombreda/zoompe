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

        const int BitCount = 2;
        
        const uint WideKindMask = uint.MaxValue << BitCount;
        const ushort NarrowKindMask = unchecked((ushort)(ushort.MaxValue << BitCount));

        readonly uint value;

        private HasConstant(uint value)
        {
            this.value = value;
        }

        public TableKind Kind { get { return (TableKind)(value >> (32 - BitCount)); } }
        public uint Index { get { return value & ~WideKindMask; } }

        public static explicit operator HasConstant(uint value)
        {
            return new HasConstant(value);
        }

        public static explicit operator HasConstant(ushort value)
        {
            ushort high = (ushort)(value & NarrowKindMask);
            ushort low = (ushort)(value & ~NarrowKindMask);

            uint extended = (uint)((high << 16) | low);

            return new HasConstant(extended);
        }

        public static explicit operator uint(HasConstant value)
        {
            return value.value;
        }

        public static explicit operator ushort(HasConstant value)
        {
            ushort high = (ushort)(value.value >> 16);
            ushort low = (ushort)(value.value & ushort.MaxValue);

            ushort compacted = (ushort)(high | low);

            return compacted;
        }
    }
}