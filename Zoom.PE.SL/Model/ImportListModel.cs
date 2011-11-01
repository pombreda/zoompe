using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Mi.PE;
using Mi.PE.PEFormat;

namespace Zoom.PE.Model
{
    public sealed class ImportListModel : ReadOnlyObservableCollection<FunctionImportModel>
    {
        public ImportListModel()
            : base(new ObservableCollection<FunctionImportModel>())
        {
        }

        internal void Add(FunctionImportModel item)
        {
            this.Items.Add(item);
        }

        internal void Remove(FunctionImportModel item)
        {
            this.Items.Remove(item);
        }

        internal void Clear()
        {
            this.Items.Clear();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            var propertyChangedHandler = this.PropertyChanged;
            if (propertyChangedHandler != null)
                propertyChangedHandler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
