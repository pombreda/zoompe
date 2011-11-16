using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Unmanaged
{
    partial struct RelocationType
    {
        public enum M32R : short
        {
            /// <summary>
            /// The relocation is ignored.
            /// </summary>
            Absolute = 0x0000,

            /// <summary>
            /// The target’s 32-bit VA.
            /// </summary>
            Addr32 = 0x0001,

            /// <summary>
            /// The target’s 32-bit RVA.
            /// </summary>
            Addr32NB = 0x0002,

            /// <summary>
            /// The target’s 24-bit VA.
            /// </summary>
            Addr24 = 0x0003,

            /// <summary>
            /// The target’s 16-bit offset from the GP register.
            /// </summary>
            GPRel16 = 0x0004,

            /// <summary>
            /// The target’s 24-bit offset from the program  counter (PC), shifted left by 2 bits and signextended
            /// </summary>
            PCRel24 = 0x0005,

            /// <summary>
            /// The target’s 16-bit offset from the PC, shifted left  by 2 bits and sign-extended
            /// </summary>
            PCRel16 = 0x0006,

            /// <summary>
            /// The target’s 8-bit offset from the PC, shifted left  by 2 bits and sign-extended
            /// </summary>
            PCRel8 = 0x0007,

            /// <summary>
            /// The 16 MSBs of the target VA.
            /// </summary>
            RefHalf = 0x0008,

            /// <summary>
            /// The 16 MSBs of the target VA, adjusted for  LSB sign extension. This is used for the first  instruction in a two-instruction sequence that  loads a full 32-bit address. This relocation must  be immediately followed by a PAIR relocation  whose SymbolTableIndex contains a signed 16-  bit displacement that is added to the upper 16  bits that are taken from the location that is being  relocated.
            /// </summary>
            RefHI = 0x0009,

            /// <summary>
            /// The 16 LSBs of the target VA.
            /// </summary>
            RefLO = 0x000A,

            /// <summary>
            /// The relocation must follow the REFHI relocation.  Its SymbolTableIndex contains a displacement  and not an index into the symbol table.
            /// </summary>
            Pair = 0x000B,

            /// <summary>
            /// The 16-bit section index of the section that  contains the target. This is used to support  debugging information.
            /// </summary>
            Section = 0x000C,

            /// <summary>
            /// The 32-bit offset of the target from the beginning  of its section. This is used to support debugging  information and static thread local storage.
            /// </summary>
            SecRel = 0x000D,

            /// <summary>
            /// The CLR token
            /// </summary>
            Token = 0x000E
        }
    }
}