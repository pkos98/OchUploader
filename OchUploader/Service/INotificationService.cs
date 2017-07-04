using System;
using System.Collections.Generic;
using System.Text;

namespace OchUploader.Service
{
    public interface INotificationService
    {
        void Info(string message);
        void Success(string message);
        void Warning(string message);
        void Error(string message);
    }
}
