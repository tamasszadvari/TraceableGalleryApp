using Android.App;
using Android.Content.PM;
using Android.OS;
using DLToolkit.Forms.Controls;
using FFImageLoading.Forms.Droid;
using XLabs.Forms;

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
            FlowListView.Init();
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new App());
        }
    }
}

