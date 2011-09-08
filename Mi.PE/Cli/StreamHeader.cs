using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}