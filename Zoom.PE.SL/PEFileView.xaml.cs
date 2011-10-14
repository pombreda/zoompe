using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Mi.PE;
using Mi.PE.PEFormat;

namespace Zoom.PE
{
    public partial class PEFileView : UserControl, INotifyPropertyChanged
    {
        readonly PEFile peFile;
        readonly string fileName;

        public PEFileView(PEFile peFile, string fileName)
        {
            this.peFile = peFile;
            this.fileName = fileName;

            InitializeComponent();

            this.LayoutRoot.DataContext = this;
        }

        public string FileName { get { return this.fileName; } }

        #region DOS header

        public ushort cblp
        {
            get { return peFile.DosHeader.cblp; }
            set
            {
                if (value == peFile.DosHeader.cblp)
                    return;

                peFile.DosHeader.cblp = value;
                OnPropertyChanged("cblp");
            }
        }

        public ushort cp
        {
            get { return peFile.DosHeader.cp; }
            set
            {
                if (value == peFile.DosHeader.cp)
                    return;

                peFile.DosHeader.cp = value;
                OnPropertyChanged("cp");
            }
        }

        public ushort crlc
        {
            get { return peFile.DosHeader.crlc; }
            set
            {
                if (value == peFile.DosHeader.crlc)
                    return;

                peFile.DosHeader.crlc = value;
                OnPropertyChanged("crlc");
            }
        }

        public ushort cparhdr
        {
            get { return peFile.DosHeader.cparhdr; }
            set
            {
                if (value == peFile.DosHeader.cparhdr)
                    return;

                peFile.DosHeader.cparhdr = value;
                OnPropertyChanged("cparhdr");
            }
        }

        public ushort minalloc
        {
            get { return peFile.DosHeader.minalloc; }
            set
            {
                if (value == peFile.DosHeader.minalloc)
                    return;

                peFile.DosHeader.minalloc = value;
                OnPropertyChanged("minalloc");
            }
        }

        public ushort maxalloc
        {
            get { return peFile.DosHeader.maxalloc; }
            set
            {
                if (value == peFile.DosHeader.maxalloc)
                    return;

                peFile.DosHeader.maxalloc = value;
                OnPropertyChanged("maxalloc");
            }
        }

        public ushort ss
        {
            get { return peFile.DosHeader.ss; }
            set
            {
                if (value == peFile.DosHeader.ss)
                    return;

                peFile.DosHeader.ss = value;
                OnPropertyChanged("ss");
            }
        }

        public ushort sp
        {
            get { return peFile.DosHeader.sp; }
            set
            {
                if (value == peFile.DosHeader.sp)
                    return;

                peFile.DosHeader.sp = value;
                OnPropertyChanged("sp");
            }
        }

        public ushort csum
        {
            get { return peFile.DosHeader.csum; }
            set
            {
                if (value == peFile.DosHeader.csum)
                    return;

                peFile.DosHeader.csum = value;
                OnPropertyChanged("csum");
            }
        }

        public ushort ip
        {
            get { return peFile.DosHeader.ip; }
            set
            {
                if (value == peFile.DosHeader.ip)
                    return;

                peFile.DosHeader.ip = value;
                OnPropertyChanged("ip");
            }
        }

        public ushort cs
        {
            get { return peFile.DosHeader.cs; }
            set
            {
                if (value == peFile.DosHeader.cs)
                    return;

                peFile.DosHeader.cs = value;
                OnPropertyChanged("cs");
            }
        }

        public ushort lfarlc
        {
            get { return peFile.DosHeader.lfarlc; }
            set
            {
                if (value == peFile.DosHeader.lfarlc)
                    return;

                peFile.DosHeader.lfarlc = value;
                OnPropertyChanged("lfarlc");
            }
        }

        public ushort ovno
        {
            get { return peFile.DosHeader.ovno; }
            set
            {
                if (value == peFile.DosHeader.ovno)
                    return;

                peFile.DosHeader.ovno = value;
                OnPropertyChanged("ovno");
            }
        }

        public ulong res1
        {
            get { return peFile.DosHeader.res1; }
            set
            {
                if (value == peFile.DosHeader.res1)
                    return;

                peFile.DosHeader.res1 = value;
                OnPropertyChanged("res1");
            }
        }
        
