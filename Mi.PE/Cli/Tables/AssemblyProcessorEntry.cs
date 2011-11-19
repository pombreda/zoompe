using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Cli.Tables
{
    /// <summary>
    /// This record should not be emitted into any PE file.
    /// However, if present in a PE file, it should be treated as if its field were zero.
    /// It should be ignored by the CLI.
    /// </summary>
    public sealed class AssemblyProcessorEntry
    {
        public uint Processor;

        public void Read(ClrModuleReader reader)
        {
            this.Processor = reader.Binary.ReadUInt32();
        }
    }
}