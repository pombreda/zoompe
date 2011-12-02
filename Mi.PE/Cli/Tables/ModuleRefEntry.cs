using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    /// <summary>
    /// The rows in the <see cref="TableKind.ModuleRef"/> table result from .module extern directives in the Assembly (ECMA §6.5).
    /// [ECMA 22.31]
    /// </summary>
    public sealed class ModuleRefEntry
    {
        public string Name;

        public void Read(ClrModuleReader reader)
        {
            this.Name = reader.ReadString();
        }
    }
}