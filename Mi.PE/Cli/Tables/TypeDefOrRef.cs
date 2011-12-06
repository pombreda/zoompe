using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    public struct TypeDefOrRef
    {
        public enum TableKind
        {
            TypeDef = 0,
            TypeRef = 1,
            TypeSpec = 2
        }

        public const int LowBitCount = 2;

        const uint WideKindMask = uint.MaxValue << LowBitCount;
        const ushort NarrowKindMask = unchecked((ushort)(ushort.MaxValue << LowBitCount));

        readonly uint value;

        private TypeDefOrRef(uint value)
        {
            this.value = value;
        }

        public TableKind Kind { get { return (TableKind)(value & (1U << LowBitCount)); } }
        public uint Index { get { return (uint)(value >> LowBitCount); } }

        public static explicit operator TypeDefOrRef(uint value)
        {
            return new TypeDefOrRef(value);
        }

        public static explicit operator uint(TypeDefOrRef value)
        {
            return value.value;
        }
    }
}