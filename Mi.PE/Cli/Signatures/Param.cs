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
            List<CustomMod> customMods = null;
            var leadByte = (ElementType)signatureBlobReader.ReadByte();
            while (true)
            {
                var cmod = CustomMod.Read(leadByte, signatureBlobReader);
                if (cmod == null)
                    break;

                if (customMods == null)
                    customMods = new List<CustomMod>();

                customMods.Add(cmod);
            }

            if (customMods != null)
                this.CustomMods = customMods.ToArray();

            switch (leadByte)
	        {
                case ElementType.ByRef:
                    // TODO: read type
                    throw new NotImplementedException("TODO: read type");
                    break;

                case ElementType.TypedByRef:
                    // TODO: read type by ref???
                    throw new NotImplementedException("TODO: read type by ref???");
                    break;

		        default:
                    throw new BadImageFormatException(
                        "Invalid lead byte (" +
                        (this.CustomMods == null ? "no" : this.CustomMods.Length.ToString()) +
                        " custom modifiers): " + leadByte + ".");
	        }
        }
    }
}