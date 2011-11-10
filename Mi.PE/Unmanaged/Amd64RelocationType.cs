using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Unmanaged
{
    public enum Amd64RelocationType : short
    {
        /// <summary>
        ///  The relocation is ignored.
        /// </summary>
        Absolute = 0,

        /// <summary>
        /// The 64-bit VA of the relocation target.
        /// </summary>
        Addr64 = 0x0001,

        /// <summary>
        /// The 32-bit VA of the relocation target.
        /// </summary>
        Addr32 = 0x0002,

        /// <summary>
        /// The 32-bit address without an image base (RVA).
        /// </summary>
        Addr32NB = 0x0003,

        /// <summary>
        /// The 32-bit relative address from the byte following the relocation.
        /// </summary>
        Rel32 = 0x0004,

        /// <summary>
        /// The 32-bit address relative to byte distance 1 from the relocation.
        /// </summary>
        Rel32_1 = 0x0005,

        /// <summary>
        /// The 32-bit address relative to byte distance 2 from the relocation.
        /// </summary>
        Rel32_2 = 0x0006,

        /// <summary>
        /// The 32-bit address relative to byte distance 3 from the relocation.
        /// </summary>
        Rel32_3 = 0x0007,

        /// <summary>
        /// The 32-bit address relative to byte distance 4 from the relocation.
        /// </summary>
        Rel32_4 = 0x0008,

        /// <summary>
        /// The 32-bit address relative to byte distance 5 from the relocation.
        /// </summary>
        Rel32_5 = 0x0009,

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
        /// A 7-bit unsigned offset from the base of the section that contains the target.
        /// </summary>
        SecRel7 = 0x000C,

        /// <summary>
        /// CLR tokens.
        /// </summary>
        Token = 0x000D,

        /// <summary>
        /// A 32-bit signed span-dependent value emitted into the object.
        /// </summary>
        SRel32 = 0x000E,

        /// <summary>
        /// A pair that must immediately follow every spandependent value.
        /// </summary>
        Pair = 0x000F,

        /// <summary>
        /// A 32-bit signed span-dependent value that is applied at link time.
        /// </summary>
        SSpan32 = 0x0010 
    }
}