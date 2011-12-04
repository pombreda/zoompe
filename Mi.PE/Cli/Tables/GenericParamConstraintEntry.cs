using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    /// <summary>
    /// [ECMA 22.21]
    /// </summary>
    public sealed class GenericParamConstraintEntry
    {
        /// <summary>
        /// An index into the <see cref="TableKind.GenericParam"/> table, specifying to which generic parameter this row refers.
        /// </summary>
        public uint Owner;

        /// <summary>
        /// An index into the <see cref="TableKind.TypeDef"/>, <see cref="TableKind.TypeRef"/>, or <see cref="TableKind.TypeSpec"/> tables,
        /// specifying from which class this generic parameter is constrained to derive;
        /// or which interface this generic parameter is constrained to implement;
        /// more precisely, a <see cref="TypeDefOrRef"/> (ECMA §24.2.6) coded index.
        /// </summary>
        public TypeDefOrRef Constraint;
    }
}