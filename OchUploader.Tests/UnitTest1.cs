using Microsoft.VisualStudio.TestTools.UnitTesting;
using OchUploader;
using OchUploader.Service;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OchUploader.Tests
{
    [TestClass]
    public class IncrementalFilterServiceTest
    {
        private IFilterService<string> _filterService;
        private ObservableCollection<string> _collectionSource;

        [TestInitialize]
        public void Initialize()
        {
            _collectionSource = new ObservableCollection<string>()
            {
                "Satz 1",
                "Satz 2",
                "Etwas anderes",
                "Noch was anderes"
            };

            _filterService = IoCContainer.Get<IFilterService<string>>();
            _filterService.CollectionSource = _collectionSource;
            _filterService.FilterFunction = FilterElement;
        }

        public void TestFilteredCollection()
        {
        }

        private bool FilterElement(string x)
        {
            return x == _collectionSource[0] || x == _collectionSource[1];
        }
    }
}
