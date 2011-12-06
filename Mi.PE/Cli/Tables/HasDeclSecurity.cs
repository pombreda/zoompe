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

        public const int LowBitCount = 2;

        const uint WideKindMask = uint.MaxValue << LowBitCount;
        const ushort NarrowKindMask = unchecked((ushort)(ushort.MaxValue << LowBitCount));

        readonly uint value;

        private HasDeclSecurity(uint value)
        {
            this.value = value;
        }

        public TableKind Kind { get { return (TableKind)(value & (1U << LowBitCount)); } }
        public uint Index { get { return (uint)(value >> LowBitCount); } }

        public static explicit operator HasDeclSecurity(uint value)
        {
            return new HasDeclSecurity(value);
        }

        public static explicit operator uint(HasDeclSecurity value)
        {
            return value.value;
        }
    }
}