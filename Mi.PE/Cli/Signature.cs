using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.Cli
{
    public sealed class Signature
    {
        readonly byte[] blob;

        public Signature(byte[] blob)
        {
            this.blob = blob;
        }
    }
}