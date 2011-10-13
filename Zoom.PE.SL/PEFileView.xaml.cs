using System;
using System.Collections.Generic;
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

namespace Zoom.PE
{
    public partial class PEFileView : UserControl
    {
        readonly PEFile peFile;
        readonly string fileName;

        public PEFileView(PEFile peFile, string fileName)
        {
            this.peFile = peFile;
            this.fileName = fileName;

            InitializeComponent();

            this.fileNameTextBlock.Text = fileName;
        }
    }
}
