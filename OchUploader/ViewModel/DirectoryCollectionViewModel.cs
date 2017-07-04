using OchUploader.Infrastructure;
using OchUploader.Model;
using OchUploader.Service;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using OchUploader.Service.Persistence;
using OchUploader.Api;

namespace OchUploader.ViewModel
{
    public class DirectoryCollectionViewModel : ViewModelBase
    {

        #region Private fields

        private readonly IDirectoryManagerService _directoryManager;
        private readonly IDirectoryModelService _directoryModelService;
        private readonly IFilterService<DirectoryViewModel> _filterService;
        private readonly INotificationService _notificationService;
        private readonly ObservableCollection<DirectoryViewModel> _allDirectories =
                         new ObservableCollection<DirectoryViewModel>();
        private int _loadedTimeRange;
        private NewDirectoryPrototypeViewModel _newDirectory;
        private string _filterString;
        private IEnumerable<UploadState> _stateFilters = new[] { UploadState.All };
        private IEnumerable<string> _hostFilters = new[] { "All" };
        private IEnumerable<AccessProtocoll> _protocollFilters = new[] { AccessProtocoll.All };

        #endregion

        #region Properties

        public DirectoryViewModel SelectedDirectory { get; set; }
        public ObservableCollection<DirectoryViewModel> SelectedDirectories { get; private set; } =
            new ObservableCollection<DirectoryViewModel>();
        public ObservableCollection<DirectoryViewModel> DirectoriesView => _filterService.FilteredCollection;
        public NewDirectoryPrototypeViewModel NewDirectoryPrototype
        {
            get { return _newDirectory; }
            set
            {
                if (_newDirectory == value) return;
                _newDirectory = value;
                RaisePropertyChanged();
            }
        }
        public int LoadedTimeRange // Not implemented yet
        {
            get
            {
                return _loadedTimeRange;
            }

            set
            {
                if (_loadedTimeRange == value)
                    return;

                _loadedTimeRange = value;
                // TODO: Load ASYNC
            }
        }

        #region Filter Properties

        public string FilterString
        {
            get { return _filterString; }
            set
            {
                if (value == _filterString)
                    return;

                _filterString = value.ToLower();
                _filterService.Refresh();
            }
        }
        public IEnumerable<UploadState> StateFilters
        {
            get { return _stateFilters; }
            set
            {
                if (value == _stateFilters)
                    return;

                _stateFilters = value;
                _filterService.Refresh();
            }
        }
        public IEnumerable<AccessProtocoll> ProtocollFilters
        {
            get { return _protocollFilters; }
            set
            {
                if (value == _protocollFilters)
                    return;
                _protocollFilters = value;
                _filterService.Refresh();
            }
        }
        public IEnumerable<string> HostFilters
        {
            get { return _hostFilters; }
            set
            {
                if (value == _hostFilters)
                    return;
                _hostFilters = value;
                _filterService.Refresh();
            }
        }

        #endregion

        #region Commands

        public RelayCommand BeginAddNewDirectoryCommand { get; private set; } =
            new RelayCommand();
        public RelayCommand FinishAddNewDirectoryCommand { get; private set; } =
            new RelayCommand();
        public RelayCommand CancelNewDirectoryCommand { get; private set; } =
            new RelayCommand();
        public RelayCommand SaveDirectoriesCommand { get; private set; } =
            new RelayCommand();
        public RelayCommand ReloadFromDatabaseCommand { get; private set; } =
            new RelayCommand();
        public RelayCommand RemoveSelectedDirectoriesCommand { get; private set; } =
            new RelayCommand();
        public RelayCommand UnselectSelectedDirectoriesCommand { get; private set; } =
            new RelayCommand();
        public RelayCommand PauseAutoInjectionCommand = new RelayCommand();
        

        #endregion

        #endregion

