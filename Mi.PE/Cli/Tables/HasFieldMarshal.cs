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

        const int HighBitCount = 1;

        const uint WideKindMask = uint.MaxValue << HighBitCount;
        const ushort NarrowKindMask = unchecked((ushort)(ushort.MaxValue << HighBitCount));

        readonly uint value;

        private HasFieldMarshal(uint value)
        {
            this.value = value;
        }

        public TableKind Kind { get { return (TableKind)(value >> (32 - HighBitCount)); } }
        public uint Index { get { return value & ~WideKindMask; } }

        public static explicit operator HasFieldMarshal(uint value)
        {
            return new HasFieldMarshal(value);
        }

        public static explicit operator HasFieldMarshal(ushort value)
        {
            ushort high = (ushort)(value & NarrowKindMask);
            ushort low = (ushort)(value & ~NarrowKindMask);

            uint extended = (uint)((high << 16) | low);

            return new HasFieldMarshal(extended);
        }

        public static explicit operator uint(HasFieldMarshal value)
        {
            return value.value;
        }

        public static explicit operator ushort(HasFieldMarshal value)
        {
            ushort high = (ushort)(value.value >> 16);
            ushort low = (ushort)(value.value & ushort.MaxValue);

            ushort compacted = (ushort)(high | low);

            return compacted;
        }
    }
}