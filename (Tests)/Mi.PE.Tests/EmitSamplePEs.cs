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
            public static readonly PEFile AnyCPU = reader.ReadMetadata(new MemoryStream(EmitAssembly(PortableExecutableKinds.ILOnly, ImageFileMachine.I386)));
            public static readonly PEFile X86 = reader.ReadMetadata(new MemoryStream(EmitAssembly(PortableExecutableKinds.Required32Bit, ImageFileMachine.I386)));
            public static readonly PEFile X64 = reader.ReadMetadata(new MemoryStream(EmitAssembly(PortableExecutableKinds.PE32Plus, ImageFileMachine.AMD64)));
            public static readonly PEFile Itanium = reader.ReadMetadata(new MemoryStream(EmitAssembly(PortableExecutableKinds.PE32Plus, ImageFileMachine.IA64)));
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