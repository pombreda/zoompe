using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.PEFormat
{
    public sealed class DosHeader
    {
        public const int Size = 64;

        public MZSignature Signature { get; set; }
        public ushort cblp { get; set; }
        public ushort cp { get; set; }
        public ushort crlc { get; set; }
        public ushort cparhdr { get; set; }
        public ushort minalloc { get; set; }
        public ushort maxalloc { get; set; }
        public ushort ss { get; set; }
        public ushort sp { get; set; }
        public ushort csum { get; set; }
        public ushort ip { get; set; }
        public ushort cs { get; set; }
        public ushort lfarlc { get; set; }
        public ushort ovno { get; set; }
        public ulong res1 { get; set; }
        public ushort oemid { get; set; }
        public ushort oeminfo { get; set; }

        public uint ReservedNumber0 { get; set; }
        public uint ReservedNumber1 { get; set; }
        public uint ReservedNumber2 { get; set; }
        public uint ReservedNumber3 { get; set; }
        public uint ReservedNumber4 { get; set; }

        public uint lfanew { get; set; }

        internal static readonly byte[] DefaultStub = new byte[]
        {
            0x0E, 0x1F, 0xBA, 0x0E, 0x00, 0xB4, 0x09, 0xCD, 0x21, 0xB8, 0x01, 0x4C, 0xCD, 0x21,
            (byte)'T', (byte)'h', (byte)'i', (byte)'s', (byte)' ',
            (byte)'p', (byte)'r', (byte)'o', (byte)'g', (byte)'r', (byte)'a', (byte)'m', (byte)' ',
            (byte)'c', (byte)'a', (byte)'n', (byte)'n', (byte)'o', (byte)'t', (byte)' ',
            (byte)'b', (byte)'e', (byte)' ',
            (byte)'r', (byte)'u', (byte)'n', (byte)' ',
            (byte)'i', (byte)'n', (byte)' ',
            (byte)'D', (byte)'O', (byte)'S', (byte)'o',
            (byte)'m', (byte)'o', (byte)'d', (byte)'e', (byte)'.',
            (byte)'\r', (byte)'\r', (byte)'\n', (byte)'$',
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        #region ToString
        public override string ToString()
        {
            var result = new StringBuilder("[");
            if(this.Signature!=MZSignature.MZ)
                result.Append("Signature:"+((int)this.Signature).ToString("X"));

            result.Append("].lfanew=");
            result.Append(this.lfanew.ToString("X"));
            result.Append('h');

            return result.ToString();
        }
        #endregion
    }
}