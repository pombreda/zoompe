using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    public struct TypeOrMethodDef
    {
        public enum TableKind
        {
            TypeDef = 0,
            MethodDef = 1
        }

        public const int HighBitCount = 1;

        const uint WideKindMask = uint.MaxValue << HighBitCount;
        const ushort NarrowKindMask = unchecked((ushort)(ushort.MaxValue << HighBitCount));

        readonly uint value;

        private TypeOrMethodDef(uint value)
        {
            this.value = value;
        }

        public TableKind Kind { get { return (TableKind)(value >> (32 - HighBitCount)); } }
        public uint Index { get { return value & ~WideKindMask; } }

        public static explicit operator TypeOrMethodDef(uint value)
        {
            return new TypeOrMethodDef(value);
        }

        public static explicit operator TypeOrMethodDef(ushort value)
        {
            ushort high = (ushort)(value & NarrowKindMask);
            ushort low = (ushort)(value & ~NarrowKindMask);

            uint extended = (uint)((high << 16) | low);

            return new TypeOrMethodDef(extended);
        }

        public static explicit operator uint(TypeOrMethodDef value)
        {
            return value.value;
        }

        public static explicit operator ushort(TypeOrMethodDef value)
        {
            ushort high = (ushort)(value.value >> 16);
            ushort low = (ushort)(value.value & ushort.MaxValue);

            ushort compacted = (ushort)(high | low);

            return compacted;
        }
    }
}