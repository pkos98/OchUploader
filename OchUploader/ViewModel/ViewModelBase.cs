using OchUploader.Infrastructure;
using System;
using System.Reflection;

namespace OchUploader.ViewModel
{
    /// <summary>
    /// Each ViewModel implements INotifyPropertyChanged, so that it can notify subscribers
    /// </summary>
    public class ViewModelBase : Notifyer
    {
        public delegate void CommandMethod(object obj);
        protected string _lastAction;

        public string LastAction
        {
            get { return _lastAction; }
            set
            {
                if (value == _lastAction)
                    return;

                _lastAction = value;
                RaisePropertyChanged();
            }
        }

        protected void ExecuteAndSetLastAction(Action<object> command)
        {
            var attribute = (LastActionAttribute)command.GetInvocationList()[0].
                            GetMethodInfo().GetCustomAttribute(typeof(LastActionAttribute));
            if (attribute != null)
                LastAction = attribute.Description;
            // Execute the function assigned to the command-property
            command(null);
        }

    }
}
