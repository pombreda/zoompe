using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.Cli
{
    public sealed class Signature
    {
        readonly uint offset;
        readonly byte[] heap;

        public Signature(uint offset, byte[] heap)
        {
            this.offset = offset;
            this.heap = heap;
        }
    }
}