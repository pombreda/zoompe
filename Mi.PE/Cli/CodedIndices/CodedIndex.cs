﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.CodedIndices
{
    using Mi.PE.Cli.Tables;

    public struct CodedIndex<TCodedIndexDefinition>
        where TCodedIndexDefinition : struct, ICodedIndexDefinition
    {
        static readonly TableKind[] Tables = default(TCodedIndexDefinition).Tables;
        static readonly int LowBitCount = CalcRequiredBitCount(Tables.Length);

        readonly uint value;

        private CodedIndex(uint value)
        {
            this.value = value;
        }

        public TableKind TableKind { get { return Tables[value & (1U << Tables.Length)]; } } // FIXME: not << Tables.Length, that's too much
        public uint Index { get { return (uint)(value >> Tables.Length); } }

        public static explicit operator uint(CodedIndex<TCodedIndexDefinition> codedIndex)
        {
            return codedIndex.value;
        }

        public static explicit operator CodedIndex<TCodedIndexDefinition>(uint value)
        {
            return new CodedIndex<TCodedIndexDefinition>(value);
        }

        static int CalcRequiredBitCount(int tableCount)
        {
            int bitMask = tableCount - 1;
            int result = 0;

            while (bitMask != 0)
            {
                result++;
                bitMask >>= 1;
            }

            return result;
        }

        public override string ToString()
        {
            return this.TableKind + "[" + this.Index+"]";
        }
    }
}