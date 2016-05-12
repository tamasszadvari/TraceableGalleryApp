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

        void OnItemSelected (object sender, XLabs.EventArgs<object> e)
        {
            var ctx = BindingContext as IGalleryViewModel;
            var item = e.Value as IImageCellData;

            if (ctx == null || item == null)
                return;

            ctx.OpenImageCommand.Execute(item);
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

