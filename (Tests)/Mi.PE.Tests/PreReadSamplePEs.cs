using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Mi.PE
{
    internal static class PreReadSamplePEs
    {
        public static class Console
        {
            public static readonly PEFile AnyCPU = reader.ReadMetadata(new MemoryStream(Properties.Resources.console_anycpu));
            public static readonly PEFile X86 = reader.ReadMetadata(new MemoryStream(Properties.Resources.console_x86));
            public static readonly PEFile X64 = reader.ReadMetadata(new MemoryStream(Properties.Resources.console_x64));
            public static readonly PEFile Itanium = reader.ReadMetadata(new MemoryStream(Properties.Resources.console_itanium));
        }

        static readonly PEFileReader reader = new PEFileReader();
    }
}