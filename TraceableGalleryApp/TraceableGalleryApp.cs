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
            Init ();
            MainPage = GetMainPage ();
        }

        /// <summary>
        /// Initializes the application.
        /// </summary>
        private void Init()
        {
            var app = Resolver.Resolve<IXFormsApp>();
            if (app == null)
            {
                return;
            }

            RegisterViews();

            app.Closing += (o, e) => Debug.WriteLine("Application Closing");
            app.Error += (o, e) => Debug.WriteLine("Application Error");
            app.Initialize += (o, e) => Debug.WriteLine("Application Initialized");
            app.Resumed += (o, e) => Debug.WriteLine("Application Resumed");
            app.Rotation += (o, e) => Debug.WriteLine("Application Rotated");
            app.Startup += (o, e) => Debug.WriteLine("Application Startup");
            app.Suspended += (o, e) => Debug.WriteLine("Application Suspended");
        }

        /// <summary>
        /// Gets the main page.
        /// </summary>
        /// <returns>The Main Page.</returns>
        public static Page GetMainPage()
        {
            var mainPage = ViewFactory.CreatePage<CameraViewModel, Page>() as Page;
            var navigation = new NavigationPage(mainPage);
            navigation.BarBackgroundColor = AppColors.Primary;
            navigation.BarTextColor = AppColors.TextAndBackground;

            return navigation;
        }

        /// <summary>
        /// Registers the views with the viewmodels
        /// </summary>
        private void RegisterViews()
        {
            ViewFactory.Register<CameraPage, CameraViewModel>();
            ViewFactory.Register<ImageGalleryPage, GalleryViewModel>();
        }
    }
}
