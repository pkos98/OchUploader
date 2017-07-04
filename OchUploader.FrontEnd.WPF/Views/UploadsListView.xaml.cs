using OchUploader.ViewModel;
using System.Windows.Controls;

namespace OchUploader.FrontEnd.WPF.Views
{
    /// <summary>
    /// Interaktionslogik für UploadsListView.xaml
    /// </summary>
    public partial class UploadsListView : UserControl
    {

        public UploadsListView()
        {
            InitializeComponent();
        }

        private void DirectoryListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var directoryCollection = DataContext as DirectoryCollectionViewModel;
            if (directoryCollection == null)
                return;
            var source = e.Source;
            directoryCollection.SelectedDirectories.Clear();
            foreach (var directory in this.MainList.SelectedItems)
                directoryCollection.SelectedDirectories.Add(directory as DirectoryViewModel);
        }
    }
}