using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Mi.PE;
using Mi.PE.PEFormat;

namespace Zoom.PE.Model
{
    public sealed class RawSectionContentModel : SectionContentModel
    {
        public RawSectionContentModel(SectionHeader sectionHeader)
            : base(sectionHeader)
        {
        }

        public ulong VirtualAddress { get { return SectionHeader.VirtualAddress; } }

        public byte[] Data
        {
            get { return new byte[20]; }
        }
    }
}