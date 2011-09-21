using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Mi.PE
{
    using Mi.PE.PEFormat;

    public sealed partial class PEFile
    {
        readonly DosHeader m_DosHeader = new DosHeader();
        readonly PEHeader m_PEHeader;
        readonly OptionalHeader m_OptionalHeader = new OptionalHeader();
        internal ReadOnlyCollection<Section> m_Sections;

        public PEFile()
        {
            this.m_PEHeader = new PEHeader(this);
        }

        public DosHeader DosHeader { get { return m_DosHeader; } }
        public PEHeader PEHeader { get { return m_PEHeader; } }
        public OptionalHeader OptionalHeader { get { return m_OptionalHeader; } }
        
        public ReadOnlyCollection<Section> Sections
        {
            get
            {
                if (m_Sections == null)
                {
                    var sections = new Section[this.PEHeader.NumberOfSections];
                    for (int i = 0; i < sections.Length; i++)
                    {
                        sections[i] = new Section();
                    }
                    m_Sections = new ReadOnlyCollection<Section>(sections);
                }

                return m_Sections;
            }
        }
    }
}