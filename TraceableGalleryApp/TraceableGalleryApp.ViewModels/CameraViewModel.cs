using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TraceableGalleryApp.Database.Models;
using TraceableGalleryApp.Interfaces;
using TraceableGalleryApp.Views.Pages;
using Xamarin.Forms;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Geolocation;
using XLabs.Platform.Services.Media;

namespace TraceableGalleryApp.ViewModels
{
    /// <summary>
    /// Class CameraViewModel.
    /// </summary>
    [ViewType(typeof(CameraPage))]
    public class CameraViewModel : BaseViewModel
    {
        readonly IStorageHandler _storageHandler;
        readonly IPictureDatabase _pictureDatabase;

        /// <summary>
        /// The _scheduler.
        /// </summary>
        readonly TaskScheduler _scheduler = TaskScheduler.FromCurrentSynchronizationContext();

        /// <summary>
        /// The picture chooser.
        /// </summary>
        IMediaPicker _mediaPicker;

        /// <summary>
        /// The image source.
        /// </summary>
        ImageSource _imageSource;

        /// <summary>
        /// The take picture command.
        /// </summary>
        Command _takePictureCommand;

        /// <summary>
        /// The select picture command.
        /// </summary>
        Command _selectPictureCommand;

        /// <summary>
        /// The list pictures command.
        /// </summary>
        Command _listPicturesCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraViewModel" /> class.
        /// </summary>
        public CameraViewModel(INavigator navigator, IStorageHandler storageHandler, IPictureDatabase pictureDatabase)
        {
            Navigator = navigator;
            _storageHandler = storageHandler;
            _pictureDatabase = pictureDatabase;

            Setup();
        }

        /// <summary>
        /// Gets or sets the image source.
        /// </summary>
        /// <value>The image source.</value>
        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set { SetObservableProperty(ref _imageSource, value); }
        }

        /// <summary>
        /// Gets the take picture command.
        /// </summary>
        /// <value>The take picture command.</value>
        public Command TakePictureCommand 
        {
            get { return _takePictureCommand ?? (_takePictureCommand = new Command(async () => await TakePicture(), () => true)); }
        }

        public Command ListPicturesCommand
        {
            get { return _listPicturesCommand ?? (_listPicturesCommand = new Command(async () => await ListPictures(), () => true)); }
        }

        /// <summary>
        /// Gets the select picture command.
        /// </summary>
        /// <value>The select picture command.</value>
        public Command SelectPictureCommand 
        {
            get { return _selectPictureCommand ?? (_selectPictureCommand = new Command(async () => await SelectPicture(), () => true)); }
        }
            
        /// <summary>
        /// Setups this instance.
        /// </summary>
        void Setup()
        {
            if (_mediaPicker != null)
            {
                return;
            }

            var device = Resolver.Resolve<IDevice>();

            //RM: hack for working on windows phone? 
            _mediaPicker = DependencyService.Get<IMediaPicker>() ?? device.MediaPicker;
        }

        /// <summary>
        /// Takes the picture.
        /// </summary>
        /// <returns>Take Picture Task.</returns>
        async Task<MediaFile> TakePicture()
        {
            Setup();

            ImageSource = null;

            return await _mediaPicker
                .TakePhotoAsync(new CameraMediaStorageOptions { DefaultCamera = CameraDevice.Front, MaxPixelDimension = 400 })
                .ContinueWith(t => {
                    if (t.IsCompleted && !t.IsCanceled && !t.IsFaulted)
                    {
                        var geolocator = Resolver.Resolve<IGeolocator>();
                        var mediaFile = t.Result;

                        var locationTask = Task.Run(async () =>  {
                            return await geolocator.GetPositionAsync(60000);
                        });

                        locationTask.Wait();

                        if (locationTask.Result != null)
                        {
                            var position = locationTask.Result;
                            var x = position.Longitude;
                            var y = position.Latitude;

                            var newPath = Task.Run(async () => await _storageHandler.SaveAsync(mediaFile.Path));
                            newPath.Wait();
                            _pictureDatabase.AddRow(new DbPictureData {
                                Path = newPath.Result,
                                XPosition = x,
                                YPosition = y
                            });

                            return mediaFile;
                        } 
                    }

                    return null;
                }, _scheduler);
        }

        /// <summary>
        /// Selects the picture.
        /// </summary>
        /// <returns>Select Picture Task.</returns>
        async Task SelectPicture()
        {
            Setup();

            ImageSource = null;
            try
            {
                var mediaFile = await _mediaPicker.SelectPhotoAsync(new CameraMediaStorageOptions {
                    DefaultCamera = CameraDevice.Front,
                    MaxPixelDimension = 400
                });
                
                ImageSource = ImageSource.FromStream(() => mediaFile.Source);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        async Task ListPictures()
        {
            try {
                await Navigator.PushAsync<GalleryViewModel>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }
    }
}

