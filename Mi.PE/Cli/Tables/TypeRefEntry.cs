using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    /// <summary>
    /// The first row of the TypeDef table represents the pseudo class that acts as parent for functions and variables defined at module scope.
    /// </summary>
    public sealed class TypeRefEntry
    {
        public TypeAttributes Flags;

        /// <summary>
        /// Shall index a non-empty string  in the String heap. [ERROR]
        /// </summary>
        public string TypeName;

        /// <summary>
        /// Can be null or non-null.
        /// If non-null, then TypeNamespace shall index a non-empty string in the String heap. [ERROR]
        /// </summary>
        public string TypeNamespace;

        public TypeDefOrRef Extends;


        public uint FieldList;
        public uint MethodList;


    }
}
