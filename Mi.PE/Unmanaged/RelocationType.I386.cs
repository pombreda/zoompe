using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Unmanaged
{
    partial struct RelocationType
    {
        public enum I386 : short
        {
            /// <summary>
            /// The relocation is ignored.
            /// </summary>
            ABSOLUTE = 0x0000,

            /// <summary>
            /// Not supported.
            /// </summary>
            Dir16 = 0x0001,

            /// <summary>
            /// Not supported.
            /// </summary>
            Rel16 = 0x0002,

            /// <summary>
            /// The target’s 32-bit VA.
            /// </summary>
            Dir32 = 0x0006,

            /// <summary>
            /// The target’s 32-bit RVA.
            /// </summary>
            Dir32NB = 0x0007,

            /// <summary>
            /// Not supported.
            /// </summary>
            Seg12 = 0x0009,

            /// <summary>
            /// The 16-bit section index of the section that contains the target.
            /// This is used to support debugging information.
            /// </summary>
            Section = 0x000A,

            /// <summary>
            /// The 32-bit offset of the target from the beginning of its section.
            /// This is used to support debugging information and static thread local storage.
            /// </summary>
            SecRel = 0x000B,

            /// <summary>
            /// The CLR token.
            /// </summary>
            Token = 0x000C,

            /// <summary>
            /// A 7-bit offset from the base of the section that contains the target.
            /// </summary>
            SecRel7 = 0x000D,

            /// <summary>
            /// The 32-bit relative displacement of the target.
            /// This supports the x86 relative branch and call instructions.
            /// </summary>
            Rel32 = 0x0014
        }
    }
}