using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    /// <summary>
    /// The first row of the TypeDef table represents the pseudo class that acts as parent for functions and variables defined at module scope.
    /// [ECMA 22.38]
    /// </summary>
    public sealed class TypeRefEntry
    {
        public ResolutionScope ResolutionScope;

        /// <summary>
        /// Shall index a non-empty string  in the String heap. [ERROR]
        /// </summary>
        public string TypeName;

        /// <summary>
        /// Can be null or non-null.
        /// If non-null, then TypeNamespace shall index a non-empty string in the String heap. [ERROR]
        /// </summary>
        public string TypeNamespace;

        public void Read(ClrModuleReader reader)
        {
            this.ResolutionScope = reader.Binary.ReadResolutionScope();
            this.TypeName = reader.ReadString();
            this.TypeNamespace = reader.ReadString();
        }
    }
}
