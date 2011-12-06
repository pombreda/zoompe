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

        public const int LowBitCount = 1;

        const uint WideKindMask = uint.MaxValue << LowBitCount;
        const ushort NarrowKindMask = unchecked((ushort)(ushort.MaxValue << LowBitCount));

        readonly uint value;

        private MethodDefOrRef(uint value)
        {
            this.value = value;
        }

        public TableKind Kind { get { return (TableKind)(value & (1U << LowBitCount)); } }
        public uint Index { get { return (uint)(value >> LowBitCount); } }

        public static explicit operator MethodDefOrRef(uint value)
        {
            return new MethodDefOrRef(value);
        }

        public static explicit operator uint(MethodDefOrRef value)
        {
            return value.value;
        }
    }
}