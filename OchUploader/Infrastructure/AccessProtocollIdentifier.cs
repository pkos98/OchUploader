using OchUploader.Api;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OchUploader.Infrastructure
{
    /// <summary>
    /// Identifies the access protocoll of a given path
    /// </summary>
    public class AccessProtocollIdentifier : IAccessProtocollIdentifier
    {
        /// Regex patterns of the different protocolls
        private readonly IDictionary<AccessProtocoll, string> _patternByAccessProtocoll =
            new Dictionary<AccessProtocoll, string>()
            {
                { AccessProtocoll.Local, @"^(?:[a-zA-Z]\:(\\|\/)|file\:\/\/|\\\\|\.(\/|\\))([^\\\/\:\*\?\<\>\\|]+(\\|\/){0,1})+$" },
                { AccessProtocoll.Http_s, @"^(http(s)?://)(www\.)?\w+\.\w+" },
                { AccessProtocoll.Ftp, @"^(ftp://)\w+:\w+@\w+(:\d+)?(/\w+)*" }
            };

        /// <summary>
        /// Identifies the access protocoll of a given path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public AccessProtocoll IdentifyProtocoll(string path)
        {
            foreach (var keyValuePair in _patternByAccessProtocoll)
            {
                var protocoll = keyValuePair.Key;
                var pattern = keyValuePair.Value;
                var regex = new Regex(pattern);

                // If path matches pattern
                if (regex.IsMatch(path))
                    return protocoll;
            }
            throw new ArgumentException("The path's format is not recognized");
        }

    }
    public interface IAccessProtocollIdentifier
    {
        AccessProtocoll IdentifyProtocoll(string path);
    }
}