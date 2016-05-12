using System;
using Xamarin.Forms;

namespace TraceableGalleryApp.Interfaces
{
    public interface IGalleryViewModel
    {
        Command<IImageCellData> OpenImageCommand { get; }
    }
}

