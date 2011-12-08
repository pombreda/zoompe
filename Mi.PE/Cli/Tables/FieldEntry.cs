using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    /// <summary>
    /// Conceptually, each row in the <see cref="TableKind.Field"/> table is owned by one, and only one, row in the <see cref="TableKind.TypeDef"/> table.
    /// However, the owner of any row in the <see cref="TableKind.Field"/> table is not stored anywhere in the <see cref="TableKind.Field"/> table itself.
    /// There is merely a 'forward-pointer' from each row in the <see cref="TableKind.TypeDef"/> table
    /// (the <see cref="TypeDefEntry.FieldList"/> column).
    /// [ECMA-335 §22.15]
    /// </summary>
    /// <remarks>
    /// Each row in the Field table results from a top-level .field directive (ECMA-335 §5.10), or a .field directive inside a Type (ECMA-335 §10.2).
    /// </remarks>
    public struct FieldEntry
    {
        public FieldAttributes Flags;
        public string Name;
        public byte[] Signature;

        public void Read(ClrModuleReader reader)
        {
            this.Flags = (FieldAttributes)reader.Binary.ReadUInt16();
            this.Name = reader.ReadString();
            this.Signature = reader.ReadBlob();
        }
    }
}