using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Mi.PE;
using Mi.PE.PEFormat;

namespace Zoom.PE.Model
{
    public abstract class PEFilePart : INotifyPropertyChanged
    {
        readonly string m_Title;
        ulong m_Address;
        ulong m_Length;

        protected PEFilePart(string title)
        {
            this.m_Title = title;
        }

        public string Title { get { return m_Title; } }

        public ulong Address
        {
            get { return m_Address; }
            protected set
            {
                if (value == this.Address)
                    return;

                this.m_Address = value;
                OnPropertyChanged("Address");
            }
        }

        public ulong Length
        {
            get { return m_Length; }
            protected set
            {
                if (value == this.Length)
                    return;

                this.m_Length = value;
                OnPropertyChanged("Length");
                OnPropertyChanged("Height");
            }
        }

        public double Height
        {
            get { return this.Length * 2 + 20; }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            var temp = this.PropertyChanged;
            if (temp != null)
                temp(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
