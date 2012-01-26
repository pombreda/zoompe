using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.Cli.Signatures
{
    using Mi.PE.Internal;

    /// <summary>
    /// A <see cref="MethodSig"/> is indexed by the Method.Signature column.
    /// It captures the signature of a method or global function.
    /// [ECMA §23.2.1, §23.2.2, §23.2.3]
    /// </summary>
    public abstract class MethodSig
    {
        public sealed class Default : MethodSig
        {
            protected override void ReadCore(BinaryStreamReader reader)
            {
            }
        }

        public sealed class C : MethodSig
        {
            protected override void ReadCore(BinaryStreamReader reader)
            {
            }
        }

        public sealed class StdCall : MethodSig
        {
            protected override void ReadCore(BinaryStreamReader reader)
            {
            }
        }

        public sealed class ThisCall : MethodSig
        {
            protected override void ReadCore(BinaryStreamReader reader)
            {
            }
        }

        public sealed class FastCall : MethodSig
        {
            protected override void ReadCore(BinaryStreamReader reader)
            {
            }
        }

        public sealed class Generic : MethodSig
        {
            public LocalVarSig[] GenParams;

            protected override void ReadCore(BinaryStreamReader reader)
            {
            }
        }

        public sealed class VarArg : MethodSig
        {
            public LocalVarSig[] VarArgs;

            protected override void ReadCore(BinaryStreamReader reader)
            {
            }
        }

        [Flags]
        private enum CallingConventions : byte
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

        public bool Instance;
        public bool Explicit;

        public TypeSpec RefType;
        public LocalVarSig[] ParamList;

        public static MethodSig Read(BinaryStreamReader signatureBlobReader)
        {
            var callingConvention = (CallingConventions)signatureBlobReader.ReadByte();

            MethodSig result;
            switch (callingConvention & ~CallingConventions.HasThis & ~CallingConventions.ExplicitThis)
            {
                case CallingConventions.Default:
                    result = new Default();
                    break;

                case CallingConventions.C:
                    result = new C();
                    break;

                case CallingConventions.StdCall:
                    result = new StdCall();
                    break;

                case CallingConventions.FastCall:
                    result = new FastCall();
                    break;

                case CallingConventions.VarArg:
                    result = new VarArg();
                    break;

                case CallingConventions.Generic:
                    result = new Generic();
                    break;

                default:
                    throw new BadImageFormatException("Invalid calling convention byte "+callingConvention+".");
            }

            result.ReadCore(signatureBlobReader);

            return result;
        }

        protected abstract void ReadCore(BinaryStreamReader signatureBlobReader);

        void PopulateInstanceAndExplicit(CallingConventions callingConvention)
        {
            this.Instance = (callingConvention & CallingConventions.HasThis) != 0;
            this.Explicit = (callingConvention & CallingConventions.ExplicitThis) != 0;
        }

        void PopulateSimpleSig(BinaryStreamReader signatureBlobReader, CallingConventions callingConvention)
        {
            PopulateInstanceAndExplicit(callingConvention);
            throw new NotImplementedException();
        }
    }
}