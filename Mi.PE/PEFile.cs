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
        public DosHeader DosHeader { get; set; }
        public PEHeader PEHeader { get; set; }
        public OptionalHeader OptionalHeader { get; set; }
        public Section[] Sections { get; set; }
    }
}