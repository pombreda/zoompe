using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    /// <summary>
    /// The <see cref="CustomAttribute"/> table stores data that can be used to instantiate a Custom Attribute
    /// (more precisely, an object of the specified Custom Attribute class) at runtime.
    /// The column called <see cref="Type"/> is slightly misleading —
    /// it actually indexes a constructor method —
    /// the owner of that constructor method is the <see cref="Type"/> of the Custom Attribute.
    /// A row in the <see cref="CustomAttribute"/> table for a parent is created by the .custom attribute,
    /// which gives the value of the <see cref="Type"/> column and optionally that of the <see cref="Value"/> column (ECMA §21).
    /// </summary>
    public sealed class CustomAttribute
    {
        public HasConstant Parent;
        public HasCustomAttribute Type;
        public byte[] Value;

        public void Read(ClrModuleReader reader)
        {
            this.Parent = reader.ReadHasConstant();
            this.Type = reader.ReadHasCustomAttribute();
            this.Value = reader.ReadBlob();
        }
    }
}