using Android.App;
using Android.Content.PM;
using Android.OS;
using FFImageLoading.Forms.Droid;
using XLabs.Forms;
using XLabs.Forms.Services;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Mvvm;
using XLabs.Platform.Services;
using XLabs.Platform.Services.Email;
using XLabs.Platform.Services.Media;
using TraceableGalleryApp.Database;
using TraceableGalleryApp.Interfaces;
using Xamarin.Forms;

namespace TraceableGalleryApp.Droid
{
    [Activity(Label = "TraceableGalleryApp.Droid", 
        Icon = "@drawable/icon",
        MainLauncher = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        Theme = "@style/CustomTheme")]
    public class MainActivity : XFormsApplicationDroid
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            CachedImageRenderer.Init();
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new App());
        }

        /// <summary>
        /// Sets the IoC.
        /// </summary>
        void SetIoc()
        {
            var resolverContainer = new SimpleContainer();
            var app = new XFormsAppDroid();

            app.Init(this);

            resolverContainer.Register<IDevice>(t => AndroidDevice.CurrentDevice)
                .Register<IDisplay>(t => t.Resolve<IDevice>().Display)
                .Register<IFontManager>(t => new FontManager(t.Resolve<IDisplay>()))
                .Register<IEmailService, EmailService>()
                .Register<IMediaPicker, MediaPicker>()
                .Register<IDependencyContainer>(resolverContainer)
                .Register<IXFormsApp>(app)
                .Register<ISecureStorage>(t => new KeyVaultStorage(t.Resolve<IDevice>().Id.ToCharArray()))
                .Register<IEnvironmentInfo>(DependencyService.Get<IEnvironmentInfo>())
                .RegisterSingle<IStorageHandler, StorageHandler>()
                .RegisterSingle<ISQLite, SQLite_Android>()
                .RegisterSingle<IPictureDatabase, PictureDatabase>();

            Resolver.SetResolver(resolverContainer.GetResolver());
        }
    }
}

