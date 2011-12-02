using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    public struct MemberForwarded
    {
        public enum TableKind
        {
            Field = 0,
            MethodDef = 1
        }

        const int HighBitCount = 1;

        const uint WideKindMask = uint.MaxValue << HighBitCount;
        const ushort NarrowKindMask = unchecked((ushort)(ushort.MaxValue << HighBitCount));

        readonly uint value;

        private MemberForwarded(uint value)
        {
            this.value = value;
        }

        public TableKind Kind { get { return (TableKind)(value >> (32 - HighBitCount)); } }
        public uint Index { get { return value & ~WideKindMask; } }

        public static explicit operator MemberForwarded(uint value)
        {
            return new MemberForwarded(value);
        }

        public static explicit operator MemberForwarded(ushort value)
        {
            ushort high = (ushort)(value & NarrowKindMask);
            ushort low = (ushort)(value & ~NarrowKindMask);

            uint extended = (uint)((high << 16) | low);

            return new MemberForwarded(extended);
        }

        public static explicit operator uint(MemberForwarded value)
        {
            return value.value;
        }

        public static explicit operator ushort(MemberForwarded value)
        {
            ushort high = (ushort)(value.value >> 16);
            ushort low = (ushort)(value.value & ushort.MaxValue);

            ushort compacted = (ushort)(high | low);

            return compacted;
        }
    }
}