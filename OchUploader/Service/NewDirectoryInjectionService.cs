using OchUploader.Api;
using OchUploader.Infrastructure;
using OchUploader.Infrastructure.Factory;
using System.Collections.Generic;
using System.Linq;

namespace OchUploader.Service
{
    public enum InjectionReason
    {
        Directory_New,
        Directory_Down
    }
    public delegate void ChangedPathDetectedEventHandler(string path, string providerName, InjectionReason reason, string host = null);

    /// <summary>
    /// Service which checks a given directory for newly added directories 
    /// </summary>
    public class NewDirectoryInjectionService : IDirectoryInjectorService
    {
        /// <summary>
        /// DEPRECATED
        /// </summary>
        private readonly IAccessProtocollIdentifier _identifier;
        private readonly IDictionary<string, IDirectoryInfoProvider> _baseDirectoryByInfoProvider = 
            new Dictionary<string, IDirectoryInfoProvider>();
        private readonly IDirectoryInfoProviderFactory _factory;

        public event ChangedPathDetectedEventHandler ChangedPathDetectedEvent;
        public int IntervallInMinutes { get; set; } = 5;
        public InjectionReason Reason => InjectionReason.Directory_New;

        public NewDirectoryInjectionService(IAccessProtocollIdentifier identifier, IDirectoryInfoProviderFactory factory)
        {
            _identifier = identifier;
            _factory = factory;
        }

        public void AddBaseDirectory(string path, string providerName)
        {
            IDirectoryInfoProvider infoProvider = null;
            if (_baseDirectoryByInfoProvider.Values.Any(x => x.ProviderName == providerName))
                infoProvider = _baseDirectoryByInfoProvider.Values.First(x => x.ProviderName == providerName);
            else
                infoProvider = _factory.CreateInfoProvider(path, providerName);

            _baseDirectoryByInfoProvider.Add(path, infoProvider);
        }

        public void CheckNewDirectories()
        {
            while(ChangedPathDetectedEvent == null) { }
            foreach (var keyValuePair in _baseDirectoryByInfoProvider)
            {
                var baseDirectory = keyValuePair.Key;
                var infoProvider = keyValuePair.Value;
                var newDirectories = infoProvider.EnumerateDirectories(baseDirectory);
                if (!newDirectories.Any()) 
                    continue;
                if (ChangedPathDetectedEvent != null)
                    foreach (var newDirectory in newDirectories)
                        ChangedPathDetectedEvent(newDirectory, infoProvider.ProviderName, Reason);

            }
        }
    }

    public interface IDirectoryInjectorService
    {
        event ChangedPathDetectedEventHandler ChangedPathDetectedEvent;
        InjectionReason Reason { get; }
        void AddBaseDirectory(string path, string providerName);
    }
}
