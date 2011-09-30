using System;
using System.Collections.Generic;
using System.Linq;
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
using Mi.PE;

namespace PEViewer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void startView_OpenFile(object sender, StartViewUserControl.OpenFileEventArgs e)
        {
            try
            {
                using(var stream = e.OpenStream())
                {
                    var pe = PEFile.FromStream(stream);
                    this.Content = new ScrollViewer
                    {
                        VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                        HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                        Padding = new Thickness(5),
                        Content = new PEFileView { DataContext = pe }
                    };
                    this.Title = e.SafeFileName;
                }
            }
            catch (Exception openFileError)
            {
                MessageBox.Show(openFileError.GetType().Name, openFileError.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}