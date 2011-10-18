using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Mi.PE;
using Mi.PE.PEFormat;

namespace Zoom.PE.Model
{
    public sealed class OptionalHeaderModel : PEFilePartModel
    {
        readonly OptionalHeader optionalHeader;

        public OptionalHeaderModel(OptionalHeader optionalHeader, PEHeaderModel peHeader)
        {
            this.optionalHeader = optionalHeader;

            BindAddressToPEHeader(peHeader);

            this.Address = peHeader.Address + peHeader.Length;

            UpdateLength();
        }

        public PEMagic PEMagic
        {
            get { return optionalHeader.PEMagic; }
            set
            {
                if (value == optionalHeader.PEMagic)
                    return;

                if (value != Mi.PE.PEFormat.PEMagic.NT32
                    && value != Mi.PE.PEFormat.PEMagic.NT64)
                    throw new ArgumentException("Invalid PEMagic value " + value + ".", "value");

                optionalHeader.PEMagic = value;
                OnPropertyChanged("PEMagic");
            }
        }
        
        public byte MajorLinkerVersion
        {
            get { return optionalHeader.MajorLinkerVersion; }
            set
            {
                if (value == optionalHeader.MajorLinkerVersion)
                    return;

                optionalHeader.MajorLinkerVersion = value;
                OnPropertyChanged("MajorLinkerVersion");
            }
        }
        
        public byte MinorLinkerVersion
        {
            get { return optionalHeader.MinorLinkerVersion; }
            set
            {
                if (value == optionalHeader.MinorLinkerVersion)
                    return;

                optionalHeader.MinorLinkerVersion = value;
                OnPropertyChanged("MinorLinkerVersion");
            }
        }
        
        public uint SizeOfCode
        {
            get { return optionalHeader.SizeOfCode; }
            set
            {
                if (value == optionalHeader.SizeOfCode)
                    return;

                optionalHeader.SizeOfCode = value;
                OnPropertyChanged("SizeOfCode");
            }
        }
        
        public uint SizeOfInitializedData
        {
            get { return optionalHeader.SizeOfInitializedData; }
            set
            {
                if (value == optionalHeader.SizeOfInitializedData)
                    return;

                optionalHeader.SizeOfInitializedData = value;
                OnPropertyChanged("SizeOfInitializedData");
            }
        }
        
        public uint SizeOfUninitializedData
        {
            get { return optionalHeader.SizeOfUninitializedData; }
            set
            {
                if (value == optionalHeader.SizeOfUninitializedData)
                    return;

                optionalHeader.SizeOfUninitializedData = value;
                OnPropertyChanged("SizeOfUninitializedData");
            }
        }
        
        public uint AddressOfEntryPoint
        {
            get { return optionalHeader.AddressOfEntryPoint; }
            set
            {
                if (value == optionalHeader.AddressOfEntryPoint)
                    return;

                optionalHeader.AddressOfEntryPoint = value;
                OnPropertyChanged("AddressOfEntryPoint");
            }
        }
        
        public uint BaseOfCode
        {
            get { return optionalHeader.BaseOfCode; }
            set
            {
                if (value == optionalHeader.BaseOfCode)
                    return;

                optionalHeader.BaseOfCode = value;
                OnPropertyChanged("BaseOfCode");
            }
        }
        
        public uint BaseOfData
        {
            get { return optionalHeader.BaseOfData; }
            set
            {
                if (value == optionalHeader.BaseOfData)
                    return;

                optionalHeader.BaseOfData = value;
                OnPropertyChanged("BaseOfData");
            }
        }
        
        public ulong ImageBase
        {
            get { return optionalHeader.ImageBase; }
            set
            {
                if (value == optionalHeader.ImageBase)
                    return;

                optionalHeader.ImageBase = value;
                OnPropertyChanged("ImageBase");
            }
        }
        
        public uint SectionAlignment
        {
            get { return optionalHeader.SectionAlignment; }
            set
            {
                if (value == optionalHeader.SectionAlignment)
                    return;

                optionalHeader.SectionAlignment = value;
                OnPropertyChanged("SectionAlignment");
            }
        }
        
        public uint FileAlignment
        {
            get { return optionalHeader.FileAlignment; }
            set
            {
                if (value == optionalHeader.FileAlignment)
                    return;

                optionalHeader.FileAlignment = value;
                OnPropertyChanged("FileAlignment");
            }
        }
        
        public ushort MajorOperatingSystemVersion
        {
            get { return optionalHeader.MajorOperatingSystemVersion; }
            set
            {
                if (value == optionalHeader.MajorOperatingSystemVersion)
                    return;

                optionalHeader.MajorOperatingSystemVersion = value;
                OnPropertyChanged("MajorOperatingSystemVersion");
            }
        }
        
        public ushort MinorOperatingSystemVersion
        {
            get { return optionalHeader.MinorOperatingSystemVersion; }
            set
            {
                if (value == optionalHeader.MinorOperatingSystemVersion)
                    return;

                optionalHeader.MinorOperatingSystemVersion = value;
                OnPropertyChanged("MinorOperatingSystemVersion");
            }
        }
        
