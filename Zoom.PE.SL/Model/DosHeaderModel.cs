using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Mi.PE;
using Mi.PE.PEFormat;

namespace Zoom.PE.Model
{
    public class DosHeaderModel : PEFilePartModel
    {
        readonly DosHeader dosHeader;

        public DosHeaderModel(DosHeader dosHeader)
        {
            this.dosHeader = dosHeader;
            this.Address = 0;
            this.Length = DosHeader.Size;
        }

        public MZSignature Signature { get { return MZSignature.MZ; } }

        public uint lfanew
        {
            get { return this.dosHeader.lfanew; }
            set
            {
                if (value == this.lfanew)
                    return;

                this.dosHeader.lfanew = value;

                OnPropertyChanged("lfanew");
            }
        }
    }
}
