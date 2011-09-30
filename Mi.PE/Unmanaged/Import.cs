using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.Unmanaged
{
    using Mi.PE.Internal;

    public sealed class Import
    {
        public string FunctionName { get; set; }
        public uint FunctionOrdinal { get; set; }
        public string DllName { get; set; }

        public static Import[] ReadImports(SectionContentReader reader)
        {
            var resultList = new List<Import>();

            while(true)
            {
                uint originalFirstThunk = reader.ReadUInt32();
                uint timeDateStamp = reader.ReadUInt32();
                uint forwarderChain = reader.ReadUInt32();
                uint nameRva = reader.ReadUInt32();
                uint firstThunk = reader.ReadUInt32();

                string libraryName =
                    nameRva == 0 ? null : ReadAsciiZAt(reader, nameRva);

                uint thunkAddressPosition = originalFirstThunk == 0 ? firstThunk : originalFirstThunk;

                if (thunkAddressPosition == 0)
                    break;

                int savePosition = reader.VirtualPosition;
                try
                {
                    while (true)
                    {
                        reader.VirtualPosition = (int)thunkAddressPosition;

                        uint importPosition = reader.ReadUInt32();
                        if (importPosition == 0)
                            break;

                        Import imp;

                        if ((importPosition & (1 << 31)) != 0)
                        {
                            imp = new Import
                            {
                                DllName = libraryName,
                                FunctionOrdinal = importPosition
                            };
                        }
                        else
                        {
                            reader.VirtualPosition = (int)importPosition;

                            uint hint = reader.ReadUInt16();
                            string fname = ReadAsciiZ(reader);

                            imp = new Import
                            {
                                DllName = libraryName,
                                FunctionOrdinal = hint,
                                FunctionName = fname
                            };
                        }

                        resultList.Add(imp);

                        thunkAddressPosition += 8;
                    }
                }
                finally
                {
                    reader.VirtualPosition = savePosition;
                }
            }

            return resultList.ToArray();
        }

        private static string ReadAsciiZAt(SectionContentReader reader, uint nameRva)
        {
            int savePosition = reader.VirtualPosition;
            reader.VirtualPosition = (int)nameRva;
            try
            {
                return ReadAsciiZ(reader);
            }
            finally
            {
                reader.VirtualPosition = savePosition;
            }
        }

        private static string ReadAsciiZ(SectionContentReader reader)
        {
            var result = new StringBuilder();
            while (true)
            {
                byte b = reader.ReadByte();
                if (b == 0)
                    break;

                const int maxLength = 512;

                if (result.Length > maxLength)
                    throw new InvalidOperationException("String is too long, for safety reasons the size limit for ASCIIZ strings is set to " + maxLength + ".");

                result.Append((char)b);
            }

            return result.ToString();
        }

        public override string ToString()
        {
            return this.DllName + " " + (this.FunctionOrdinal != 0 ? "[" + this.FunctionOrdinal + "]" : "") + this.FunctionName;
        }
    }
}