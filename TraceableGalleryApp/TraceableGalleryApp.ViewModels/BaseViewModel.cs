using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TraceableGalleryApp.Interfaces;

namespace TraceableGalleryApp.ViewModels
{
    public class BaseViewModel : IViewModel, INotifyPropertyChanged
    {
        INavigator _navigator;
        bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public INavigator Navigator
        {
            get { return _navigator; }
            set
            {
                _navigator = value;
                OnPropertyChanged();
            }
        }

        public void SetState<T>(Action<T> action) where T : class, IViewModel
        {
            action(this as T);
        }

        // this little bit is how we trigger the PropertyChanged notifier.
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler (this, new PropertyChangedEventArgs (propertyName));
        }

        protected void SetObservableProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) 
                return;

            field = value;
            OnPropertyChanged (propertyName);
        }
    }
}

