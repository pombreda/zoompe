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

        public const int LowBitCount = 1;

        const uint WideKindMask = uint.MaxValue << LowBitCount;
        const ushort NarrowKindMask = unchecked((ushort)(ushort.MaxValue << LowBitCount));

        readonly uint value;

        private TypeOrMethodDef(uint value)
        {
            this.value = value;
        }

        public TableKind Kind { get { return (TableKind)(value & (1U << LowBitCount)); } }
        public uint Index { get { return (uint)(value >> LowBitCount); } }

        public static explicit operator TypeOrMethodDef(uint value)
        {
            return new TypeOrMethodDef(value);
        }

        public static explicit operator uint(TypeOrMethodDef value)
        {
            return value.value;
        }
    }
}