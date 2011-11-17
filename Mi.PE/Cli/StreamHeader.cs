using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.Cli
{
    using Mi.PE.Internal;

    public sealed class StreamHeader
    {
        public uint Offset { get; set; }
        public uint Size { get; set; }
        public string Name { get; set; }

        #region ToString
        public override string ToString()
        {
            return
                Name + " " + this.Offset.ToString("X") + ":" + this.Size.ToString("X") + "h";
        }
        #endregion

        public void Read(BinaryStreamReader reader)
        {
            this.Offset = reader.ReadUInt32();
            this.Size = reader.ReadUInt32();
            this.Name = ReadAlignedNameString(reader);
        }

        private static string ReadAlignedNameString(BinaryStreamReader reader)
        {
            var bytes = new List<byte>();
            while (true)
            {
                var b = reader.ReadByte();
                if (b == 0)
                    break;

                bytes.Add(b);
            }

            int skipCount = -1 + ((bytes.Count + 4) & ~3) - bytes.Count;

            reader.Position += skipCount;

            return Encoding.UTF8.GetString(bytes.ToArray(), 0, bytes.Count);
        }
    }
}