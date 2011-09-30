using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.Internal
{
    public sealed class SectionContentReader
    {
        readonly byte[] byteArray;
        readonly int m_VirtualBaseAddress;
        int m_VirtualPosition;

        public SectionContentReader(byte[] byteArray, int virtualBaseAddress)
        {
            this.byteArray = byteArray;
            this.m_VirtualBaseAddress = virtualBaseAddress;
        }

        public int VirtualBaseAddress { get { return this.m_VirtualBaseAddress; } }
        public int VirtualPosition { get { return this.m_VirtualPosition; } set { this.m_VirtualPosition = value; } }

        public byte ReadByte()
        {
            byte result = this.byteArray[this.VirtualPosition - this.VirtualBaseAddress];
            this.VirtualPosition++;
            return result;
        }

        public short ReadInt16()
        {
            short result = BitConverter.ToInt16(this.byteArray, this.VirtualPosition - this.VirtualBaseAddress);
            this.VirtualPosition+=2;
            return result;
        }

        public ushort ReadUInt16()
        {
            return unchecked((ushort)this.ReadInt16());
        }

        public int ReadInt32()
        {
            int result = BitConverter.ToInt32(this.byteArray, this.VirtualPosition - this.VirtualBaseAddress);
            this.VirtualPosition += 4;
            return result;
        }

        public uint ReadUInt32()
        {
            return unchecked((uint)this.ReadInt32());
        }

        public long ReadInt64()
        {
            long result = BitConverter.ToInt64(this.byteArray, this.VirtualPosition - this.VirtualBaseAddress);
            this.VirtualPosition += 8;
            return result;
        }

        public ulong ReadUInt64()
        {
            return unchecked((ulong)this.ReadInt64());
        }
    }
}