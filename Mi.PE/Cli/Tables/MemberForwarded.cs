using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    public struct MemberForwarded
    {
        public enum TableKind
        {
            Field = 0,
            MethodDef = 1
        }

        public const int LowBitCount = 1;

        const uint WideKindMask = uint.MaxValue << LowBitCount;
        const ushort NarrowKindMask = unchecked((ushort)(ushort.MaxValue << LowBitCount));

        readonly uint value;

        private MemberForwarded(uint value)
        {
            this.value = value;
        }

        public TableKind Kind { get { return (TableKind)(value & (1U << LowBitCount)); } }
        public uint Index { get { return (uint)(value >> LowBitCount); } }

        public static explicit operator MemberForwarded(uint value)
        {
            return new MemberForwarded(value);
        }

        public static explicit operator uint(MemberForwarded value)
        {
            return value.value;
        }
    }
}