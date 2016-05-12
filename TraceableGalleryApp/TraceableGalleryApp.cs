using Xamarin.Forms;

namespace TraceableGalleryApp
{
    /// <summary>
    /// Class App.
    /// </summary>
    public class App : Application
    {
        public App ()
        {
            var bootstrapper = new Bootstrapper (this);
            bootstrapper.Run ();
        }
    }
}
