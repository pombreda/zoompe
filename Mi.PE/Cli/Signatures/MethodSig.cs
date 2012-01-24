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
            public Signature[] GenParams;

            protected override void ReadCore(BinaryStreamReader reader)
            {
            }
        }

        public sealed class VarArg : MethodSig
        {
            public Signature[] VarArgs;

            protected override void ReadCore(BinaryStreamReader reader)
            {
            }
        }

        public bool Instance;
        public bool Explicit;

        public Signature RefType;
        public Signature[] ParamList;

        public static MethodSig Read(BinaryStreamReader reader)
        {
            var callingConvention = (LeadingByte)reader.ReadByte();

            
            switch (callingConvention & ~LeadingByte.HasThis & ~LeadingByte.ExplicitThis)
            {
                case LeadingByte.Default:
                    {
                        var result = new Default();
                        result.PopulateSimpleSig(reader, callingConvention);
                        return result;
                    }

                case LeadingByte.C:
                    {
                        var result = new C();
                        result.PopulateSimpleSig(reader, callingConvention);
                        return result;
                    }

                case LeadingByte.StdCall:
                    {
                        var result = new StdCall();
                        result.PopulateSimpleSig(reader, callingConvention);
                        return result;
                    }

                case LeadingByte.FastCall:
                    {
                        var result = new FastCall();
                        result.PopulateSimpleSig(reader, callingConvention);
                        return result;
                    }

                case LeadingByte.VarArg:
                    {
                        var result = new VarArg();
                        throw new NotImplementedException();
                        return result;
                    }

                case LeadingByte.Generic:
                    {
                        var result = new Generic();
                        throw new NotImplementedException();
                        return result;
                    }

                default:
                    throw new BadImageFormatException("Invalid calling convention byte "+callingConvention+".");
            }
        }

        void PopulateInstanceAndExplicit(LeadingByte callingConvention)
        {
            this.Instance = (callingConvention & LeadingByte.HasThis) != 0;
            this.Explicit = (callingConvention & LeadingByte.ExplicitThis) != 0;
        }

        void PopulateSimpleSig(BinaryStreamReader reader, LeadingByte callingConvention)
        {
            PopulateInstanceAndExplicit(callingConvention);
            throw new NotImplementedException();
        }
    }
}