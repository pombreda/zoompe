using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Unmanaged
{
    partial struct RelocationType
    {
        public enum SuperH : short
        {
            /// <summary>
            /// The relocation is ignored.
            /// </summary>
            Absolute = 0x0000,

            /// <summary>
            /// A reference to the 16-bit location that contains the VA of the target symbol.
            /// </summary>
            Direct16 = 0x0001,

            /// <summary>
            /// The 32-bit VA of the target symbol.
            /// </summary>
            Direct32 = 0x0002,

            /// <summary>
            /// A reference to the 8-bit location that contains the VA of the target symbol.
            /// </summary>
            Direct8 = 0x0003,

            /// <summary>
            /// A reference to the 8-bit instruction that contains the effective 16-bit VA of the target symbol.
            /// </summary>
            Direct8_Word = 0x0004,

            /// <summary>
            /// A reference to the 8-bit instruction that contains the effective 32-bit VA of the target symbol.
            /// </summary>
            Direct8_Long = 0x0005,

            /// <summary>
            /// A reference to the 8-bit location whose low 4 bits contain the VA of the target symbol.
            /// </summary>
            Direct4 = 0x0006,

            /// <summary>
            /// A reference to the 8-bit instruction whose low 4 bits contain the effective 16-bit VA of the target symbol.
            /// </summary>
            Direct4_Word = 0x0007,

            /// <summary>
            /// A reference to the 8-bit instruction whose low 4 bits contain the effective 32-bit VA of the target symbol.
            /// </summary>
            Direct4_Long = 0x0008,

            /// <summary>
            /// A reference to the 8-bit instruction that contains the effective 16-bit relative offset of the target symbol.
            /// </summary>
            PCRel8_Word = 0x0009,

            /// <summary>
            /// A reference to the 8-bit instruction that contains the effective 32-bit relative offset of the target symbol.
            /// </summary>
            PCRel8_Long = 0x000A,

            /// <summary>
            /// A reference to the 16-bit instruction whose low 12 bits contain the effective 16-bit relative offset of the target symbol.
            /// </summary>
            PCRel12_Word = 0x000B,

            /// <summary>
            /// A reference to a 32-bit location that is the VA of the section that contains the target symbol.
            /// </summary>
            StartOf_Section = 0x000C,

            /// <summary>
            /// A reference to the 32-bit location that is the size of the section that contains the target symbol.
            /// </summary>
            SizeOf_Section = 0x000D,

            /// <summary>
            /// The 16-bit section index of the section that contains the target.
            /// This is used to support debugging information.
            /// </summary>
            Section = 0x000E,

            /// <summary>
            /// The 32-bit offset of the target from the beginning of its section.
            /// This is used to support debugging information and static thread local storage.
            /// </summary>
            SecRel = 0x000F,

            /// <summary>
            /// The 32-bit RVA of the target symbol.
            /// </summary>
            Direct32_NB = 0x0010,

            /// <summary>
            /// GP relative.
            /// </summary>
            GPRel4_Long = 0x0011,

            /// <summary>
            /// CLR token.
            /// </summary>
            Token = 0x0012,

            /// <summary>
            /// The offset from the current instruction in longwords. If the NOMODE bit is not set, insert the inverse of the low bit at bit 32 to select PTA or PTB.
            /// </summary>
            PCRelPT = 0x0013,

            /// <summary>
            /// The low 16 bits of the 32-bit address.
            /// </summary>
            RefLo = 0x0014,

            /// <summary>
            /// The high 16 bits of the 32-bit address
            /// </summary>
            RefHlf = 0x0015,

            /// <summary>
            /// The low 16 bits of the relative address.
            /// </summary>
            RelLo = 0x0016,

            /// <summary>
            /// The high 16 bits of the relative address.
            /// </summary>
            RelHalf = 0x0017,

            /// <summary>
            /// The relocation is valid only when it immediately follows a <see cref="RefHalf"/>, <see cref="RelHalf"/>, or <see cref="RelLo"/>.
            /// The SymbolTableIndex field of the relocation contains a displacement and not an index into the symbol table.
            /// </summary>
            Pair = 0x0018,

            /// <summary>
            /// The relocation ignores section mode.
            /// </summary>
            NoMode = 0x8000
        }
    }
}