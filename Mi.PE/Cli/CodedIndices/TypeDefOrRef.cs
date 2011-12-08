﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.CodedIndices
{
    using Mi.PE.Cli.Tables;

    public struct TypeDefOrRef : ICodedIndexDefinition
    {
        public static TableKind[] Tables { get { return tables; } }

        static readonly TableKind[] tables = new[]
            { 
                TableKind.TypeDef,
                TableKind.TypeRef,
                TableKind.TypeSpec
            };
    }
}