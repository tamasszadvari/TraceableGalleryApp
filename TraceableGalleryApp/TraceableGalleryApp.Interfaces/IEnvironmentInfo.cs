using System;

namespace TraceableGalleryApp.Interfaces
{
    public interface IEnvironmentInfo
    {
        string DeviceName { get; }

        string Manufacturer { get; }

        string Model { get; }

        string AppVersionName { get; }

        string AppVersionCode { get; }

        int ScreenWidth { get; }

        int ScreenHeight { get; }
    }
}
