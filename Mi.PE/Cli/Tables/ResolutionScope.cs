using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    public struct ResolutionScope
    {
        public enum TableKind
        {
            Module = 0,
            ModuleRef = 1,
            AssemblyRef = 2,
            TypeRef = 3
        }

        const int BitCount = 2;
        
        const uint WideKindMask = uint.MaxValue << BitCount;
        const ushort NarrowKindMask = unchecked((ushort)(ushort.MaxValue << BitCount));

        readonly uint value;

        private ResolutionScope(uint value)
        {
            this.value = value;
        }

        public TableKind Kind { get { return (TableKind)(value >> (32 - BitCount)); } }
        public uint Index { get { return value & ~WideKindMask; } }

        public static explicit operator ResolutionScope(uint value)
        {
            return new ResolutionScope(value);
        }

        public static explicit operator ResolutionScope(ushort value)
        {
            ushort high = (ushort)(value & NarrowKindMask);
            ushort low = (ushort)(value & ~NarrowKindMask);

            uint extended = (uint)((high << 16) | low);

            return new ResolutionScope(extended);
        }

        public static explicit operator uint(ResolutionScope value)
        {
            return value.value;
        }

        public static explicit operator ushort(ResolutionScope value)
        {
            ushort high = (ushort)(value.value >> 16);
            ushort low = (ushort)(value.value & ushort.MaxValue);

            ushort compacted = (ushort)(high | low);

            return compacted;
        }
    }
}