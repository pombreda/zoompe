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

        public const int LowBitCount = 2;

        const uint WideKindMask = uint.MaxValue << LowBitCount;
        const ushort NarrowKindMask = unchecked((ushort)(ushort.MaxValue << LowBitCount));

        readonly uint value;

        private ResolutionScope(uint value)
        {
            this.value = value;
        }

        public TableKind Kind { get { return (TableKind)(value & (1U << LowBitCount)); } }
        public uint Index { get { return (uint)(value >> LowBitCount); } }

        public static explicit operator ResolutionScope(uint value)
        {
            return new ResolutionScope(value);
        }

        public static explicit operator uint(ResolutionScope value)
        {
            return value.value;
        }
    }
}