using OchUploader.Model;
using System.Collections.Generic;

namespace OchUploader.Service
{
    public delegate void UploadStateChangedEventHandler(object source, UploadState oldState);
    public class StateManagerService<T> where T: IStateProvider
    {
        private readonly List<T> _stateList = new List<T>();

        public StateManagerService(IEnumerable<T> stateCollection)
        {
            foreach (var state in stateCollection)
                Add(state);
        }

        public void Add(T stateProvider)
        {
            stateProvider.UploadStateChangedEvent += StateProvider_UploadStateChangedEvent;
            _stateList.Add(stateProvider);
        }

        private void StateProvider_UploadStateChangedEvent(object source, UploadState oldState)
        {

        }
    }

    public interface IStateProvider
    {
        UploadState State { get; set; }
        event UploadStateChangedEventHandler UploadStateChangedEvent;

    }
}
