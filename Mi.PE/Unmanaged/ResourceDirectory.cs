using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.Unmanaged
{
    using Mi.PE.Internal;

    public sealed class ResourceDirectory
    {
        public sealed class DirectoryEntry
        {
            public string Name;
            public uint IntegerID;

            public DirectoryEntry[] Subdirectories;
            public DataEntry[] DataEntries;
        }

        public sealed class DataEntry
        {
            public string Name;
            public uint IntegerID;
            public uint DataRVA;
            public uint Size;
            public uint Codepage;
            public uint Reserved;
        }

        /// <summary>
        /// Resource flags. This field is reserved for future use. It is currently set to zero.
        /// </summary>
        public uint Characteristics;

        /// <summary>
        ///  The time that the resource data was created by the resource compiler
        /// </summary>
        public DateTime Timestamp;

        /// <summary>
        /// The major version number, set by the user.
        /// </summary>
        public ushort MajorVersion;

        /// <summary>
        /// The minor version number, set by the user.
        /// </summary>
        public ushort MinorVersion;

        /// <summary>
        /// The number of directory entries immediately following the table that use strings
        /// to identify Type, Name, or Language entries (depending on the level of the table).
        /// </summary>
        public ushort NumberOfNameEntries;

        /// <summary>
        /// The number of directory entries immediately following the Name entries
        /// that use numeric IDs for Type, Name, or Language entries.
        /// </summary>
        public ushort NumberOfIDEntries;

        public void ReadResourceDirectories(BinaryStreamReader reader)
        {
            this.Characteristics = reader.ReadUInt32();
            uint timestampNum = reader.ReadUInt32();
            this.Timestamp = PEFile.TimestampEpochUTC.AddSeconds(timestampNum);
            this.MajorVersion = reader.ReadUInt16();
            this.MinorVersion = reader.ReadUInt16();
            ushort nameEntryCount = reader.ReadUInt16();
            ushort idEntryCount = reader.ReadUInt16();

            var subdirectories = new List<DirectoryEntry>();
            var dataEntries = new List<DataEntry>();

            for (int i = 0; i < nameEntryCount; i++)
            {
                uint nameRva = reader.ReadUInt32();
                uint contentRva = reader.ReadUInt32();

                long savePosition = reader.Position;
                string name = ReadName(reader);
                reader.Position = savePosition;

                if ((contentRva & (1U << 31)) == 0)
                {
                    // TODO read data
                }
                else
                {
                    contentRva = contentRva | ~(1U << 31);
                    // TODO read directory
                }
            }
        }

        private string ReadName(BinaryStreamReader reader)
        {
            throw new NotImplementedException();
        }
    }
}