        public ushort oemid
        {
            get { return peFile.DosHeader.oemid; }
            set
            {
                if (value == peFile.DosHeader.oemid)
                    return;

                peFile.DosHeader.oemid = value;
                OnPropertyChanged("oemid");
            }
        }
        
        public ushort oeminfo
        {
            get { return peFile.DosHeader.oeminfo; }
            set
            {
                if (value == peFile.DosHeader.oeminfo)
                    return;

                peFile.DosHeader.oeminfo = value;
                OnPropertyChanged("oeminfo");
            }
        }
        
        public uint ReservedNumber0
        {
            get { return peFile.DosHeader.ReservedNumber0; }
            set
            {
                if (value == peFile.DosHeader.ReservedNumber0)
                    return;

                peFile.DosHeader.ReservedNumber0 = value;
                OnPropertyChanged("ReservedNumber0");
            }
        }
        
        public uint ReservedNumber1
        {
            get { return peFile.DosHeader.ReservedNumber1; }
            set
            {
                if (value == peFile.DosHeader.ReservedNumber1)
                    return;

                peFile.DosHeader.ReservedNumber1 = value;
                OnPropertyChanged("ReservedNumber1");
            }
        }
        
        public uint ReservedNumber2
        {
            get { return peFile.DosHeader.ReservedNumber2; }
            set
            {
                if (value == peFile.DosHeader.ReservedNumber2)
                    return;

                peFile.DosHeader.ReservedNumber2 = value;
                OnPropertyChanged("ReservedNumber2");
            }
        }
        
        public uint ReservedNumber3
        {
            get { return peFile.DosHeader.ReservedNumber3; }
            set
            {
                if (value == peFile.DosHeader.ReservedNumber3)
                    return;

                peFile.DosHeader.ReservedNumber3 = value;
                OnPropertyChanged("ReservedNumber3");
            }
        }
        
        public uint ReservedNumber4
        {
            get { return peFile.DosHeader.ReservedNumber4; }
            set
            {
                if (value == peFile.DosHeader.ReservedNumber4)
                    return;

                peFile.DosHeader.ReservedNumber4 = value;
                OnPropertyChanged("ReservedNumber4");
            }
        }
        
        public uint lfanew
        {
            get { return peFile.DosHeader.lfanew; }
            set
            {
                if (value == peFile.DosHeader.lfanew)
                    return;

                peFile.DosHeader.lfanew = value;

                int dosStubSize = (int)(value - DosHeader.HeaderSize);
                if (dosStubSize > 0)
                {
                    if (peFile.DosStub != null)
                    {
                        Array.Resize(ref peFile.DosStub, dosStubSize);
                    }
                    else
                    {
                        peFile.DosStub = new byte[dosStubSize];
                    }
                }
                else
                {
                    peFile.DosStub = null;
                }

                OnPropertyChanged("lfanew");
                OnPropertyChanged("IsDosStubPresent");
                OnPropertyChanged("OptionalHeaderOffset");
                OnPropertyChanged("DosStub");
            }
        }

        #endregion

        public bool IsDosStubPresent
        {
            get
            {
                return peFile.DosHeader.lfanew > DosHeader.HeaderSize;
            }
        }

        public uint OptionalHeaderOffset
        {
            get
            {
                return peFile.DosHeader.lfanew + PEHeader.Size;
            }
        }

        public byte[] DosStub
        {
            get { return peFile.DosStub; }
        }

        #region PE header

        public Machine Machine
        {
            get { return peFile.PEHeader.Machine; }
            set
            {
                if (value == peFile.PEHeader.Machine)
                    return;

                peFile.PEHeader.Machine = value;
                OnPropertyChanged("Machine");
            }
        }
        
        public ushort NumberOfSections
        {
            get { return peFile.PEHeader.NumberOfSections; }
            set
            {
                if (value == peFile.PEHeader.NumberOfSections)
                    return;

                peFile.PEHeader.NumberOfSections = value;
                OnPropertyChanged("NumberOfSections");
            }
        }
        
