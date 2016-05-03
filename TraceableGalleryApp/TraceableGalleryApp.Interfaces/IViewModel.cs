using System;

namespace TraceableGalleryApp.Interfaces
{
    public interface IViewModel
    {
        void SetState<T>(Action<T> action) where T : class, IViewModel;
        bool IsBusy { get; set; }

        INavigator Navigator { get; set; }
    }
}

