using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Mi.PE
{
    partial class PEFile
    {
        public sealed class PEFileWriter
        {
            public PEFileWriter()
            {
            }

            public void Write(PEFile pe, Stream stream)
            {
                throw new NotImplementedException();
            }
        }
    }
}