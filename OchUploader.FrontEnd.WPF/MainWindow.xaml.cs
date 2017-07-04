using MahApps.Metro.Controls;
using OchUploader.ViewModel;
using System;

namespace OchUploader.FrontEnd.WPF
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                int d = 0;
            }
        }

        private void MetroWindow_Initialized(object sender, EventArgs e)
        {
            try
            {
                DataContext = IocContainer.Get<DirectoryCollectionViewModel>();
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}