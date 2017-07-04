using OchUploader.ViewModel;
using System.Windows.Controls;
using WinForms = System.Windows.Forms;

namespace OchUploader.FrontEnd.WPF.Views
{
    /// <summary>
    /// Interaktionslogik für NewDirectorySettingsView.xaml
    /// </summary>
    public partial class NewDirectoryPrototypeView: UserControl
    {
        public NewDirectoryPrototypeView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var folderDialog = new WinForms.FolderBrowserDialog();
            folderDialog.Description = "Select the directory of the new uploads you want to add";
            var dialogResult = folderDialog.ShowDialog();
            if (dialogResult != WinForms.DialogResult.OK)
                return;

            var newDirectory = (DataContext as DirectoryCollectionViewModel).NewDirectoryPrototype;
            newDirectory.Path = folderDialog.SelectedPath;
            //PathBox.Text = folderDialog.SelectedPath;
        }
    }
}
