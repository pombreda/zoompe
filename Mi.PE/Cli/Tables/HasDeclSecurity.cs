using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    public struct HasDeclSecurity
    {
        public enum TableKind
        {
            TypeDef = 0,
            MethodDef = 1,
            Assembly = 2
        }

        public const int HighBitCount = 2;

        const uint WideKindMask = uint.MaxValue << HighBitCount;
        const ushort NarrowKindMask = unchecked((ushort)(ushort.MaxValue << HighBitCount));

        readonly uint value;

        private HasDeclSecurity(uint value)
        {
            this.value = value;
        }

        public TableKind Kind { get { return (TableKind)(value >> (32 - HighBitCount)); } }
        public uint Index { get { return value & ~WideKindMask; } }

        public static explicit operator HasDeclSecurity(uint value)
        {
            return new HasDeclSecurity(value);
        }

        public static explicit operator HasDeclSecurity(ushort value)
        {
            ushort high = (ushort)(value & NarrowKindMask);
            ushort low = (ushort)(value & ~NarrowKindMask);

            uint extended = (uint)((high << 16) | low);

            return new HasDeclSecurity(extended);
        }

        public static explicit operator uint(HasDeclSecurity value)
        {
            return value.value;
        }

        public static explicit operator ushort(HasDeclSecurity value)
        {
            ushort high = (ushort)(value.value >> 16);
            ushort low = (ushort)(value.value & ushort.MaxValue);

            ushort compacted = (ushort)(high | low);

            return compacted;
        }
    }
}