using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.Cli.Signatures
{
    using Mi.PE.Internal;

    public abstract class Signature
    {
        [Flags]
        internal enum LeadingByte : byte
        {
            /// <summary>
            /// Used to encode the keyword default in the calling convention, see ECMA §15.3.
            /// </summary>
            Default = 0x0,

            C = 0x1,

            StdCall = 0x2,

            FastCall = 0x4,

            /// <summary>
            /// Used to encode the keyword vararg in the calling convention, see ECMA §15.3.
            /// </summary>
            VarArg = 0x5,

            Field = 0x6,

            /// <summary>
            /// Used to indicate that the method has one or more generic parameters.
            /// </summary>
            Generic = 0x10,

            /// <summary>
            /// Used to encode the keyword instance in the calling convention, see ECMA §15.3.
            /// </summary>
            HasThis = 0x20,

            /// <summary>
            /// Used to encode the keyword explicit in the calling convention, see ECMA §15.3.
            /// </summary>
            ExplicitThis = 0x40,

            /// <summary>
            /// (ECMA §23.1.16), used to encode '...' in the parameter list, see ECMA §15.3.
            /// </summary>
            Sentinel = 0x41,
        }

        public static Signature Read(BinaryStreamReader reader)
        {
            LeadingByte sigByte = (LeadingByte)reader.ReadByte();
            Signature result;

            if ((sigByte & LeadingByte.Field) == LeadingByte.Field)
            {
                result = new FieldSig();
            }
            else if ((sigByte & LeadingByte.C) == LeadingByte.C)
            {
                    result = new MethodSig.C();
            }
            else if ((sigByte & LeadingByte.StdCall) == LeadingByte.StdCall)
            {
                result = new MethodSig.StdCall();
            }
            else if ((sigByte & LeadingByte.FastCall) == LeadingByte.FastCall)
            {
                result = new MethodSig.FastCall();
            }
            else if ((sigByte & LeadingByte.VarArg) == LeadingByte.VarArg)
            {
                result = new MethodSig.VarArg();
            }
            else if ((sigByte & LeadingByte.Generic) == LeadingByte.Generic)
            {
                result = new MethodSig.Generic();
            }
            else
            {
                throw new BadImageFormatException("Invalid signature leading byte " + sigByte.ToString() + ".");
            }
            
            result.ReadCore(reader);

            return result;
        }

        protected abstract void ReadCore(BinaryStreamReader reader);
    }
}