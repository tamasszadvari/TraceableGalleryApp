using Autofac;
using TraceableGalleryApp.Interfaces;
using TraceableGalleryApp.Utilities;
using TraceableGalleryApp.ViewModels;
using TraceableGalleryApp.Views.Pages;
using Xamarin.Forms;

namespace TraceableGalleryApp
{
    public class Bootstrapper : AutofacBootstrapper
    {
        readonly App _application;

        public Bootstrapper(App application)
        {
            _application = application;           
        }

        protected override void RegisterModules(ContainerBuilder builder)
        {
            base.RegisterModules(builder);

            builder.RegisterModule<TraceableGalleryAppModule>();
        } 

        protected override void RegisterViews(IViewFactory viewFactory)
        {
            viewFactory.Register<CameraViewModel, CameraPage> ();
            viewFactory.Register<GalleryViewModel, ImageGalleryPage> ();
        }

        protected override void ConfigureApplication(IContainer container)
        {
            // set main page
            var viewFactory = container.Resolve<IViewFactory>();
            var mainPage = viewFactory.Resolve<CameraViewModel> ();

            var navigationPage = new NavigationPage(mainPage);

            NavigationPage.SetHasNavigationBar (navigationPage, false);
            navigationPage.BarBackgroundColor = AppColors.Primary;
            navigationPage.BarTextColor = AppColors.TextAndBackground;

            _application.MainPage = navigationPage;
        }
    }
}

