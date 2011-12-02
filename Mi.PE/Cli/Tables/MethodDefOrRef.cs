using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    public struct MethodDefOrRef
    {
        public enum TableKind
        {
            MethodDef = 0,
            MemberRef = 1
        }

        const int HighBitCount = 1;

        const uint WideKindMask = uint.MaxValue << HighBitCount;
        const ushort NarrowKindMask = unchecked((ushort)(ushort.MaxValue << HighBitCount));

        readonly uint value;

        private MethodDefOrRef(uint value)
        {
            this.value = value;
        }

        public TableKind Kind { get { return (TableKind)(value >> (32 - HighBitCount)); } }
        public uint Index { get { return value & ~WideKindMask; } }

        public static explicit operator MethodDefOrRef(uint value)
        {
            return new MethodDefOrRef(value);
        }

        public static explicit operator MethodDefOrRef(ushort value)
        {
            ushort high = (ushort)(value & NarrowKindMask);
            ushort low = (ushort)(value & ~NarrowKindMask);

            uint extended = (uint)((high << 16) | low);

            return new MethodDefOrRef(extended);
        }

        public static explicit operator uint(MethodDefOrRef value)
        {
            return value.value;
        }

        public static explicit operator ushort(MethodDefOrRef value)
        {
            ushort high = (ushort)(value.value >> 16);
            ushort low = (ushort)(value.value & ushort.MaxValue);

            ushort compacted = (ushort)(high | low);

            return compacted;
        }
    }
}