        public DateTime Timestamp
        {
            get { return peFile.PEHeader.Timestamp; }
            set
            {
                if (value == peFile.PEHeader.Timestamp)
                    return;

                peFile.PEHeader.Timestamp = value;
                OnPropertyChanged("Timestamp");
            }
        }
        
        public uint PointerToSymbolTable
        {
            get { return peFile.PEHeader.PointerToSymbolTable; }
            set
            {
                if (value == peFile.PEHeader.PointerToSymbolTable)
                    return;

                peFile.PEHeader.PointerToSymbolTable = value;
                OnPropertyChanged("PointerToSymbolTable");
            }
        }
        
        public uint NumberOfSymbols
        {
            get { return peFile.PEHeader.NumberOfSymbols; }
            set
            {
                if (value == peFile.PEHeader.NumberOfSymbols)
                    return;

                peFile.PEHeader.NumberOfSymbols = value;
                OnPropertyChanged("NumberOfSymbols");
            }
        }
        
        public ushort SizeOfOptionalHeader
        {
            get { return peFile.PEHeader.SizeOfOptionalHeader; }
            set
            {
                if (value == peFile.PEHeader.SizeOfOptionalHeader)
                    return;

                peFile.PEHeader.SizeOfOptionalHeader = value;
                OnPropertyChanged("SizeOfOptionalHeader");
            }
        }
        
        public ImageCharacteristics Characteristics
        {
            get { return peFile.PEHeader.Characteristics; }
            set
            {
                if (value == peFile.PEHeader.Characteristics)
                    return;

                peFile.PEHeader.Characteristics = value;
                OnPropertyChanged("Characteristics");
            }
        }

        #endregion

        #region Optional header

        public PEMagic PEMagic
        {
            get { return peFile.OptionalHeader.PEMagic; }
            set
            {
                if (value == peFile.OptionalHeader.PEMagic)
                    return;

                if (value != Mi.PE.PEFormat.PEMagic.NT32
                    && value != Mi.PE.PEFormat.PEMagic.NT64)
                    throw new ArgumentException("Invalid PEMagic value " + value + ".", "value");

                peFile.OptionalHeader.PEMagic = value;
                OnPropertyChanged("PEMagic");
            }
        }
        
        public byte MajorLinkerVersion
        {
            get { return peFile.OptionalHeader.MajorLinkerVersion; }
            set
            {
                if (value == peFile.OptionalHeader.MajorLinkerVersion)
                    return;

                peFile.OptionalHeader.MajorLinkerVersion = value;
                OnPropertyChanged("MajorLinkerVersion");
            }
        }
        
        public byte MinorLinkerVersion
        {
            get { return peFile.OptionalHeader.MinorLinkerVersion; }
            set
            {
                if (value == peFile.OptionalHeader.MinorLinkerVersion)
                    return;

                peFile.OptionalHeader.MinorLinkerVersion = value;
                OnPropertyChanged("MinorLinkerVersion");
            }
        }
        
        public uint SizeOfCode
        {
            get { return peFile.OptionalHeader.SizeOfCode; }
            set
            {
                if (value == peFile.OptionalHeader.SizeOfCode)
                    return;

                peFile.OptionalHeader.SizeOfCode = value;
                OnPropertyChanged("SizeOfCode");
            }
        }
        
        public uint SizeOfInitializedData
        {
            get { return peFile.OptionalHeader.SizeOfInitializedData; }
            set
            {
                if (value == peFile.OptionalHeader.SizeOfInitializedData)
                    return;

                peFile.OptionalHeader.SizeOfInitializedData = value;
                OnPropertyChanged("SizeOfInitializedData");
            }
        }
        
        public uint SizeOfUninitializedData
        {
            get { return peFile.OptionalHeader.SizeOfUninitializedData; }
            set
            {
                if (value == peFile.OptionalHeader.SizeOfUninitializedData)
                    return;

                peFile.OptionalHeader.SizeOfUninitializedData = value;
                OnPropertyChanged("SizeOfUninitializedData");
            }
        }
        
