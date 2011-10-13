using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Mi.PE;
using Mi.PE.Internal;

namespace Zoom.PE
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();

            string fileName = new AssemblyName(this.GetType().Assembly.FullName).Name+".dll";
            var streamInfo = Application.GetResourceStream(new Uri(fileName, UriKind.Relative));

            var reader = new BinaryStreamReader(streamInfo.Stream, new byte[32]);

            var pe = new PEFile();
            pe.ReadFrom(reader);

            var view = new PEFileView(pe, fileName);

            this.LayoutRoot.Children.Add(view);
        }
    }
}