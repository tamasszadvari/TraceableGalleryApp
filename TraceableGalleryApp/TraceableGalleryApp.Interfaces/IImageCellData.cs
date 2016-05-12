using System.Collections.Generic;
using Xamarin.Forms;

namespace TraceableGalleryApp.Interfaces
{
    public interface IImageCellData
    {
        ImageSource ImageSource { get; set; }
        List<string> Labels { get; set; }
        double Longitude { get; set; }
        double Latitude { get; set; }
    }
}

