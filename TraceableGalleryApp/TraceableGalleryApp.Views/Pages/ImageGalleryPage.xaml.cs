using TraceableGalleryApp.Interfaces;
using TraceableGalleryApp.Utilities;
using Xamarin.Forms;
using XLabs.Enums;

namespace TraceableGalleryApp.Views.Pages
{
    public partial class ImageGalleryPage : ContentPage
    {
        readonly IEnvironmentInfo _environmentInfo;
        Orientation previousOrientation;

        public ImageGalleryPage(IEnvironmentInfo environmentInfo)
        {
            _environmentInfo = environmentInfo;
            previousOrientation = _environmentInfo.ScreenHeight > _environmentInfo.ScreenWidth
                ? Orientation.Portrait
                : Orientation.Landscape;

            BackgroundColor = AppColors.TextAndBackground;

            InitializeComponent();
        }
            
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            var orientation = height > width
                ? Orientation.Portrait
                : Orientation.Landscape;

            if (orientation != previousOrientation)
            {
                previousOrientation = orientation;
                InitializeComponent();
            }
        }
    }
}

