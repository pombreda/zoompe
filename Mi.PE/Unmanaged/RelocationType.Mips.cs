using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Unmanaged
{
    partial struct RelocationType
    {
        public enum Mips : short
        {
            /// <summary>
            /// The relocation is ignored.
            /// </summary>
            Absolute = 0x0000,

            /// <summary>
            /// The high 16 bits of the target’s 32-bit VA.
            /// </summary>
            RefHalf = 0x0001,

            /// <summary>
            /// The target’s 32-bit VA.
            /// </summary>
            RefWord = 0x0002,

            /// <summary>
            /// The low 26 bits of the target’s VA. This supports the MIPS J and JAL instructions.
            /// </summary>
            JmpAddr = 0x0003,

            /// <summary>
            /// The high 16 bits of the target’s 32-bit VA. This is used for the first instruction in a two-instruction sequence that loads a full address. This relocation must be immediately followed by a PAIR relocation whose SymbolTableIndex contains a signed 16-bit displacement that is added to the upper 16 bits that are taken from the location that is being relocated.
            /// </summary>
            RefHi = 0x0004,

            /// <summary>
            /// The low 16 bits of the target’s VA.
            /// </summary>
            RefLo = 0x0005 ,

            /// <summary>
            /// A 16-bit signed displacement of the target  relative to the GP register.
            /// </summary>
            GPRel = 0x0006 ,

            /// <summary>
            /// The same as <see cref="GPRel"/>.
            /// </summary>
            Literal = 0x0007 ,

            /// <summary>
            /// The 16-bit section index of the section contains  the target. This is used to support debugging  information.
            /// </summary>
            Section = 0x000A ,

            /// <summary>
            /// The 32-bit offset of the target from the  beginning of its section. This is used to support  debugging information and static thread local  storage.
            /// </summary>
            SecRel = 0x000B ,

            /// <summary>
            /// The low 16 bits of the 32-bit offset of the target  from the beginning of its section.
            /// </summary>
            SecRelLo = 0x000C ,

            /// <summary>
            /// The high 16 bits of the 32-bit offset of the  target from the beginning of its section. 
            /// A <see cref="Pair"/> relocation must immediately follow this one.
            /// The SymbolTableIndex of the <see cref="Pair"/> relocation contains a signed 16-bit displacement
            /// that is added to the upper 16 bits that are taken from the location that is being relocated.
            /// </summary>
            SecRelHi = 0x000D ,

            /// <summary>
            /// The low 26 bits of the target’s VA.
            /// This supports  the MIPS16 JAL instruction.
            /// </summary>
            JmpAddr16 = 0x0010,

            /// <summary>
            ///  The target’s 32-bit RVA.
            /// </summary>
            RefWordNb = 0x0022,

            /// <summary>
            /// The relocation is valid only when it immediately  follows a <see cref="RefHi"/> or <see cref="SecRelHi"/> relocation.
            /// Its  SymbolTableIndex contains a displacement and  not an index into the symbol table
            /// </summary>
            Pair = 0x0025 
        }
    }
}