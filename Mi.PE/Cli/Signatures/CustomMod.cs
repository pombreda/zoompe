using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.Cli.Signatures
{
    using Mi.PE.Cli.CodedIndices;
    using Mi.PE.Internal;

    /// <summary>
    /// [ECMA-335 §23.2.7, 23.2.8]
    /// </summary>
    public sealed class CustomMod
    {
        public bool Required;
        public CodedIndex<TypeDefOrRef> Type;

        public static CustomMod Read(ElementType leadByte, BinaryStreamReader signatureBlobReader)
        {
            CustomMod result;
            if (leadByte == ElementType.CMod_Opt)
            {
                result = new CustomMod();
                result.Required = true;
            }
            else if (leadByte == ElementType.CMod_ReqD)
            {
                result = new CustomMod();
                result.Required = false;
            }
            else
            {
                return null;
            }

            // TypeDefOrRefOrSpecEncoded (ECMA-335 §23.2.8)
            uint? encodedOrNull = signatureBlobReader.ReadCompressedInteger();

            switch (encodedOrNull)
            {
                case 0: // TypeDef
                case 1: // TypeRef
                case 2: // TypeSpec
                    result.Type = (CodedIndices.CodedIndex<CodedIndices.TypeDefOrRef>)encodedOrNull.Value;
                    break;

                default:
                    throw new BadImageFormatException("Invalid TypeDefOrRefOrSpecEncoded value: " + (encodedOrNull == null ? "null" : encodedOrNull.ToString()) + ".");
            }

            return result;
        }
    }
}