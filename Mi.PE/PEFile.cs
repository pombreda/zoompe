using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Mi.PE
{
    using Mi.PE.PEFormat;

    public sealed class PEFile
    {
        readonly DosHeader m_DosHeader = new DosHeader();
        readonly PEHeader m_PEHeader = new PEHeader();
        readonly OptionalHeader m_OptionalHeader = new OptionalHeader();

        public PEFile()
        {
        }

        public DosHeader DosHeader { get { return m_DosHeader; } }
        public PEHeader PEHeader { get { return m_PEHeader; } }
        public OptionalHeader OptionalHeader { get { return m_OptionalHeader; } }
        public Section[] Sections { get; set; }
    }
}