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
    public struct ParamEntry
    {
        /// <summary>
        /// If Flags.HasDefault = 1 then this row shall own exactly one row in the Constant table [ERROR]
        /// If Flags.HasDefault = 0, then there shall be no rows in the Constant table owned by this row [ERROR]
        /// if Flags.FieldMarshal = 1 then this row shall own exactly one row in the FieldMarshal table [ERROR]
        /// </summary>
        public ParamAttributes Flags;

        /// <summary>
        /// <see cref="Sequence"/> shall have a value >= 0 and <= number of parameters in owner method.
        /// A  <see cref="Sequence"/> value of 0 refers to the owner method‘s return type;
        /// its parameters are then numbered from 1 onwards  [ERROR]
        /// Successive rows of the <see cref="TableKind.Param"/> table that are owned by the same method
        /// shall be ordered by increasing Sequence value -
        /// although gaps in the sequence are allowed  [WARNING]
        /// </summary>
        public ushort Sequence;

        public string Name;

        public void Read(ClrModuleReader reader)
        {
            this.Flags = (ParamAttributes)reader.Binary.ReadUInt16();
            this.Sequence = reader.Binary.ReadUInt16();
            this.Name = reader.ReadString();
        }
    }
}