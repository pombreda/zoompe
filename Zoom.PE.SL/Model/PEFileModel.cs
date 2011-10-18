using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Mi.PE;
using Mi.PE.PEFormat;

namespace Zoom.PE.Model
{
    public sealed class PEFileModel : INotifyPropertyChanged
    {
        readonly string m_FileName;
        readonly PEFile peFile;
        readonly DosHeaderModel m_DosHeader;
        DosStubModel m_DosStub;
        readonly PEHeaderModel m_PEHeader;
        readonly OptionalHeaderModel m_OptionalHeader;

        public PEFileModel(string fileName, PEFile peFile)
        {
            this.m_FileName = fileName;
            
            this.peFile = peFile;

            this.m_DosHeader = new DosHeaderModel(peFile.DosHeader);

            UpdateDosStubFromPEFile();
            
            this.m_PEHeader = new PEHeaderModel(peFile.PEHeader, m_DosHeader);
            this.m_OptionalHeader = new OptionalHeaderModel(peFile.OptionalHeader, m_PEHeader);
            
            this.DosHeader.PropertyChanged += DosHeader_PropertyChanged;
        }

        public string FileName { get { return m_FileName; } }

        public DosHeaderModel DosHeader { get { return m_DosHeader; } }

        public DosStubModel DosStub { get { return m_DosStub; } }

        public PEHeaderModel PEHeader { get { return m_PEHeader; } }

        public OptionalHeaderModel OptionalHeader { get { return m_OptionalHeader; } }

        public event PropertyChangedEventHandler PropertyChanged;

        void DosHeader_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "lfanew")
                UpdateDosStubFromPEFile();
        }

        private void UpdateDosStubFromPEFile()
        {
            if (peFile.DosStub == null)
            {
                if (this.DosStub == null)
                    return;

                this.m_DosStub = null;
            }
            else
            {
                if (this.DosStub != null)
                {
                    if(this.DosStub.Data != this.peFile.DosStub)
                        this.DosStub.Data = this.peFile.DosStub;
                    return;
                }

                {
                    this.m_DosStub = new DosStubModel
                    {
                        Data = this.peFile.DosStub
                    };
                }
            }

            var propertyChangedHandler = this.PropertyChanged;
            if (propertyChangedHandler != null)
                propertyChangedHandler(this, new PropertyChangedEventArgs("DosStub"));
        }
    }
}