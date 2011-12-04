using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    /// <summary>
    /// One or more rows can refer to the same row in the <see cref="TableKind.MethodDef"/> or <see cref="TableKind.MemberRef"/> table.
    /// (There can be multiple instantiations of the same generic method.)
    /// [ECMA 22.29]
    /// </summary>
    public sealed class MethodSpecEntry
    {
        /// <summary>
        /// An index into the <see cref="TableKind.MethodDef"/> or <see cref="TableKind.MemberRef"/> table,
        /// specifying to which generic method this row refers;
        /// that is, which generic method this row is an instantiation of;
        /// more precisely, a <see cref="MethodDefOrRef"/> (ECMA §24.2.6) coded index.
        /// </summary>
        public MethodDefOrRef Method;

        /// <summary>
        /// The signature of this instantiation.
        /// The signature stored at <see cref="Instantiation"/> shall be a valid instantiation
        /// of the signature of the generic method stored at <see cref="Method "/> [ERROR]
        /// </summary>
        public byte[] Instantiation;

        public void Read(ClrModuleReader reader)
        {
            this.Method = reader.ReadMethodDefOrRef();
            this.Instantiation = reader.ReadBlob();
        }
    }
}