using System;
using System.Collections.Generic;
using System.IO;
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

namespace PEViewer.SL
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();

            this.Drop += new DragEventHandler(MainPage_Drop);
        }

        void MainPage_Drop(object sender, DragEventArgs e)
        {
            if (e.Data != null)
            {
                var files = e.Data.GetData(DataFormats.FileDrop) as FileInfo[];

                if (files != null)
                {
                    foreach (var fi in files)
                    {
                        AddFile(fi);
                    }
                }
            }
        }

        private void AddFile(FileInfo fi)
        {
            var tabControl = LayoutRoot.Children.OfType<TabControl>().FirstOrDefault();
            if (tabControl == null)
            {
                tabControl = new TabControl();
                LayoutRoot.Children.Add(tabControl);
            }

            PEFile pe;
            using (var stream = fi.OpenRead())
            {
                pe = PEFile.FromStream(stream);
            }

            var tabItem = new TabItem
            {
                Header = fi.Name,
                Content = new ScrollViewer
                {
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                    //Background = Brushes.White,
                    Padding = new Thickness(5),
                    Content = new PEFileView { DataContext = pe }
                }
            };

            tabControl.Items.Add(tabItem);

            tabControl.SelectedIndex = tabControl.Items.Count;
        }

        private void openFileButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "DLL and EXE files|*.dll;*.exe|All files|*.*",
                Multiselect = true
            };

            if(dialog.ShowDialog()!=true)
                return;

            foreach (var fi in dialog.Files)
            {
                AddFile(fi);
            }
        }
    }
}
