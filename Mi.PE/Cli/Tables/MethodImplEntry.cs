﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    /// <summary>
    /// <see cref="TableKind.MethodImpl"/> tables let a compiler override the default inheritance rules provided by the CLI
    /// [ECMA 22.27]
    /// </summary>
    public sealed class MethodImplEntry
    {
        /// <summary>
        /// An index into the <see cref="TableKind.TypeDef"/> table.
        /// ILAsm uses the .override directive to specify the rows of the <see cref="TableKind.MethodImpl"/> table (ECMA §10.3.2 and ECMA §15.4.1).
        /// </summary>
        public uint Class;

        /// <summary>
        /// An index into the MethodDef or MemberRef table;
        /// more precisely, a <see cref="MethodDefOrRef"/> (ECMA §24.2.6) coded index.
        /// 
        /// The method indexed by <see cref="MethodBody"/> shall be virtual. [ERROR]
        /// 
        /// The method indexed by <see cref="MethodBody"/> shall have its Method.RVA != 0
        /// (cannot be an unmanaged method reached via PInvoke, for example). [ERROR]
        /// </summary>
        public MethodDefOrRef MethodBody;

        /// <summary>
        /// An index into the <see cref="TableKind.MethodDef"/> or <see cref="TableKind.MemberRef"/> table;
        /// more precisely, a <see cref="MethodDefOrRef"/> (ECMA §24.2.6) coded index.
        /// The method indexed by <see cref="MethodDeclaration"/> shall have Flags.Virtual set. [ERROR]
        /// </summary>
        public MethodDefOrRef MethodDeclaration;

        public void Read(ClrModuleReader reader)
        {
            this.Class = reader.ReadTableIndex(TableKind.TypeDef);
            this.MethodBody = reader.ReadMethodDefOrRef();
            this.MethodDeclaration = reader.ReadMethodDefOrRef();
        }
    }
}