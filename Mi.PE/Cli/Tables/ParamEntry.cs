using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    /// <summary>
    /// Conceptually, every row in the <see cref="TableKind.Param"/> table is owned by one, and only one, row in the <see cref="TableKind.MethodDef "/> table.
    /// The rows in the <see cref="TableKind.Param"/> table result from the parameters in a method declaration (ECMA §15.4),
    /// or from a .param attribute attached to a method (ECMA §15.4.1).
    /// [ECMA 22.33]
    /// </summary>
    public sealed class ParamEntry
    {
        public ParamAttributes Flags;
        public ushort Sequence;
        public string Name;

        public void Read(ClrModuleReader reader)
        {
            throw new NotImplementedException();
        }
    }
}