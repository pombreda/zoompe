using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Conceptually, each row in the <see cref="TableKind.Field"/> table is owned by one, and only one, row in the <see cref="TableKind.TypeDef"/> table.
    /// However, the owner of any row in the <see cref="TableKind.Field"/> table is not stored anywhere in the <see cref="TableKind.Field"/> table itself.
    /// There is merely a 'forward-pointer' from each row in the <see cref="TableKind.TypeDef"/> table
    /// (the <see cref="TypeDefEntry.FieldList"/> column).  
    /// </remarks>
    public sealed class FieldEntry
    {
        public FieldAttributes Flags;
        public string Name;
        public byte[] Signature;
    }
}