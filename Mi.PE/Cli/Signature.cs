using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.Cli
{
    public sealed class Signature
    {
        readonly int index;
        readonly byte[] blobHeap;

        public Signature(int index, byte[] blobHeap)
        {
            this.index = index;
            this.blobHeap = blobHeap;
        }
    }
}