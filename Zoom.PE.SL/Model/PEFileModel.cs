using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Mi.PE;
using Mi.PE.PEFormat;

namespace Zoom.PE.Model
{
    public sealed class PEFileModel
    {
        readonly string m_FileName;
        readonly PEFile peFile;
        readonly DosHeaderModel m_DosHeader;

        public PEFileModel(string fileName, PEFile peFile)
        {
            this.m_FileName = fileName;
            this.peFile = peFile;
            this.m_DosHeader = new DosHeaderModel(peFile.DosHeader);
            this.DosHeader.PropertyChanged += DosHeader_PropertyChanged;
        }

        public string FileName { get { return m_FileName; } }
        public DosHeaderModel DosHeader { get { return m_DosHeader; } }

        public event PropertyChangedEventHandler PropertyChanged;

        void DosHeader_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "lfanew")
                return;


        }
    }
}