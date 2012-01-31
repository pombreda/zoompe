using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.Cli.Signatures
{
    using Mi.PE.Internal;

    /// <summary>
    /// [ECMA-335 §23.2.11]
    /// </summary>
    public sealed class RefType
    {
        public void Read(BinaryStreamReader signatureBlobReader)
        {
            var etype = (ElementType)signatureBlobReader.ReadByte();
            if (etype == ElementType.ByRef)
            {
            }
            else if (etype == ElementType.TypedByRef)
            {
            }
            else if (etype == ElementType.Void)
            {
            }
            else
            {
                throw new BadImageFormatException("Invalid element type byte " + etype + ".");
            }
        }
    }
}