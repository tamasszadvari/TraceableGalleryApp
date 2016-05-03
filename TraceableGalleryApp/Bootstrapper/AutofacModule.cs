using Autofac;
using TraceableGalleryApp.Interfaces;
using TraceableGalleryApp.Utilities;
using Xamarin.Forms;

namespace TraceableGalleryApp
{
    public class AutofacModule : Module
    {
        protected override void Load (ContainerBuilder builder)
        {
            // service registration
            builder.RegisterType<ViewFactory>()
                .As<IViewFactory>()
                .SingleInstance();

            builder.RegisterType<Navigator>()
                .As<INavigator>()
                .SingleInstance();

            builder.Register<INavigation>(context => App.Current.MainPage.Navigation).SingleInstance();
        }
    }
}

