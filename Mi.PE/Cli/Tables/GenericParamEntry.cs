using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    /// <summary>
    /// [ECMA 22.20]
    /// </summary>
    public sealed class GenericParamEntry
    {
        /// <summary>
        /// The 2-byte index of the generic parameter, numbered left-to-right, from zero.
        /// </summary>
        public ushort Number;

        /// <summary>
        /// A 2-byte bitmask of type <see cref="GenericParamAttributes"/>, ECMA §23.1.7.
        /// </summary>
        public GenericParamAttributes Flags;

        /// <summary>
        /// An index into the <see cref="TableKind.TypeDef"/> or <see cref="TableKind.MethodDef"/> table,
        /// specifying the Type or Method to which this generic parameter applies;
        /// more precisely, a <see cref="TypeOrMethodDef"/> (ECMA §24.2.6) coded index.
        /// </summary>
        public TypeOrMethodDef Owner;

        /// <summary>
        /// This is purely descriptive and is used only by source language compilers and by Reflection.
        /// </summary>
        public string Name;
    }
}