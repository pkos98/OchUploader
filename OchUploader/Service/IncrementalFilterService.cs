using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace OchUploader.Service
{
    /// <summary>
    /// Filters incrementally a given collectionsource using a defined predicate
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IncrementalFilterService<T> : IFilterService<T>
    {
        private ObservableCollection<T> _collectionSource;

        public ObservableCollection<T> CollectionSource
        {
            get => _collectionSource;
            set
            {
                if (_collectionSource == value) return;
                FilteredCollection.Clear();

                // Unregister old source
                if (_collectionSource != null)
                    _collectionSource.CollectionChanged -= CollectionSourceChangedEvent;
                _collectionSource = value;
                // Register new source
                _collectionSource.CollectionChanged += CollectionSourceChangedEvent;
                var collectionChangedEventArgs = new NotifyCollectionChangedEventArgs(
                                                 NotifyCollectionChangedAction.Add, _collectionSource);
                CollectionSourceChangedEvent(this, collectionChangedEventArgs);
            }
        }
        public ObservableCollection<T> FilteredCollection { get; private set; }
                                       = new ObservableCollection<T>();
        public Predicate<T> FilterFunction { get; set; } = (x) => true;

        public IncrementalFilterService() { }
        public IncrementalFilterService(Predicate<T> filterFunction)
        {
            FilterFunction = filterFunction;
        }

        public void Refresh()
        {
            FilteredCollection.Clear();
            foreach (var item in CollectionSource)
            {
                if (FilterFunction(item))
                    FilteredCollection.Add(item);
            }
        }

        /// <summary>
        /// Event to monitor all changes made to collection and filter accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectionSourceChangedEvent(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                FilteredCollection.Clear();
                return;
            }
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var newItems = e.NewItems;
                foreach (var newItem in newItems)
                {
                    var addedItem = (T)newItem;
                    if (FilterFunction((addedItem)))
                        FilteredCollection.Add(addedItem);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var oldItem in e.OldItems)
                {
                    var removedItem = (T)oldItem;
                    if (FilteredCollection.Contains(removedItem))
                        FilteredCollection.Remove(removedItem);
                }
            }
        }

    }

    public interface IFilterService<T>
    {
        ObservableCollection<T> FilteredCollection { get; }
        ObservableCollection<T> CollectionSource { get; set; }
        Predicate<T> FilterFunction { get; set; }
        void Refresh();
    }
}
