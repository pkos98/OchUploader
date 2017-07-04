using FluentScheduler;
using OchUploader.Service;
using System.Windows;

namespace OchUploader.FrontEnd.WPF
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        private readonly NewDirectoryInjectionService _injectionService = (NewDirectoryInjectionService)IocContainer.Get<IDirectoryInjectorService>();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            InitializeJobs();
        }

        private void InitializeJobs()
        {
            Registry _jobRegistry = new Registry();
            _jobRegistry.Schedule(() => _injectionService.CheckNewDirectories()).ToRunNow().DelayFor(5).Seconds();
            JobManager.Initialize(_jobRegistry);
        }
    }
}
