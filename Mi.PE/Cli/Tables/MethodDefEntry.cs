using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    using Mi.PE.Cli.Signatures;

    /// <summary>
    /// Conceptually, every row in the <see cref="TableKind.MethodDef"/> table is owned by one, and only one, row in the <see cref="TableKind.TypeDef"/> table.
    /// The rows in the MethodDef table result from .method directives (ECMA-335 §15).
    /// The <see cref="MethodDefEntry.RVA"/> column is computed when the image for the PE file is emitted
    /// and points to the COR_ILMETHOD structure
    /// for the body of the method (ECMA-335 §25.4) 
    /// [Note: If Signature is GENERIC (0x10), the generic arguments are described in the GenericParam table (ECMA-335 §22.20). 
    /// end note]
    /// [ECMA-335 §22.26]
    /// </summary>
    public struct MethodDefEntry
    {
        public uint RVA;
        public MethodImplAttributes ImplFlags;
        public MethodAttributes Flags;
        public string Name;
        public MethodSig Signature;
        public uint ParamList;

        public void Read(ClrModuleReader reader)
        {
            this.RVA = reader.Binary.ReadUInt32();
            this.ImplFlags = (MethodImplAttributes)reader.Binary.ReadUInt16();
            this.Flags = (MethodAttributes)reader.Binary.ReadUInt16();
            this.Name = reader.ReadString();
            this.Signature = reader.ReadMethodSignature();
            this.ParamList = reader.ReadTableIndex(TableKind.Param);
        }

        public override string ToString()
        {
            return
                this.Signature == null ? this.Name + "()" :
                this.Signature.RefType + " " + this.Name + "(" +
                (this.Signature.ParamList == null ? "" :
                string.Join(", ", this.Signature.ParamList.Select(t => t.ToString()).ToArray())) + ")";

        }
    }
}