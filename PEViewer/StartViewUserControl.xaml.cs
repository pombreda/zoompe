using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace PEViewer
{
    public partial class StartViewUserControl : UserControl
    {
        public class OpenFileEventArgs : EventArgs
        {
            readonly string m_SafeFileName;
            readonly Func<string> getFileName;
            readonly Func<Stream> m_OpenStream;

            public OpenFileEventArgs(string safeFileName, Func<string> getFileName, Func<Stream> openStream)
            {
                if (getFileName == null)
                    throw new ArgumentNullException("getFileName");
                if (openStream == null)
                    throw new ArgumentNullException("getStream");

                this.m_SafeFileName = safeFileName;
                this.getFileName = getFileName;
                this.m_OpenStream = openStream;
            }

            public string SafeFileName { get { return m_SafeFileName; } }
            public string FileName { get { return getFileName(); } }

            public Stream OpenStream()
            {
                return m_OpenStream();
            }
        }

        public StartViewUserControl()
        {
            InitializeComponent();
        }

        public event EventHandler<OpenFileEventArgs> OpenFile;

        private void openFileButton_Click(object sender, RoutedEventArgs e)
        {
            var openFile = (OpenFileDialog)this.Resources["openFile"];
            if (openFile.ShowDialog() == true)
            {
                var temp = this.OpenFile;
                if (temp != null)
                {
                    var args = new OpenFileEventArgs(
                        openFile.SafeFileName,
                        () => openFile.FileName,
                        () => openFile.OpenFile());

                    temp(this, args);
                }
            }
        }

        private void openSelfButton_Click(object sender, RoutedEventArgs e)
        {
            var temp = this.OpenFile;
            if (temp != null)
            {
                var args = new OpenFileEventArgs(
                    new AssemblyName(this.GetType().Assembly.FullName).Name + ".exe",
                    () => typeof(StartViewUserControl).Assembly.Location,
                    () => File.OpenRead(typeof(StartViewUserControl).Assembly.Location));

                temp(this, args);
            }
        }
    }
}