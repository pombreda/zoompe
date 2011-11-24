using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli
{
    public struct TypeDefOrRef
    {
        public enum TableKind
        {
            TypeDef = 0,
            TypeRef = 1,
            TypeSpec = 2
        }

        const int HighBitCount = 2;

        const uint WideKindMask = uint.MaxValue << HighBitCount;
        const ushort NarrowKindMask = unchecked((ushort)(ushort.MaxValue << HighBitCount));

        readonly uint value;

        private TypeDefOrRef(uint value)
        {
            this.value = value;
        }

        public TableKind Kind { get { return (TableKind)(value >> (32 - HighBitCount)); } }
        public uint Index { get { return value & ~WideKindMask; } }

        public static explicit operator TypeDefOrRef(uint value)
        {
            return new TypeDefOrRef(value);
        }

        public static explicit operator TypeDefOrRef(ushort value)
        {
            ushort high = (ushort)(value & NarrowKindMask);
            ushort low = (ushort)(value & ~NarrowKindMask);

            uint extended = (uint)((high << 16) | low);

            return new TypeDefOrRef(extended);
        }

        public static explicit operator uint(TypeDefOrRef value)
        {
            return value.value;
        }

        public static explicit operator ushort(TypeDefOrRef value)
        {
            ushort high = (ushort)(value.value >> 16);
            ushort low = (ushort)(value.value & ushort.MaxValue);

            ushort compacted = (ushort)(high | low);

            return compacted;
        }
    }
}