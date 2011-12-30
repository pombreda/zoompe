using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.Cli.Signatures
{
    using Mi.PE.Internal;

    public abstract class Signature
    {
        public static Signature Read(BinaryStreamReader reader)
        {
            throw new NotImplementedException();
        }
    }
}