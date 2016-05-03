using System.Diagnostics;
using TraceableGalleryApp.Utilities;
using TraceableGalleryApp.ViewModels;
using TraceableGalleryApp.Views.Pages;
using Xamarin.Forms;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;
using XLabs.Platform.Mvvm;

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
