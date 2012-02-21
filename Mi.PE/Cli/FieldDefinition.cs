using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mi.PE.Cli
{
    using Mi.PE.Cli.Tables;

    public sealed class FieldDefinition
    {
        public FieldAttributes Attributes;
        public string Name;

        public override string ToString()
        {
            return this.Name;
        }
    }
}