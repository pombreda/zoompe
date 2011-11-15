using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Unmanaged
{
    partial struct RelocationType
    {
        public enum Itanium : short
        {
            /// <summary>
            /// The relocation is ignored.
            /// </summary>
            Absolute= 0x000,

            /// <summary>
            /// The instruction relocation can be followed by an <see cref="AddEnd"/> relocation
            /// whose value is added to the target address before it is inserted into the specified slot in the <see cref="IMM14"/> bundle.
            /// The relocation target must be absolute or the image must be fixed.
            /// </summary>
            IMM14= 0x001,

            /// <summary>
            /// The instruction relocation can be followed by an <see cref="AddEnd"/> relocation
            /// whose value is added to the target address before it is inserted into the specified slot in the <see cref="IMM22"/> bundle.
            /// The relocation target must be absolute or the image must be fixed.
            /// </summary>
            IMM22= 0x002,

            /// <summary>
            /// The slot number of this relocation must be one (1).
            /// The relocation can be followed by an <see cref="AddEnd"/> relocation
            /// whose value is added to the target address before it is stored in all three slots of the <see cref="IMM64"/> bundle.
            /// </summary>
            IMM64= 0x003,

            /// <summary>
            /// The target’s 32-bit VA. This is supported only for /LARGEADDRESSAWARE:NO images.
            /// </summary>
            Dir32 = 0x004,

            /// <summary>
            /// The target’s 64-bit VA.
            /// </summary>
            Dir64 = 0x005,

            /// <summary>
            /// The instruction is fixed up with the 25-bit relative displacement of the 16-bit aligned target.
            /// The low 4 bits of the displacement are zero and are not stored.
            /// </summary>
            PCRel21B = 0x006,

            /// <summary>
            /// The instruction is fixed up with the 25-bit relative displacement of the 16-bit aligned target.
            /// The low 4 bits of the displacement, which are zero, are not stored.
            /// </summary>
            PCRel21M = 0x007,

            /// <summary>
            /// The LSBs of this relocation’s offset must contain the slot number whereas the rest is the bundle address.
            /// The bundle is fixed up with the 25-bit relative displacement of the 16-bit aligned target.
            /// The low 4 bits of the displacement are zero and are not stored
            /// </summary>
            PCRel21F= 0x008,
            
            /// <summary>
            /// The instruction relocation can be followed by an <see cref="AddEnd"/> relocation
            /// whose value is added to the target address and then a 22-bit GPrelative offset
            /// that is calculated and applied to the <see cref="GPRel22"/> bundle.
            /// </summary>
            GPRel22= 0x009,

            /// <summary>
            /// The instruction is fixed up with the 22-bit GPrelative offset to the target symbol’s literal table entry.
            /// The linker creates this literal table entry based on this relocation and the <see cref="AddEnd"/> relocation that might follow.
            /// </summary>
            LTOff22= 0x00A,
            
            /// <summary>
            /// The 16-bit section index of the section contains the target.
            /// This is used to support debugging information.
            /// </summary>
            Section= 0x00B,
            
            /// <summary>
            /// The instruction is fixed up with the 22-bit offset of the target from the beginning of its section.
            /// This relocation can be followed immediately by an <see cref="AddEnd"/> relocation,
            /// whose Value field contains the 32-bit unsigned offset of the target from the beginning of the section.
            /// </summary>
            SecRel22= 0x00C,

            /// <summary>
            /// The slot number for this relocation must be one (1).
            /// The instruction is fixed up with the 64-bit offset of the target from the beginning of its section.
            /// This relocation can be followed immediately by an <see cref="AddEnd"/> relocation,
            /// whose Value field contains the 32-bit unsigned offset of the target from the beginning of the section.
            /// </summary>
            SecRel64I= 0x00D,

            /// <summary>
            /// The address of data to be fixed up with the 32-bit offset of the target from the beginning of its section.
            /// </summary>
            SecRel32= 0x00E,

            /// <summary>
            /// The target’s 32-bit RVA.
            /// </summary>
            Dir32NB= 0x010,

            /// <summary>
            /// This is applied to a signed 14-bit immediate that contains the difference between two relocatable targets.
            /// This is a declarative field for the linker that indicates that the compiler has already emitted this value.
            /// </summary>
            SRel14= 0x011,

            /// <summary>
            /// This is applied to a signed 22-bit immediate that contains the difference between two relocatable targets.
            /// This is a declarative field for the linker that indicates that the compiler has already emitted this value.
            /// </summary>
            SRel22= 0x012,

            /// <summary>
            /// This is applied to a signed 32-bit immediate that contains the difference between two relocatable values.
            /// This is a declarative field for the linker that indicates that the compiler has already emitted this value.
            /// </summary>
            SRel32= 0x013,

            /// <summary>
            /// This is applied to an unsigned 32-bit immediate that contains the difference between two relocatable values.
            /// This is a declarative field for the linker that indicates that the compiler has already emitted this value.
            /// </summary>
            URel32= 0x014,
            
            /// <summary>
            /// A 60-bit PC-relative fixup that always stays as a BRL instruction of an MLX bundle.
            /// </summary>
            PCRel60X= 0x015,
            
            /// <summary>
            /// A 60-bit PC-relative fixup.
            /// If the target displacement fits in a signed 25-bit field, convert the entire bundle to an MBB bundle
            /// with NOP.B in slot 1 and a 25-bit BR instruction
            /// (with the 4 lowest bits all zero and dropped) in slot 2.
            /// </summary>
            PCRel60B= 0x016,
            
            /// <summary>
            /// A 60-bit PC-relative fixup.
            /// If the target displacement fits in a signed 25-bit field, convert the entire bundle to an MFB bundle
            /// with NOP.F in slot 1 and a 25-bit
            /// (4 lowest bits all zero and dropped) BR instruction in slot 2.
            /// </summary>
            PCRel60F= 0x017,

            /// <summary>
            /// A 60-bit PC-relative fixup.
            /// If the target displacement fits in a signed 25-bit field, convert the entire bundle to an MIB bundle
            /// with NOP.I in slot 1 and a 25-bit
            /// (4 lowest bits all zero and dropped) BR instruction in slot 2.
            /// </summary>
            PCRel60I= 0x018,
            
            /// <summary>
            /// A 60-bit PC-relative fixup.
            /// If the target displacement fits in a signed 25-bit field, convert the entire bundle to an MMB bundle
            /// with NOP.M in slot 1 and a 25-bit
            /// (4 lowest bits all zero and dropped) BR instruction in slot 2.
            /// </summary>
            PCRel60M= 0x019,
            
            /// <summary>
            /// A 64-bit GP-relative fixup.
            /// </summary>
            IMMGPRel64= 0x01a,

            /// <summary>
            /// A CLR token.
            /// </summary>
            TOKEN= 0x01b,

            /// <summary>
            /// A 32-bit GP-relative fixup.
            /// </summary>
            GPRel32= 0x01c,

            /// <summary>
            /// The relocation is valid only when it immediately follows one of the following relocations:
            /// <see cref="IMM14"/>, <see cref="IMM22"/>, <see cref="IMM64"/>, <see cref="GPRel22"/>, <see cref="LTOff22"/>, <see cref="LTOff64"/>,
            /// <see cref="SecRel22"/>, <see cref="SecRel64I"/>, or <see cref="SecRel32"/>.
            /// Its value contains the <see cref="AddEnd"/> to apply to instructions within a bundle, not for data.
            /// </summary>
            AddEnd= 0x01F
}
    }
}