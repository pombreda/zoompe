using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mi.PE.Internal;
using Mi.PE.PEFormat;

namespace Mi.PE.Unmanaged
{
    public sealed class ImageRelocation
    {
        /// <summary>
        /// The address of the item to which relocation is applied.
        /// This is the offset from the beginning of the section,
        /// plus the value of the section’s RVA/Offset field.
        /// For example, if the first byte of the section has an address of 0x10,
        /// the third byte has an address of 0x12.
        /// </summary>
        public uint VirtualAddress;

        /// <summary>
        /// A zero-based index into the symbol table.
        /// This symbol gives the address that is to be used for the relocation.
        /// If the specified symbol has section storage class,
        /// then the symbol’s address is the address with the first section of the same name.
        /// </summary>
        public uint SymbolTableIndex;

        /// <summary>
        /// A value that indicates the kind of relocation that should be performed.
        /// Valid relocation types depend on machine type.
        /// </summary>
        public ushort Type;
    }
}