using System;
using System.Collections.Generic;
using System.Linq;

namespace Mi.PE.Import
{
    public class ImportDescriptor
    {
        /// <summary>
        /// RVA to original unbound IAT (PIMAGE_THUNK_DATA).
        /// 0 for terminating null import descriptor.
        /// </summary>
        public uint OriginalFirstThunk { get; set; }

        /// <summary>
        /// 0 if not bound,
        /// -1 if bound, and real date\time stamp
        ///     in IMAGE_DIRECTORY_ENTRY_BOUND_IMPORT (new BIND)
        /// O.W. date/time stamp of DLL bound to (Old BIND)
        /// </summary>
        public uint TimeDateStamp { get; set; }

        /// <summary>
        /// -1 if no forwarders
        /// </summary>
        public uint ForwarderChain { get; set; }

        public uint Name { get; set; }

        /// <summary>
        /// RVA to IAT (if bound this IAT has actual addresses)
        /// </summary>
        public uint FirstThunk { get; set; }
    }
}