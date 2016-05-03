using System;
using Autofac;
using TraceableGalleryApp.Database;
using TraceableGalleryApp.Droid;
using TraceableGalleryApp.Interfaces;
using TraceableGalleryApp.ViewModels;
using TraceableGalleryApp.Views.Pages;
using Xamarin.Forms;
using XLabs.Forms.Services;
using XLabs.Platform.Device;
using XLabs.Platform.Services;
using XLabs.Platform.Services.Email;
using XLabs.Platform.Services.Media;

namespace TraceableGalleryApp
{
    public class TraceableGalleryAppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterViewModels(builder);
            RegisterViews(builder);
            RegisterServices(builder);

            //current page resolver
            builder.RegisterInstance<Func<Page>>(() => ((NavigationPage)Application.Current.MainPage).CurrentPage);
        }

        void RegisterViewModels(ContainerBuilder builder)
        {
            builder.RegisterType<CameraViewModel> ().SingleInstance();
            builder.RegisterType<GalleryViewModel> ().SingleInstance();
        }

        void RegisterViews(ContainerBuilder builder)
        {
            builder.RegisterType<CameraPage>().SingleInstance();
            builder.RegisterType<ImageGalleryPage>().SingleInstance();
        }

        void RegisterServices(ContainerBuilder builder)
        {
            builder.Register<IDevice>(t => AndroidDevice.CurrentDevice);
            builder.Register<IDisplay>(t => t.Resolve<IDevice>().Display);
            builder.Register<IFontManager>(t => new FontManager(t.Resolve<IDisplay>()));
            builder.RegisterType<EmailService>().As<IEmailService>().SingleInstance();
            builder.RegisterType<MediaPicker>().As<IMediaPicker>().SingleInstance();
            builder.RegisterType<StorageHandler>().As<IStorageHandler>().SingleInstance();
            builder.RegisterType<SQLite_Android>().As<ISQLite>().SingleInstance();
            builder.RegisterType<EmailService>().As<IEmailService>().SingleInstance();
            builder.RegisterType<PictureDatabase>().As<IPictureDatabase>().SingleInstance();
            builder.Register<ISecureStorage>(t => new KeyVaultStorage(t.Resolve<IDevice>().Id.ToCharArray()));
            builder.Register<IEnvironmentInfo>(t => DependencyService.Get<IEnvironmentInfo>()).SingleInstance();
        }
    }
}

