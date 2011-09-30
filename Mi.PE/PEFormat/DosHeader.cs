using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.PEFormat
{
    using Mi.PE.Internal;
    
    public sealed class DosHeader
    {
        public const int HeaderSize = 64;

        enum StubSizeImplication
        {
            UpdateStubFromLfanew,
            UpdateLfanewFromStub,
            Consistent
        }

        uint m_lfanew;
        byte[] m_Stub;

        internal DosHeader()
        {
        }

        /// <summary> Magic number. </summary>
        public MZSignature Signature { get; set; }

        /// <summary> Bytes on last page of file. </summary>
        public ushort cblp { get; set; }

        /// <summary> Pages in file. </summary>
        public ushort cp { get; set; }

        /// <summary> Relocations. </summary>
        public ushort crlc { get; set; }

        /// <summary> Size of header in paragraphs. </summary>
        public ushort cparhdr { get; set; }

        /// <summary> Minimum extra paragraphs needed. </summary>
        public ushort minalloc { get; set; }

        /// <summary> Maximum extra paragraphs needed. </summary>
        public ushort maxalloc { get; set; }

        /// <summary> Initial (relative) SS value. </summary>
        public ushort ss { get; set; }

        /// <summary> Initial SP value. </summary>
        public ushort sp { get; set; }

        /// <summary> Checksum. </summary>
        public ushort csum { get; set; }

        /// <summary>  Initial IP value. </summary>
        public ushort ip { get; set; }

        /// <summary> Initial (relative) CS value. </summary>
        public ushort cs { get; set; }

        /// <summary> File address of relocation table. </summary>
        public ushort lfarlc { get; set; }

        /// <summary> Overlay number. </summary>
        public ushort ovno { get; set; }

        public ulong res1 { get; set; }

        /// <summary> OEM identifier (for e_oeminfo). </summary>
        public ushort oemid { get; set; }

        /// <summary> OEM information; e_oemid specific. </summary>
        public ushort oeminfo { get; set; }

        public uint ReservedNumber0 { get; set; }
        public uint ReservedNumber1 { get; set; }
        public uint ReservedNumber2 { get; set; }
        public uint ReservedNumber3 { get; set; }
        public uint ReservedNumber4 { get; set; }

        /// <summary> File address of PE header. </summary>
        public uint lfanew
        {
            get { return m_lfanew; }
            set
            {
                if (value == this.lfanew)
                    return;

                this.m_lfanew = value;

                if (this.m_Stub != null
                    && this.m_Stub.Length != value)
                    this.m_Stub = null;
            }
        }

        public byte[] Stub
        {
            get
            {
                if (m_Stub == null
                    && (int)this.lfanew - DosHeader.HeaderSize >= 0)
                    m_Stub = new byte[(int)this.lfanew - DosHeader.HeaderSize];

                return this.m_Stub;
            }
        }

        #region ToString
        public override string ToString()
        {
            var result = new StringBuilder("[");
            if (this.Signature == MZSignature.MZ)
                result.Append("MZ");
            else
                result.Append("Signature:"+((ushort)this.Signature).ToString("X4")+"h");

            result.Append("].lfanew=");
            result.Append(this.lfanew.ToString("X"));
            result.Append('h');

            return result.ToString();
        }
        #endregion
    }
}