using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Unmanaged
{
    public enum ArmRelocationType : short
    {
        /// <summary>
        /// The relocation is ignored.
        /// </summary>
        Absolute = 0x0000,

        /// <summary>
        /// The 32-bit VA of the target.
        /// </summary>
        Addr32 = 0x0001,

        /// <summary>
        /// The 32-bit RVA of the target.
        /// </summary>
        Addr32NB = 0x0002,

        /// <summary>
        /// The most significant 24 bits of the signed 26-bit relative displacement of the target.
        /// Applied to a B or BL instruction in ARM mode.
        /// </summary>
        Branch24 = 0x0003,

        /// <summary>
        /// The most significant 22 bits of the signed 23-bit relative displacement of the target.
        /// Applied to a contiguous 16-bit B+BL pair in Thumb mode prior to ARMv7.
        /// </summary>
        Branch11 = 0x0004,

        /// <summary>
        /// CLR tokens.
        /// </summary>
        Token = 0x0005,

        /// <summary>
        /// The most significant 24 or 25 bits of the signed 26-bit relative displacement of the target.
        /// Applied to an unconditional BL instruction in ARM mode.
        /// The BL is transformed to a BLX during relocation if the target is in Thumb mode.
        /// </summary>
        Blx24 = 0x0008,

        /// <summary>
        /// The most significant 21 or 22 bits of the signed 23-bit relative displacement of the target.
        /// Applied to a contiguous 16-bit B+BL pair in Thumb mode prior to ARMv7.
        /// The BL is transformed to a BLX during relocation if the target is in ARM mode.
        /// </summary>
        BLX11 = 0x0009,

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
        /// The 32-bit VA of the target.
        /// Applied to a contiguous MOVW+MOVT pair in ARM mode.
        /// The 32-bit VA is added to the existing value that is encoded in the immediate fields of the pair.
        /// </summary>
        Mov32A = 0x0010,

        /// <summary>
        /// The 32-bit VA of the target.
        /// Applied to a contiguous MOVW+MOVT pair in Thumb mode.
        /// The 32-bit VA is added to the existing value that is encoded in the immediate fields of the pair.
        /// </summary>
        Mov32T = 0x0011,

        /// <summary>
        /// The most significant 20 bits of the signed 21-bit relative displacement of the target.
        /// Applied to a 32-bit conditional B instruction in Thumb mode.
        /// </summary>
        Branch20T = 0x0012,

        /// <summary>
        /// The most significant 24 bits of the signed 25-bit relative displacement of the target.
        /// Applied to a 32-bit unconditional B or BL instruction in Thumb mode.
        /// </summary>
        Branch24T = 0x0014,

        /// <summary>
        /// The most significant 23 or 24 bits of the signed 25-bit relative displacement of the target.
        /// Applied to a 32-bit BL instruction in Thumb mode.
        /// The BL is transformed to a BLX during relocation if the target is in ARM mode.
        /// </summary>
        Blx23T = 0x0015
    }
}