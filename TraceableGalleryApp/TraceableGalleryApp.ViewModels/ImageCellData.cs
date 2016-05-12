using System.Collections.Generic;
using TraceableGalleryApp.Interfaces;
using Xamarin.Forms;
using XLabs.Forms.Mvvm;

namespace TraceableGalleryApp.ViewModels
{
    public class ImageCellData : ViewModel, IImageCellData
    {
        ImageSource _imageSource;
        List<string> _labels;
        double _longitude;
        double _latitude;

        /// <summary>
        /// Gets or sets the image source.
        /// </summary>
        /// <value>The image source.</value>
        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set { SetProperty(ref _imageSource, value); }
        }

        public List<string> Labels
        {
            get { return _labels; }
            set { SetProperty(ref _labels, value); }
        }

        public double Longitude
        {
            get { return _longitude; }
            set { SetProperty(ref _longitude, value); }
        }

        public double Latitude
        {
            get { return _latitude; }
            set { SetProperty(ref _latitude, value); }
        }
    }
}

