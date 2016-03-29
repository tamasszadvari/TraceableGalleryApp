using TraceableGalleryApp.Database;
using TraceableGalleryApp.Interfaces;
using Xamarin.Forms;

namespace TraceableGalleryApp.Utilities
{
    public class PlatformUtilities
    {
        private IEnvironmentInfo _environmentInfo;

        private static volatile PlatformUtilities _instance;
        private static readonly AsyncLock MLock = new AsyncLock();

        public static PlatformUtilities Instance
        {
            get 
            {
                if (_instance == null)
                {
                    using (MLock.Lock ())
                    {
                        _instance = new PlatformUtilities();
                    }
                }
                return _instance;
            }
        }

        public void SetupAppDependecies()
        {
            _environmentInfo = DependencyService.Get<IEnvironmentInfo> ();
        }

        #region Public properties
        public IEnvironmentInfo EnvironmentInfo
        {
            get { return _environmentInfo ?? (_environmentInfo = DependencyService.Get<IEnvironmentInfo> ()); }
        }
        #endregion
    }
}
