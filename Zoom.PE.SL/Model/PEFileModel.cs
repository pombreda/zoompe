using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Mi.PE;
using Mi.PE.PEFormat;

namespace Zoom.PE.Model
{
    public sealed class PEFileModel : ReadOnlyObservableCollection<object>
    {
        readonly string m_FileName;
        readonly PEFile peFile;
        readonly DosHeaderModel m_DosHeader;
        DosStubModel m_DosStub;
        readonly PEHeaderModel m_PEHeader;
        readonly OptionalHeaderModel m_OptionalHeader;

        readonly ObservableCollection<object> partsCore = new ObservableCollection<object>();

        public PEFileModel(string fileName, PEFile peFile)
            : base(new ObservableCollection<object>())
        {
            this.m_FileName = fileName;
            this.peFile = peFile;

            this.Items.Add(this.DosHeader);
            this.m_DosHeader = new DosHeaderModel(peFile.DosHeader);

            UpdateDosStubFromlfanew();
            
            this.m_PEHeader = new PEHeaderModel(peFile.PEHeader, m_DosHeader);
            this.Items.Add(this.PEHeader);

            this.m_OptionalHeader = new OptionalHeaderModel(peFile.OptionalHeader, m_PEHeader);
            this.Items.Add(this.OptionalHeader);
            
            this.DosHeader.PropertyChanged += DosHeader_PropertyChanged;
        }

        public string FileName { get { return m_FileName; } }

        public DosHeaderModel DosHeader { get { return m_DosHeader; } }

        public DosStubModel DosStub
        {
            get { return m_DosStub; }
            private set
            {
                if (value == this.DosStub)
                    return;

                if (this.DosStub != null)
                    this.Items.RemoveAt(1);

                this.m_DosStub = value;

                if (this.DosStub != null)
                    this.Items.Insert(1, this.DosStub);

                OnPropertyChanged(new PropertyChangedEventArgs("DosStub"));
            }
        }

        public PEHeaderModel PEHeader { get { return m_PEHeader; } }

        public OptionalHeaderModel OptionalHeader { get { return m_OptionalHeader; } }

        void DosHeader_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "lfanew")
                UpdateDosStubFromlfanew();
        }

        private void UpdateDosStubFromlfanew()
        {
            long newDosStubSize = (long)this.peFile.DosHeader.lfanew - Mi.PE.PEFormat.DosHeader.Size;

            // adjust peFile.DosStub
            if (newDosStubSize > 0)
            {
                if (this.peFile.DosStub == null)
                {
                    this.peFile.DosStub = new byte[newDosStubSize];
                }
                else
                {
                    Array.Resize(ref this.peFile.DosStub, (int)newDosStubSize);
                }
            }
            else
            {
                if (this.peFile.DosStub != null)
                {
                    this.peFile.DosStub = null;
                }
            }

            // adjust this.DosStub
            if (this.peFile.DosStub == null)
            {
                this.DosStub = null;
            }
            else
            {
                if (this.DosStub == null)
                    this.DosStub = new DosStubModel { Data = this.peFile.DosStub };
                else
                    this.DosStub.Data = this.peFile.DosStub;
            }
        }
    }
}