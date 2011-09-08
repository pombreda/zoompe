using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.Cli
{
    public sealed class TableStream
    {
        public int Reserved0 { get; set; }
        public byte MajorVersion { get; set; }
        public byte MinorVersion { get; set; }
        public byte HeapSizes { get; set; }
        public byte Reserved1 { get; set; }
        public ulong Valid { get; set; }
        public ulong Sorted { get; set; }
        public uint[] Rows { get; set; }
    }
}