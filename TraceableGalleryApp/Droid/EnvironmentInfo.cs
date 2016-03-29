using Android.OS;
using Xamarin.Forms;
using TraceableGalleryApp.Droid;
using TraceableGalleryApp.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(EnvironmentInfo))]
namespace TraceableGalleryApp.Droid
{
    public class EnvironmentInfo : IEnvironmentInfo
    {
        public string DeviceName
        {
            get { return Manufacturer + " " + Model ;}
        }

        public string Manufacturer
        {
            get { return Build.Manufacturer; }
        }

        public string Model
        {
            get { return Build.Model; }
        }

        public string AppVersionName
        {
            get { return Forms.Context.PackageManager.GetPackageInfo(Forms.Context.PackageName, 0).VersionName; }
        }

        public string AppVersionCode
        {
            get { return Forms.Context.PackageManager.GetPackageInfo(Forms.Context.PackageName, 0).VersionCode.ToString(); }
        }

        public int ScreenWidth
        {
            get { return (int)(Forms.Context.Resources.DisplayMetrics.WidthPixels); }
        }

        public int ScreenHeight
        {
            get { return (int)(Forms.Context.Resources.DisplayMetrics.HeightPixels); }
        }
    }
}


