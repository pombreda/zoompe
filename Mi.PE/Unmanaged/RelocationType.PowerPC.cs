using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Unmanaged
{
    public enum PowerPCRelocationType : short
    {
        /// <summary>
        /// The relocation is ignored.
        /// </summary>
        Absolute = 0x0000,

        /// <summary>
        /// The 64-bit VA of the target.
        /// </summary>
        Addr64 = 0x0001,

        /// <summary>
        /// The 32-bit VA of the target.
        /// </summary>
        Addr32 = 0x0002,

        /// <summary>
        /// The low 24 bits of the VA of the target. This is valid only when the target symbol is absolute and can be sign-extended to its original value.
        /// </summary>
        Addr24 = 0x0003,

        /// <summary>
        /// The low 16 bits of the target’s VA.
        /// </summary>
        Addr16 = 0x0004,

        /// <summary>
        /// The low 14 bits of the target’s VA. This is valid only when the target symbol is absolute and can be sign-extended to its original value.
        /// </summary>
        Addr14 = 0x0005,

        /// <summary>
        /// A 24-bit PC-relative offset to the symbol’s location.
        /// </summary>
        Rel24 = 0x0006,

        /// <summary>
        /// A 14-bit PC-relative offset to the symbol’s location.
        /// </summary>
        Rel14 = 0x0007,

        /// <summary>
        /// The 32-bit RVA of the target.
        /// </summary>
        Addr32NB = 0x000A,

        /// <summary>
        /// The 32-bit offset of the target from the beginning of its section.
        /// This is used to support debugging information and static thread local storage.
        /// </summary>
        SecRel = 0x000B,

        /// <summary>
        /// The 16-bit section index of the section that contains the target.
        /// This is used to support debugging information.
        /// </summary>
        Section = 0x000C,
        
        /// <summary>
        /// The 16-bit offset of the target from the beginning of its section.
        /// This is used to support debugging information and static thread local storage.
        /// </summary>
        SecRel16 = 0x000F,

        /// <summary>
        /// The high 16 bits of the target’s 32-bit VA.
        /// This is used for the first instruction in a two-instruction sequence that loads a full address.
        /// This relocation must be immediately followed by a PAIR relocation whose SymbolTableIndex
        /// contains a signed 16-bit displacement that is added to the upper 16 bits
        /// that was taken from the location that is being relocated.
        /// </summary>
        RefHi = 0x0010,

        /// <summary>
        /// The low 16 bits of the target’s VA.
        /// </summary>
        RefLo = 0x0011,

        /// <summary>
        /// A relocation that is valid only when it immediately follows a <see cref="RefHi"/> or <see cref="SecRelHi"/> relocation.
        /// Its SymbolTableIndex contains a displacement and not an index into the symbol table.
        /// </summary>
        Pair = 0x0012,

        /// <summary>
        /// The low 16 bits of the 32-bit offset of the target from the beginning of its section.
        /// </summary>
        SecRelLo = 0x0013,

        /// <summary>
        /// The 16-bit signed displacement of the target relative to the GP register.
        /// </summary>
        GPRel = 0x0015,

        /// <summary>
        /// The CLR token.
        /// </summary>
        Token = 0x0016
    }
}