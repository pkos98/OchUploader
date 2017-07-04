using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OchUploader.FrontEnd.WPF.Views
{
    /// <summary>
    /// Interaktionslogik für VerticalMenu.xaml
    /// </summary>
    public partial class VerticalMenu : UserControl
    {
        private readonly MainWindow _mainWindow = Application.Current.MainWindow as MainWindow;
        private readonly UploadsListView _uploadsView = new UploadsListView();

        public VerticalMenu()
        {
            InitializeComponent();
            //_mainWindow.MainContent.Content = _uploadsView;
        }

        private void UploadView_MouseDown(object sender, MouseEventArgs e)
        {
            return;
            //_mainWindow.MainContent.Content = _uploadsView;
        }


        private void SettingsView_MouseDown(object sender, MouseEventArgs mouseEventArgs)
        {
            return;
            //var text = new TextBlock();
            //text.Text = "Hallo!";
            //text.FontSize = 28.0;
            //_mainWindow.MainContent.Content = text;
        }
    }
}
