using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.Cli
{
    public sealed class ClrType : TypeReference
    {
        public string Name;
        public string Namespace;
        public TypeReference BaseType;
    }
}