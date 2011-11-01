using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Mi.PE;
using Mi.PE.PEFormat;

namespace Zoom.PE.Model
{
    public abstract class SectionContentModel :PEFilePart
    {
        readonly SectionHeader m_SectionHeader;

        protected SectionContentModel(SectionHeader sectionHeader)
            : base(sectionHeader.Name)
        {
            this.m_SectionHeader = sectionHeader;
            this.Address = sectionHeader.PointerToRawData;
            this.Length = sectionHeader.SizeOfRawData;
        }

        protected SectionHeader SectionHeader { get { return m_SectionHeader; } }
    }
}