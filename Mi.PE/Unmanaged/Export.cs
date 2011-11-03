using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.Unmanaged
{
    public sealed class Export
    {
        public sealed class Header
        {
            public uint Flags;

        }

        public string FunctionName;
        public uint FunctionOrdinal;
        public uint Pointer;
        public string Forwarder;
    }
}