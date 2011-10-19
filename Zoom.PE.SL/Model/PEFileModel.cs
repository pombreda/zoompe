﻿using System;
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

            UpdateDosStubFromlfanew();
            
            this.m_PEHeader = new PEHeaderModel(peFile.PEHeader, m_DosHeader);
            this.m_OptionalHeader = new OptionalHeaderModel(peFile.OptionalHeader, m_PEHeader);
            
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

                this.m_DosStub = value;

                var propertyChangedHandler = this.PropertyChanged;
                if (propertyChangedHandler != null)
                    propertyChangedHandler(this, new PropertyChangedEventArgs("DosStub"));
            }
        }

        public PEHeaderModel PEHeader { get { return m_PEHeader; } }

        public OptionalHeaderModel OptionalHeader { get { return m_OptionalHeader; } }

        public event PropertyChangedEventHandler PropertyChanged;

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