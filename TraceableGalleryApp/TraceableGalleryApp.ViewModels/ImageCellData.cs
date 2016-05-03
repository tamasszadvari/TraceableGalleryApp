using Xamarin.Forms;
using XLabs.Forms.Mvvm;

namespace TraceableGalleryApp.ViewModels
{
    public class ImageCellData : ViewModel
    {
        /// <summary>
        /// The image source.
        /// </summary>
        ImageSource _imageSource;

        /// <summary>
        /// Gets or sets the image source.
        /// </summary>
        /// <value>The image source.</value>
        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set { SetProperty(ref _imageSource, value); }
        }
    }
}

