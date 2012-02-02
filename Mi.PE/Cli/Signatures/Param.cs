using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.Cli.Signatures
{
    using Mi.PE.Internal;

    public sealed class Param
    {
        public CustomMod[] CustomMods;

        public void Read(BinaryStreamReader signatureBlobReader)
        {
            ElementType leadByte;
            this.CustomMods = CustomMod.ReadCustomModArray(out leadByte, signatureBlobReader);

            switch (leadByte)
	        {
                case ElementType.ByRef:
                    // TODO: read type
                    throw new NotImplementedException("TODO: read type");

                case ElementType.TypedByRef:
                    // TODO: read type by ref???
                    throw new NotImplementedException("TODO: read type by ref???");

		        default:
                    throw new BadImageFormatException(
                        "Invalid lead byte (" +
                        (this.CustomMods == null ? "no" : this.CustomMods.Length.ToString()) +
                        " custom modifiers): " + leadByte + ".");
	        }
        }
    }
}