        public uint AddressOfEntryPoint
        {
            get { return peFile.OptionalHeader.AddressOfEntryPoint; }
            set
            {
                if (value == peFile.OptionalHeader.AddressOfEntryPoint)
                    return;

                peFile.OptionalHeader.AddressOfEntryPoint = value;
                OnPropertyChanged("AddressOfEntryPoint");
            }
        }
        
        public uint BaseOfCode
        {
            get { return peFile.OptionalHeader.BaseOfCode; }
            set
            {
                if (value == peFile.OptionalHeader.BaseOfCode)
                    return;

                peFile.OptionalHeader.BaseOfCode = value;
                OnPropertyChanged("BaseOfCode");
            }
        }
        
        public uint BaseOfData
        {
            get { return peFile.OptionalHeader.BaseOfData; }
            set
            {
                if (value == peFile.OptionalHeader.BaseOfData)
                    return;

                peFile.OptionalHeader.BaseOfData = value;
                OnPropertyChanged("BaseOfData");
            }
        }
        
        public ulong ImageBase
        {
            get { return peFile.OptionalHeader.ImageBase; }
            set
            {
                if (value == peFile.OptionalHeader.ImageBase)
                    return;

                peFile.OptionalHeader.ImageBase = value;
                OnPropertyChanged("ImageBase");
            }
        }
        
        public uint SectionAlignment
        {
            get { return peFile.OptionalHeader.SectionAlignment; }
            set
            {
                if (value == peFile.OptionalHeader.SectionAlignment)
                    return;

                peFile.OptionalHeader.SectionAlignment = value;
                OnPropertyChanged("SectionAlignment");
            }
        }
        
        public uint FileAlignment
        {
            get { return peFile.OptionalHeader.FileAlignment; }
            set
            {
                if (value == peFile.OptionalHeader.FileAlignment)
                    return;

                peFile.OptionalHeader.FileAlignment = value;
                OnPropertyChanged("FileAlignment");
            }
        }
        
        public ushort MajorOperatingSystemVersion
        {
            get { return peFile.OptionalHeader.MajorOperatingSystemVersion; }
            set
            {
                if (value == peFile.OptionalHeader.MajorOperatingSystemVersion)
                    return;

                peFile.OptionalHeader.MajorOperatingSystemVersion = value;
                OnPropertyChanged("MajorOperatingSystemVersion");
            }
        }
        
        public ushort MinorOperatingSystemVersion
        {
            get { return peFile.OptionalHeader.MinorOperatingSystemVersion; }
            set
            {
                if (value == peFile.OptionalHeader.MinorOperatingSystemVersion)
                    return;

                peFile.OptionalHeader.MinorOperatingSystemVersion = value;
                OnPropertyChanged("MinorOperatingSystemVersion");
            }
        }
        
        public ushort MajorImageVersion
        {
            get { return peFile.OptionalHeader.MajorImageVersion; }
            set
            {
                if (value == peFile.OptionalHeader.MajorImageVersion)
                    return;

                peFile.OptionalHeader.MajorImageVersion = value;
                OnPropertyChanged("MajorImageVersion");
            }
        }
        
        public ushort MinorImageVersion
        {
            get { return peFile.OptionalHeader.MinorImageVersion; }
            set
            {
                if (value == peFile.OptionalHeader.MinorImageVersion)
                    return;

                peFile.OptionalHeader.MinorImageVersion = value;
                OnPropertyChanged("MinorImageVersion");
            }
        }
        
        public ushort MajorSubsystemVersion
        {
            get { return peFile.OptionalHeader.MajorSubsystemVersion; }
            set
            {
                if (value == peFile.OptionalHeader.MajorSubsystemVersion)
                    return;

                peFile.OptionalHeader.MajorSubsystemVersion = value;
                OnPropertyChanged("MajorSubsystemVersion");
            }
        }
        
        public ushort MinorSubsystemVersion
        {
            get { return peFile.OptionalHeader.MinorSubsystemVersion; }
            set
            {
                if (value == peFile.OptionalHeader.MinorSubsystemVersion)
                    return;

                peFile.OptionalHeader.MinorSubsystemVersion = value;
                OnPropertyChanged("MinorSubsystemVersion");
            }
        }
        
