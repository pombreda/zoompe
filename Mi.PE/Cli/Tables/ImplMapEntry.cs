using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    /// <summary>
    /// The <see cref="TabeKind.ImplMap"/> table holds information about unmanaged methods
    /// that can be reached from managed code, using PInvoke dispatch.
    /// Each row of the <see cref="TableKind.ImplMap"/> table associates a row in the <see cref="TableKind.MethodDef"/> table
    /// (<see cref="MemberForwarded"/>)
    /// with the name of a routine (<see cref="ImportName"/>) in some unmanaged DLL (<see cref="ImportScope"/>).  
    /// [ECMA 22.22]
    /// </summary>
    public sealed class ImplMapEntry
    {
        /// <summary>
        /// A 2-byte bitmask of type <see cref="PInvokeAttributes"/>, ECMA §23.1.8.
        /// </summary>
        public PInvokeAttributes MappingFlags;

        /// <summary>
        /// An index into the <see cref="TableKind.Field"/> or <see cref="TableKind.MethodDef"/> table;
        /// more precisely, a <see cref="MemberForwarded"/> (ECMA §24.2.6) coded index.
        /// However, it only ever indexes the <see cref="TableKind.MethodDef"/> table, since Field export is not supported.
        /// </summary>
        public MemberForwarded MemberForwarded;

        public string ImportName;

        /// <summary>
        /// An index into the ModuleRef table.
        /// </summary>
        public uint ImportScope;
    }
}