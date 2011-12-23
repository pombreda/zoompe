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
        public Mi.PE.Cli.Tables.TypeAttributes Attributes;
        public TypeReference BaseType;
        public ClrField[] Fields;

        public override string ToString()
        {
            return
                string.IsNullOrEmpty(this.Namespace) ? this.Name :
                this.Namespace + "." + this.Name;
        }
    }
}