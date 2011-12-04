using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    /// <summary>
    /// The <see cref="TableKind.CustomAttribute"/> table stores data that can be used to instantiate a Custom Attribute
    /// (more precisely, an object of the specified Custom Attribute class) at runtime.
    /// The column called <see cref="Type"/> is slightly misleading —
    /// it actually indexes a constructor method —
    /// the owner of that constructor method is the Type of the Custom Attribute.
    /// A row in the CustomAttribute table for a parent is created by the .custom attribute,
    /// which gives the value of the Type column and optionally that of the <see cref="Value"/> column (ECMA §21).
    /// [ECMA 22.10]
    /// </summary>
    /// <remarks>
    /// All binary values are stored in little-endian format
    /// (except for PackedLen items, which are used only as a count for the number of bytes to follow in a UTF8 string).
    /// </remarks>
    public struct CustomAttributeEntry
    {
        /// <summary>
        /// Parent can be an index into any metadata table, except the <see cref="TableKind.CustomAttribute"/> table itself  [ERROR]
        /// </summary>
        public HasCustomAttribute Parent;

        /// <summary>
        /// Type shall index a valid row in the <see cref="TableKind.Method"/> or <see cref="TableKind.MemberRef"/> table.
        /// That row shall be a constructor method
        /// (for the class of which this information forms an instance)  [ERROR]
        /// </summary>
        public CustomAttributeType Type;

        public byte[] Value;

        public void Read(ClrModuleReader reader)
        {
            this.Parent = reader.ReadHasCustomAttribute();
            this.Type = reader.ReadCustomAttributeType();
            this.Value = reader.ReadBlob();
        }
    }
}