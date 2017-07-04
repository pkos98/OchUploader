using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OchUploader.Infrastructure
{
    public class Notifyer: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected PropertyChangedEventHandler GetEvent => PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
