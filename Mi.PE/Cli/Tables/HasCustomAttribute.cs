using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    /// <summary>
    /// <see cref="HasCustomAttributes"/> only has values for tables that are ―externally visible;
    /// that is, that correspond to items in a user source program.
    /// For example, an attribute can be attached to a <see cref="Mi.PE.Cli.Tables.TableKind.TypeDef"/> table
    /// and a <see cref="Mi.PE.Cli.Tables.TableKind.Field"/> table,
    /// but not a <see cref="Mi.PE.Cli.Tables.TableKind.ClassLayout"/> table.
    /// As a result, some table types are missing from <see cref="HasCustomAttribute.TableKind"/> enum.
    /// </summary>
    public struct HasCustomAttribute
    {
        public enum TableKind
        {
            MethodDef = 0,
            Field = 1,
            TypeRef = 2,
            TypeDef = 3,
            Param = 4,
            InterfaceImpl = 5,
            MemberRef = 6,
            Module = 7,
            Permission = 8,
            Property = 9,
            Event = 10,
            StandAloneSig = 11,
            ModuleRef = 12,
            TypeSpec = 13,
            Assembly = 14,
            AssemblyRef = 15,
            File = 16,
            ExportedType = 17,
            ManifestResource = 18,
            GenericParam = 19,
            GenericParamConstraint = 20,
            MethodSpec = 21
        }

        public const int LowBitCount = 5;

        const uint WideKindMask = uint.MaxValue << LowBitCount;
        const ushort NarrowKindMask = unchecked((ushort)(ushort.MaxValue << LowBitCount));

        readonly uint value;

        private HasCustomAttribute(uint value)
        {
            this.value = value;
        }

        public TableKind Kind { get { return (TableKind)(value & (1U << LowBitCount)); } }
        public uint Index { get { return (uint)(value >> LowBitCount); } }

        public static explicit operator HasCustomAttribute(uint value)
        {
            return new HasCustomAttribute(value);
        }

        public static explicit operator uint(HasCustomAttribute value)
        {
            return value.value;
        }
    }
}