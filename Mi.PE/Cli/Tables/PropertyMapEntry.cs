using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    /// <summary>
    /// The <see cref="TableKind.PropertyMap"/> and <see cref="TableKind.Property"/> tables result from putting the .property directive on a class (ECMA §17).
    /// [ECMA 22.35]
    /// </summary>
    public sealed class PropertyMapEntry
    {
        /// <summary>
        /// An index into the <see cref="TableKind.TypeDef"/> table.
        /// </summary>
        public uint Parent;

        /// <summary>
        /// An index into the Property table.
        /// It marks the first of a contiguous run of Properties owned by <see cref="Parent"/>.
        /// </summary>
        public uint PropertyList;
    }
}