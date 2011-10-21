using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Mi.PE;
using Mi.PE.PEFormat;

namespace Zoom.PE.Model
{
    public sealed class OptionalHeaderModel : PEFilePartModel
    {
        readonly OptionalHeader optionalHeader;
        OptionalHeaderData m_Data;
        ReadOnlyCollection<DataDirectoryModel> m_DataDirectories;

        public OptionalHeaderModel(OptionalHeader optionalHeader, PEHeaderModel peHeader)
        {
            this.optionalHeader = optionalHeader;

            BindAddressToPEHeader(peHeader);

            this.Address = peHeader.Address + peHeader.Length;

            UpdateLength();

            UpdateDataFromPEMagic();
        }

        public PEMagic PEMagic
        {
            get { return optionalHeader.PEMagic; }
            set
            {
                if (value == optionalHeader.PEMagic)
                    return;

                if (value != PEMagic.NT32
                    && value != PEMagic.NT64)
                    throw new ArgumentException("Invalid PEMagic value " + value + ".", "value");

                optionalHeader.PEMagic = value;
                OnPropertyChanged("PEMagic");

                UpdateDataFromPEMagic();
            }
        }

        public OptionalHeaderData Data
        {
            get { return m_Data; }
            private set
            {
                this.m_Data = value;
                OnPropertyChanged("Data");
            }
        }

        public ReadOnlyCollection<DataDirectoryModel> DataDirectories
        {
            get { return m_DataDirectories; }
            private set
            {
                this.m_DataDirectories = value;
                OnPropertyChanged("DataDirectories");
            }
        }

        private void UpdateDataFromPEMagic()
        {
            if (this.PEMagic == PEMagic.NT32)
                this.Data = new OptionalHeaderData32(this.optionalHeader);
            else
                this.Data = new OptionalHeaderData64(this.optionalHeader);
        }

        private void UpdateLength()
        {
            if (optionalHeader.PEMagic == PEMagic.NT32)
                this.Length = OptionalHeader.Size.NT32;
            else if (optionalHeader.PEMagic == PEMagic.NT64)
                this.Length = OptionalHeader.Size.NT64;
            else
                this.Length = 0;
        }

        private void BindAddressToPEHeader(PEHeaderModel peHeader)
        {
            peHeader.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "Address" || e.PropertyName == "Length")
                    this.Address = peHeader.Address + peHeader.Length;
            };
        }
    }
}