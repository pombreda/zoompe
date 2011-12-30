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
    public abstract class MethodSig : Signature
    {
        public sealed class Default : MethodSig
        {

        }

        public sealed class C : MethodSig
        {
        }

        public sealed class StdCall : MethodSig
        {
        }

        public sealed class ThisCall : MethodSig
        {
        }

        public sealed class FastCall : MethodSig
        {
        }

        public sealed class Generic : MethodSig
        {
            public Signature[] GenParams;
        }

        public sealed class VarArg : MethodSig
        {
            public Signature[] VarArgs;
        }

        enum CallingConvention : byte
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

        public Signature RefType;
        public Signature[] ParamList;

        public static MethodSig Read(BinaryStreamReader reader)
        {
            var callingConvention = (CallingConvention)reader.ReadByte();

            
            switch (callingConvention & ~CallingConvention.HasThis & ~CallingConvention.ExplicitThis)
            {
                case CallingConvention.Default:
                    {
                        var result = new Default();
                        result.PopulateSimpleSig(reader, callingConvention);
                        return result;
                    }

                case CallingConvention.C:
                    {
                        var result = new C();
                        result.PopulateSimpleSig(reader, callingConvention);
                        return result;
                    }

                case CallingConvention.StdCall:
                    {
                        var result = new StdCall();
                        result.PopulateSimpleSig(reader, callingConvention);
                        return result;
                    }

                case CallingConvention.FastCall:
                    {
                        var result = new FastCall();
                        result.PopulateSimpleSig(reader, callingConvention);
                        return result;
                    }

                case CallingConvention.VarArg:
                    {
                        var result = new VarArg();
                        throw new NotImplementedException();
                        return result;
                    }

                case CallingConvention.Generic:
                    {
                        var result = new Generic();
                        throw new NotImplementedException();
                        return result;
                    }

                default:
                    throw new BadImageFormatException("Invalid calling convention byte "+callingConvention+".");
            }
        }

        void PopulateInstanceAndExplicit(CallingConvention callingConvention)
        {
            this.Instance = (callingConvention & CallingConvention.HasThis) != 0;
            this.Explicit = (callingConvention & CallingConvention.ExplicitThis) != 0;
        }

        void PopulateSimpleSig(BinaryStreamReader reader, CallingConvention callingConvention)
        {
            PopulateInstanceAndExplicit(callingConvention);
            throw new NotImplementedException();
        }
    }
}