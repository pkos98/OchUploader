using OchUploader.Api;
using OchUploader.Model;
using OchUploader.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace OchUploader.FrontEnd.WPF.Views
{
    /// <summary>
    /// Interaktionslogik für SideBar.xaml
    /// </summary>
    public partial class SideBar : UserControl
    {
        public SideBar()
        {
            InitializeComponent();
            this.ProtocollFiltersList.SelectionChanged += ProtocollFiltersList_SelectionChanged;
            this.HostFiltersList.SelectionChanged += HostFiltersList_SelectionChanged;
            this.StateFiltersList.SelectionChanged += StateFiltersList_SelectionChanged;
        }

        private void StateFiltersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext == null)
                return;
            var selectedFilters = new List<UploadState>();
            foreach (var selectedItem in StateFiltersList.SelectedItems)
            {
                var selectedFilter = (UploadState)Enum.Parse(typeof(UploadState), selectedItem.ToString());
                selectedFilters.Add(selectedFilter);
            }
            (DataContext as DirectoryCollectionViewModel).StateFilters = selectedFilters;
        }

        private void HostFiltersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext == null)
                return;
            (DataContext as DirectoryCollectionViewModel).HostFilters = from string x in HostFiltersList.SelectedItems
                                                                        select x;
        }

        private void ProtocollFiltersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext == null)
                return;
            var selectedFilters = new List<AccessProtocoll>();
            foreach (var selectedItem in ProtocollFiltersList.SelectedItems)
            {
                var selectedProtocoll = (AccessProtocoll)Enum.Parse(typeof(AccessProtocoll), selectedItem.ToString());
                selectedFilters.Add(selectedProtocoll);
            }
            (DataContext as DirectoryCollectionViewModel).ProtocollFilters = selectedFilters;
        }
    }
}
