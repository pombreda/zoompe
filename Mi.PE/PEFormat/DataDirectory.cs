using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.PEFormat
{
    public struct DataDirectory
    {
        /// <summary> The relative virtual address of the table. </summary>
        public uint VirtualAddress { get; set; }

        /// <summary> The size of the table, in bytes. </summary>
        public uint Size { get; set; }

        #region ToString
        public override string ToString()
        {
            return this.VirtualAddress.ToString("X") + ":" + this.Size.ToString("X") + "h";
        }
        #endregion
    }
}