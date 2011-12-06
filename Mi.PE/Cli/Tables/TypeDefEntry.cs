using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    /// <summary>
    /// The first row of the <see cref="TableKind.TypeDef"/> table represents the pseudo class that acts as parent for functions and variables defined at module scope.
    /// [ECMA-335 22.37]
    /// </summary>
    public struct TypeDefEntry
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

        public void Read(ClrModuleReader reader)
        {
            this.Flags = (TypeAttributes)reader.Binary.ReadUInt32();
            this.TypeName = reader.ReadString();
            this.TypeNamespace = reader.ReadString();
            this.Extends = reader.ReadTypeDefOrRef();
            this.FieldList = reader.ReadTableIndex(TableKind.Field);
            this.MethodList = reader.ReadTableIndex(TableKind.MethodDef);
        }
    }
}
