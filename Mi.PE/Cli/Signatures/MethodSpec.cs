using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.Cli.Signatures
{
    using Mi.PE.Internal;

    public sealed class MethodSpec
    {
        public TypeSpec[] GenArgs;

        public void Read(BinaryStreamReader signatureBlobReader)
        {
            byte genericInst = signatureBlobReader.ReadByte();
            if (genericInst != 0x0a)
                throw new BadImageFormatException("Invalid leading byte in MethodSpec: " + genericInst + ".");

            uint? genArgCount = signatureBlobReader.ReadCompressedInteger();
            if(genArgCount==null)
                throw new BadImageFormatException("Null value for MethodSpec.GenArgCount is not supported.");

            this.GenArgs = new TypeSpec[genArgCount.Value];
        }
    }
}