        public DirectoryCollectionViewModel(IDirectoryManagerService directoryManager,
                                            IFilterService<DirectoryViewModel> filterService,
                                            INotificationService notificationService)
        {
            _notificationService = notificationService;

                        _directoryManager = directoryManager;
            _directoryManager.NewDirectoriesAddedEvent += DirectoryManager_NewDirectoryInjectedEvent;
            _directoryManager.DownDirectoryDetectedEvent += DirectoryManager_DownDirectoryDetectedEvent;
            _directoryModelService = _directoryManager.DirectoryModelService;

            #region Command <-> Execute-function assignment

            BeginAddNewDirectoryCommand.ExecuteFunc = () => ExecuteAndSetLastAction(BeginAddNewDirectory);
            BeginAddNewDirectoryCommand.CanExecuteFunc = (x) => NewDirectoryPrototype == null;
            FinishAddNewDirectoryCommand.ExecuteFunc = () => ExecuteAndSetLastAction(FinishAddNewDirectory);
            CancelNewDirectoryCommand.ExecuteFunc = () => ExecuteAndSetLastAction(CancelNewDirectory);
            CancelNewDirectoryCommand.CanExecuteFunc = (x) => NewDirectoryPrototype != null;

            SaveDirectoriesCommand.ExecuteFunc = () => ExecuteAndSetLastAction(SaveDirectories);
            ReloadFromDatabaseCommand.ExecuteFunc = () => ExecuteAndSetLastAction(ReloadFromDatabase);
            RemoveSelectedDirectoriesCommand.ExecuteFunc = () => ExecuteAndSetLastAction(RemoveSelectedDirectories);
            RemoveSelectedDirectoriesCommand.CanExecuteFunc = (x) => SelectedDirectories.Count > 0;
            UnselectSelectedDirectoriesCommand.ExecuteFunc = () => ExecuteAndSetLastAction(UnselectSelectedDirectories);
            UnselectSelectedDirectoriesCommand.CanExecuteFunc = (x) => SelectedDirectories.Count > 0;

            //PauseAutoInjectionCommand.ExecuteFunc = () => ExecuteAndSetLastAction(PauseAutoInjection);
            //PauseAutoInjectionCommand.CanExecuteFunc = (x) => _directoryInjectorService.IsStopped == false;

            #endregion

            foreach (var directory in _directoryModelService.GetAll())
                _allDirectories.Add(directory.ToViewModel());

            _filterService = filterService;
            _filterService.FilterFunction = DirectoryFilter;
            // By assigning the collectionsource after the filling of _alldirectories,
            // we reduce the overload, because the collectionsourcechanged event is only called once
            _filterService.CollectionSource = _allDirectories;

        }


        #region Command Implementations

        [LastAction("Unselected all selected directories")]
        private void UnselectSelectedDirectories(object obj)
        {
            SelectedDirectory = null;
            RaisePropertyChanged("SelectedDirectory");
        }

        [LastAction("Removed selected directories")]
        private void RemoveSelectedDirectories(object obj)
        {
            for (int i = SelectedDirectories.Count - 1; i >= 0; i--)
            {
                var selectedDirectory = SelectedDirectories[i];
                _directoryModelService.Remove(selectedDirectory.DirectoryModel);
                _allDirectories.Remove(selectedDirectory);
            }
        }

        [LastAction("Begin creation of new directory")]
        private void BeginAddNewDirectory(object obj)
        {
            NewDirectoryPrototype = NewDirectoryPrototypeViewModel.Create();
        }

        [LastAction("Finished creation of new directory")]
        private void FinishAddNewDirectory(object obj)
        {
            var newDirectory = _directoryModelService.CreateDirectory(NewDirectoryPrototype.Path, 
                                NewDirectoryPrototype.ProviderName, NewDirectoryPrototype.Host);
            _directoryModelService.AddToDb(newDirectory);
            _allDirectories.Add(newDirectory.ToViewModel());
            NewDirectoryPrototype = null;
        }

        [LastAction("Canceled creation of new directory")]
        private void CancelNewDirectory(object obj)
        {
            NewDirectoryPrototype = null;
        }

        [LastAction("Reloaded all directories from database (sync with database)")]
        private void ReloadFromDatabase(object obj)
        {
            _allDirectories.Clear();
            _directoryModelService.Rollback();
            foreach (var directory in _directoryModelService.GetAll())
                _allDirectories.Add(directory.ToViewModel());
        }

        [LastAction("Saved all directories")]
        private void SaveDirectories(object obj)
        {
            _directoryModelService.Save();
        }

        [LastAction("Stopped auto-injection of new directories")]
        private void PauseAutoInjection(object obj)
        {
            //_directoryInjectorService.Stop();
        }

        #endregion

        /// <summary>
        /// Raised when a new directory is injected to the manager via an injector
        /// </summary>
        /// <param name="newDirectories"></param>
        private void DirectoryManager_NewDirectoryInjectedEvent(IEnumerable<DirectoryModel> newDirectories)
        {
            //_notificationService.Info($"Added 0 new directories");
            //foreach (var newDirectory in newDirectories)
            //    App.Current.Dispatcher.Invoke(() => _allDirectories.Add(newDirectory.ToViewModel()));

        }

        /// <summary>
        /// Raised when a directory which is marked as down was detected
        /// </summary>
        /// <param name="directory"></param>
        private void DirectoryManager_DownDirectoryDetectedEvent(DirectoryModel directory)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Predicate which tests if the given directory meets the filter-criteria
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool DirectoryFilter(object obj)
        {
            var directory = obj as DirectoryViewModel;

            var containsNameFilter = String.IsNullOrEmpty(FilterString) ?
                                     true : directory.Name.ToLower().Contains(FilterString);
            var containsStateFilters = StateFilters.Contains(UploadState.All) ?
                                           true : StateFilters.Contains(directory.State);
            var containsProtocollFilters = true;
            var containsHostFilters = HostFilters.Contains("All") ?
                                        true : HostFilters.Contains(directory.Host);

            return containsNameFilter && containsStateFilters && 
                   containsProtocollFilters && containsHostFilters;
        }

    }
}