        public ushort MajorImageVersion
        {
            get { return optionalHeader.MajorImageVersion; }
            set
            {
                if (value == optionalHeader.MajorImageVersion)
                    return;

                optionalHeader.MajorImageVersion = value;
                OnPropertyChanged("MajorImageVersion");
            }
        }
        
        public ushort MinorImageVersion
        {
            get { return optionalHeader.MinorImageVersion; }
            set
            {
                if (value == optionalHeader.MinorImageVersion)
                    return;

                optionalHeader.MinorImageVersion = value;
                OnPropertyChanged("MinorImageVersion");
            }
        }
        
        public ushort MajorSubsystemVersion
        {
            get { return optionalHeader.MajorSubsystemVersion; }
            set
            {
                if (value == optionalHeader.MajorSubsystemVersion)
                    return;

                optionalHeader.MajorSubsystemVersion = value;
                OnPropertyChanged("MajorSubsystemVersion");
            }
        }
        
        public ushort MinorSubsystemVersion
        {
            get { return optionalHeader.MinorSubsystemVersion; }
            set
            {
                if (value == optionalHeader.MinorSubsystemVersion)
                    return;

                optionalHeader.MinorSubsystemVersion = value;
                OnPropertyChanged("MinorSubsystemVersion");
            }
        }
        
        public uint Win32VersionValue
        {
            get { return optionalHeader.Win32VersionValue; }
            set
            {
                if (value == optionalHeader.Win32VersionValue)
                    return;

                optionalHeader.Win32VersionValue = value;
                OnPropertyChanged("Win32VersionValue");
            }
        }
        
        public uint SizeOfImage
        {
            get { return optionalHeader.SizeOfImage; }
            set
            {
                if (value == optionalHeader.SizeOfImage)
                    return;

                optionalHeader.SizeOfImage = value;
                OnPropertyChanged("SizeOfImage");
            }
        }
        
        public uint SizeOfHeaders
        {
            get { return optionalHeader.SizeOfHeaders; }
            set
            {
                if (value == optionalHeader.SizeOfHeaders)
                    return;

                optionalHeader.SizeOfHeaders = value;
                OnPropertyChanged("SizeOfHeaders");
            }
        }
        
        public uint CheckSum
        {
            get { return optionalHeader.CheckSum; }
            set
            {
                if (value == optionalHeader.CheckSum)
                    return;

                optionalHeader.CheckSum = value;
                OnPropertyChanged("CheckSum");
            }
        }
        
        public Subsystem Subsystem
        {
            get { return optionalHeader.Subsystem; }
            set
            {
                if (value == optionalHeader.Subsystem)
                    return;

                optionalHeader.Subsystem = value;
                OnPropertyChanged("Subsystem");
            }
        }
        
        public DllCharacteristics DllCharacteristics
        {
            get { return optionalHeader.DllCharacteristics; }
            set
            {
                if (value == optionalHeader.DllCharacteristics)
                    return;

                optionalHeader.DllCharacteristics = value;
                OnPropertyChanged("DllCharacteristics");
            }
        }
        
        public ulong SizeOfStackReserve
        {
            get { return optionalHeader.SizeOfStackReserve; }
            set
            {
                if (value == optionalHeader.SizeOfStackReserve)
                    return;

                optionalHeader.SizeOfStackReserve = value;
                OnPropertyChanged("SizeOfStackReserve");
            }
        }
        
        public ulong SizeOfStackCommit
        {
            get { return optionalHeader.SizeOfStackCommit; }
            set
            {
                if (value == optionalHeader.SizeOfStackCommit)
                    return;

                optionalHeader.SizeOfStackCommit = value;
                OnPropertyChanged("SizeOfStackCommit");
            }
        }
        
        public ulong SizeOfHeapReserve
        {
            get { return optionalHeader.SizeOfHeapReserve; }
            set
            {
                if (value == optionalHeader.SizeOfHeapReserve)
                    return;

                optionalHeader.SizeOfHeapReserve = value;
                OnPropertyChanged("SizeOfHeapReserve");
            }
        }
        
        public ulong SizeOfHeapCommit
        {
            get { return optionalHeader.SizeOfHeapCommit; }
            set
            {
                if (value == optionalHeader.SizeOfHeapCommit)
                    return;

                optionalHeader.SizeOfHeapCommit = value;
                OnPropertyChanged("SizeOfHeapCommit");
            }
        }
        
        public uint LoaderFlags
        {
            get { return optionalHeader.LoaderFlags; }
            set
            {
                if (value == optionalHeader.LoaderFlags)
                    return;

                optionalHeader.LoaderFlags = value;
                OnPropertyChanged("LoaderFlags");
            }
        }

        public uint NumberOfRvaAndSizes
        {
            get { return optionalHeader.NumberOfRvaAndSizes; }
            set
            {
                if (value == optionalHeader.NumberOfRvaAndSizes)
                    return;

                optionalHeader.NumberOfRvaAndSizes = value;
                OnPropertyChanged("NumberOfRvaAndSizes");
            }
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