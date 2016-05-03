using System;

namespace TraceableGalleryApp.Interfaces
{
    public interface IDbPictureData
    {
        int Id { get; set; }
        string Path { get; set; }
        string[] Labels { get; set; }
        double XPosition { get; set; }
        double YPosition { get; set; }
    }
}

