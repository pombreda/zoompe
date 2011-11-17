using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.Cli
{
    using Mi.PE.Internal;
    using Mi.PE.PEFormat;
    
    public sealed class ClrHeader
    {
        public const uint Size = 72;

        public uint Cb { get; set; }

        public ushort MajorRuntimeVersion { get; set; }
        public ushort MinorRuntimeVersion { get; set; }

        public DataDirectory MetaData { get; set; }

        public ClrImageFlags Flags { get; set; }

        /// <summary>
        /// The main program if it is an EXE (not used if a DLL?)
        /// If COMIMAGE_FLAGS_NATIVE_ENTRYPOINT is not set, EntryPointToken represents a managed entrypoint.
        /// If COMIMAGE_FLAGS_NATIVE_ENTRYPOINT is set, EntryPointRVA represents an RVA to a native entrypoint
        /// (depricated for DLLs, use modules constructors intead).
        /// </summary>
        public uint EntryPointToken { get; set; }


        /// <summary>
        /// This is the blob of managed resources. Fetched using code:AssemblyNative.GetResource and
        /// code:PEFile.GetResource and accessible from managed code from
        /// System.Assembly.GetManifestResourceStream.  The meta data has a table that maps names to offsets into
        /// this blob, so logically the blob is a set of resources.
        /// </summary>
        public DataDirectory Resources { get; set; }

        /// <summary>
        /// IL assemblies can be signed with a public-private key to validate who created it.  The signature goes
        /// here if this feature is used.
        /// </summary>
        public DataDirectory StrongNameSignature { get; set; }

        /// <summary>
        /// Deprecated, not used.
        /// </summary>
        public DataDirectory CodeManagerTable { get; set; }

        /// <summary>
        /// Used for manged code that has unmaanaged code inside it (or exports methods as unmanaged entry points)
        /// </summary>
        public DataDirectory VTableFixups { get; set; }

        public DataDirectory ExportAddressTableJumps { get; set; }

        /// <summary>
        /// null for ordinary IL images.  NGEN images it points at a code:CORCOMPILE_HEADER structure
        /// </summary>
        public DataDirectory ManagedNativeHeader { get; set; }

        public void Read(BinaryStreamReader reader)
        {
            this.Cb = reader.ReadUInt32();

            if (this.Cb < ClrHeader.Size)
            {
                throw new BadImageFormatException(
                    "Unexpectedly short " + typeof(ClrHeader).Name + " structure " +
                    this.Cb + " reported by the Cb field " +
                    "(expected at least " + ClrHeader.Size + ").");
            }

            this.MajorRuntimeVersion = reader.ReadUInt16();
            this.MinorRuntimeVersion = reader.ReadUInt16();

            this.MetaData.Read(reader);

            this.Flags = (ClrImageFlags)reader.ReadInt32();

            this.EntryPointToken = reader.ReadUInt32();

            this.Resources.Read(reader);
            this.StrongNameSignature.Read(reader);
            this.CodeManagerTable.Read(reader);
            this.VTableFixups.Read(reader);
            this.ExportAddressTableJumps.Read(reader);
            this.ManagedNativeHeader.Read(reader);
        }
    }
}