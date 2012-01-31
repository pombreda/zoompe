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

            result.Type = signatureBlobReader.ReadTypeDefOrRefOrSpecEncoded();

            return result;
        }
    }
}