﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.PEFormat
{
    using Mi.PE.Internal;
    
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

        #region ToString
        public override string ToString()
        {
            var result = new StringBuilder("[");
            if (this.Signature == MZSignature.MZ)
                result.Append("MZ");
            else
                result.Append("Signature:"+Format.ToString((ushort)this.Signature));

            result.Append("].lfanew=");
            result.Append(this.lfanew.ToString("X"));
            result.Append('h');

            return result.ToString();
        }
        #endregion
    }
}