        public uint Win32VersionValue
        {
            get { return peFile.OptionalHeader.Win32VersionValue; }
            set
            {
                if (value == peFile.OptionalHeader.Win32VersionValue)
                    return;

                peFile.OptionalHeader.Win32VersionValue = value;
                OnPropertyChanged("Win32VersionValue");
            }
        }
        
        public uint SizeOfImage
        {
            get { return peFile.OptionalHeader.SizeOfImage; }
            set
            {
                if (value == peFile.OptionalHeader.SizeOfImage)
                    return;

                peFile.OptionalHeader.SizeOfImage = value;
                OnPropertyChanged("SizeOfImage");
            }
        }
        
        public uint SizeOfHeaders
        {
            get { return peFile.OptionalHeader.SizeOfHeaders; }
            set
            {
                if (value == peFile.OptionalHeader.SizeOfHeaders)
                    return;

                peFile.OptionalHeader.SizeOfHeaders = value;
                OnPropertyChanged("SizeOfHeaders");
            }
        }
        
        public uint CheckSum
        {
            get { return peFile.OptionalHeader.CheckSum; }
            set
            {
                if (value == peFile.OptionalHeader.CheckSum)
                    return;

                peFile.OptionalHeader.CheckSum = value;
                OnPropertyChanged("CheckSum");
            }
        }
        
        public Subsystem Subsystem
        {
            get { return peFile.OptionalHeader.Subsystem; }
            set
            {
                if (value == peFile.OptionalHeader.Subsystem)
                    return;

                peFile.OptionalHeader.Subsystem = value;
                OnPropertyChanged("Subsystem");
            }
        }
        
        public DllCharacteristics DllCharacteristics
        {
            get { return peFile.OptionalHeader.DllCharacteristics; }
            set
            {
                if (value == peFile.OptionalHeader.DllCharacteristics)
                    return;

                peFile.OptionalHeader.DllCharacteristics = value;
                OnPropertyChanged("DllCharacteristics");
            }
        }
        
        public ulong SizeOfStackReserve
        {
            get { return peFile.OptionalHeader.SizeOfStackReserve; }
            set
            {
                if (value == peFile.OptionalHeader.SizeOfStackReserve)
                    return;

                peFile.OptionalHeader.SizeOfStackReserve = value;
                OnPropertyChanged("SizeOfStackReserve");
            }
        }
        
        public ulong SizeOfStackCommit
        {
            get { return peFile.OptionalHeader.SizeOfStackCommit; }
            set
            {
                if (value == peFile.OptionalHeader.SizeOfStackCommit)
                    return;

                peFile.OptionalHeader.SizeOfStackCommit = value;
                OnPropertyChanged("SizeOfStackCommit");
            }
        }
        
        public ulong SizeOfHeapReserve
        {
            get { return peFile.OptionalHeader.SizeOfHeapReserve; }
            set
            {
                if (value == peFile.OptionalHeader.SizeOfHeapReserve)
                    return;

                peFile.OptionalHeader.SizeOfHeapReserve = value;
                OnPropertyChanged("SizeOfHeapReserve");
            }
        }
        
        public ulong SizeOfHeapCommit
        {
            get { return peFile.OptionalHeader.SizeOfHeapCommit; }
            set
            {
                if (value == peFile.OptionalHeader.SizeOfHeapCommit)
                    return;

                peFile.OptionalHeader.SizeOfHeapCommit = value;
                OnPropertyChanged("SizeOfHeapCommit");
            }
        }
        
        public uint LoaderFlags
        {
            get { return peFile.OptionalHeader.LoaderFlags; }
            set
            {
                if (value == peFile.OptionalHeader.LoaderFlags)
                    return;

                peFile.OptionalHeader.LoaderFlags = value;
                OnPropertyChanged("LoaderFlags");
            }
        }

        public uint NumberOfRvaAndSizes
        {
            get { return peFile.OptionalHeader.NumberOfRvaAndSizes; }
            set
            {
                if (value == peFile.OptionalHeader.NumberOfRvaAndSizes)
                    return;

                peFile.OptionalHeader.NumberOfRvaAndSizes = value;
                OnPropertyChanged("NumberOfRvaAndSizes");
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var temp = this.PropertyChanged;
            if (temp != null)
                temp(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
