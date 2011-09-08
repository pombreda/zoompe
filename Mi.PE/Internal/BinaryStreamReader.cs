using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Mi.PE.Internal
{
    public sealed class BinaryStreamReader
    {
        readonly Stream stream;
        readonly byte[] buffer;
        int bufferDataPosition;
        int bufferDataSize;

        public BinaryStreamReader(Stream stream, byte[] buffer)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (buffer.Length <= 8)
                throw new ArgumentException("Too short buffer, needs to fit at least sizeof(long).", "buffer");

            this.stream = stream;
            this.buffer = buffer;
        }

        public long Position
        { 
            get 
            {
                return this.stream.Position - this.bufferDataSize; 
            } 
            set
            {
                long offset = value - this.Position;
                if (offset == 0)
                {
                    return;
                }
                else if (offset > 0 && offset <this.bufferDataSize )
                {
                    this.bufferDataPosition += unchecked((int)offset);
                    this.bufferDataSize -= unchecked((int)offset);
                }
                else
                {
                    this.stream.Position = value;
                    this.bufferDataPosition = 0;
                    this.bufferDataSize = 0;
                }
            }
        }

        public byte ReadByte()
        {
            EnsurePopulatedData(1);
            byte result = buffer[bufferDataPosition];
            SkipUnchecked(1);
            return result;
        }

        public int ReadInt32()
        {
            EnsurePopulatedData(4);
            int result = BitConverter.ToInt32(buffer, bufferDataPosition);
            SkipUnchecked(4);
            return result;
        }

        public uint ReadUInt32()
        {
            return unchecked((uint)this.ReadInt32());
        }

        public short ReadInt16()
        {
            EnsurePopulatedData(2);
            short result = BitConverter.ToInt16(buffer, bufferDataPosition);
            SkipUnchecked(2);
            return result;
        }

        public ushort ReadUInt16()
        {
            return unchecked((ushort)this.ReadInt16());
        }

        public long ReadInt64()
        {
            EnsurePopulatedData(8);
            long result = BitConverter.ToInt64(buffer, bufferDataPosition);
            SkipUnchecked(8);
            return result;
        }

        public ulong ReadUInt64()
        {
            return unchecked((ulong)this.ReadInt64());
        }

        public string ReadFixedZeroFilledString(int size)
        {
            if (size <= 8 || size <= this.bufferDataSize)
            {
                EnsurePopulatedData(size);
                int actualSize = 0;
                for (int i = 0; i < size; i++)
                {
                    if (this.buffer[this.bufferDataPosition + i] != 0)
                        actualSize = i + 1;
                }

                if (actualSize == 0)
                    return string.Empty;

                string result = Encoding.UTF8.GetString(this.buffer, this.bufferDataPosition, actualSize);

                SkipUnchecked(size);

                return result;
            }
            else
            {
                int actualSize = 0;
                for (int i = 0; i < this.bufferDataSize; i++)
                {
                    if (this.buffer[this.bufferDataPosition + i] != 0)
                        actualSize = i + 1;
                }

                var byteBuffer = new MemoryStream();
                byteBuffer.Write(this.buffer, this.bufferDataPosition, this.bufferDataSize);
                this.bufferDataPosition = 0;
                this.bufferDataSize = 0;

                while (true)
                {
                    int readCount = this.stream.Read(this.buffer, 0, this.buffer.Length);

                    if (readCount <= 0)
                        throw new EndOfStreamException();

                    int processCount = Math.Min(readCount, size - unchecked((int)byteBuffer.Length));

                    for (int i = 0; i < processCount; i++)
                    {
                        if (this.buffer[i] != 0)
                            actualSize = unchecked((int)byteBuffer.Length) + i;
                    }

                    byteBuffer.Write(this.buffer, 0, processCount);

                    if (readCount >= processCount)
                    {
                        this.bufferDataPosition = readCount - processCount;
                        if (readCount > processCount)
                            this.bufferDataPosition = processCount;
                        else
                            this.bufferDataPosition = 0;
                        break;
                    }
                }

                string result = Encoding.UTF8.GetString(byteBuffer.ToArray(), 0, actualSize);

                return result;
            }
        }

        public void ReadBytes(byte[] bytes, int offset, int size)
        {
            if (size <= 8 || this.bufferDataSize <= size)
            {
                EnsurePopulatedData(size);
                Array.Copy(
                    this.buffer, this.bufferDataPosition,
                    bytes, offset,
                    size);
                SkipUnchecked(size);
            }
            else
            {

            }
        }

        private void SkipUnchecked(int size)
        {
            this.bufferDataSize-=size;
            if (this.bufferDataSize == 0)
                this.bufferDataPosition = 0;
            else
                this.bufferDataPosition += size;
        }

        private void EnsurePopulatedData(int size)
        {
            if (this.bufferDataSize < size)
            {
                if (this.buffer.Length - this.bufferDataPosition < size)
                {
                    Array.Copy(
                        this.buffer, this.bufferDataPosition,
                        this.buffer, 0,
                        this.bufferDataSize);

                    this.bufferDataPosition = 0;
                }

                while (this.bufferDataSize < size)
                {
                    int bufferDataEnd = this.bufferDataPosition + this.bufferDataSize;
                    int readCount = this.stream.Read(this.buffer, bufferDataEnd, this.buffer.Length - bufferDataEnd);

                    if (readCount <= 0)
                        throw new EndOfStreamException();

                    this.bufferDataSize += readCount;
                }
            }
        }
    }
}