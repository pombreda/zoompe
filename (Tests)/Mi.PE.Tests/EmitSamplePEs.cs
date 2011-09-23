using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace Mi.PE
{
    internal static class EmitSamplePEs
    {
        public static class Library
        {
            public static class Bytes
            {
                public static readonly byte[] AnyCPU = EmitAssembly(PortableExecutableKinds.ILOnly, ImageFileMachine.I386);
                public static readonly byte[] X86 = EmitAssembly(PortableExecutableKinds.Required32Bit, ImageFileMachine.I386);
                public static readonly byte[] X64 = EmitAssembly(PortableExecutableKinds.PE32Plus, ImageFileMachine.AMD64);
                public static readonly byte[] Itanium = EmitAssembly(PortableExecutableKinds.PE32Plus, ImageFileMachine.IA64);
            }

            public static readonly PEFile AnyCPU = reader.Read(new MemoryStream(Bytes.AnyCPU));
            public static readonly PEFile X86 = reader.Read(new MemoryStream(Bytes.X86));
            public static readonly PEFile X64 = reader.Read(new MemoryStream(Bytes.X64));
            public static readonly PEFile Itanium = reader.Read(new MemoryStream(Bytes.Itanium));
        }

        static readonly PEFile.Reader reader = new PEFile.Reader();

        private static byte[] EmitAssembly(PortableExecutableKinds peKind, ImageFileMachine machine)
        {
            byte[] bytes;
            var asmName = new AssemblyName { Name = "Dummy" + Guid.NewGuid() };
            var asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(
                asmName,
                System.Reflection.Emit.AssemblyBuilderAccess.Save);
            asmBuilder.Save(asmName.Name, peKind, machine);
            try
            {
                bytes = File.ReadAllBytes(asmName.Name);
            }
            finally
            {
                File.Delete(asmName.Name);
            }
            return bytes;
        }
    }
}