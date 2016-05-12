using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TraceableGalleryApp.Interfaces;
using TraceableGalleryApp.Views.Pages;
using Xamarin.Forms;
using XLabs.Forms.Mvvm;
using System.Collections.Generic;

namespace TraceableGalleryApp.ViewModels
{
    [ViewType(typeof(ImageGalleryPage))]
    public class GalleryViewModel : BaseViewModel, IGalleryViewModel
    {
        readonly INavigator _navigator;
        readonly IStorageHandler _storageHandler;
        readonly IPictureDatabase _pictureDatabase;
        readonly IEnvironmentInfo _environmentInfo;
        readonly IJsonHelper _jsonHelper;

        ObservableCollection<ImageCellData> _images;
        string _searchText;
        Command _searchByLabelCommand;
        Command<IImageCellData> _openImageCommand;

        public GalleryViewModel(INavigator navigator, IStorageHandler storageHandler, IPictureDatabase pictureDatabase, 
            IEnvironmentInfo environmentInfo, IJsonHelper jsonHelper)
        {
            _navigator = navigator;
            _storageHandler = storageHandler;
            _pictureDatabase = pictureDatabase;
            _environmentInfo = environmentInfo;
            _jsonHelper = jsonHelper;

            ChangeImageSourceCommand = new Command(ChangeImageSource);

            ChangeImageSource();

            Task.Factory.StartNew(LoadImages);
        }

        async void LoadImages()
        {
            var files = await _storageHandler.GetFilesAsync();
            foreach (var file in files)
            {
                var dbObject = await _pictureDatabase.GetByPath(file);

                if (dbObject != null)
                {
                    var cellData = MakeImageCell(dbObject);
                    if (cellData != null)
                        Images.Add(cellData);   
                }
            }
        }
            

        /// <summary>
        /// Command to swap the image source to a new object
        /// </summary>
        /// <value>The change image source command.</value>
        public Command ChangeImageSourceCommand { get; private set; }

        public void ChangeImageSource()
        {
            Images = new ObservableCollection<ImageCellData>();
        }

        /// <summary>
        /// Gets or sets the images.
        /// </summary>
        /// <value>
        /// The images.
        /// </value>
        public ObservableCollection<ImageCellData> Images 
        {
            get { return _images; }
            set { SetObservableProperty (ref _images, value); }
        }

        /// <summary>
        /// Gets the width of the image accordin to the screen's width.
        /// </summary>
        /// <value>The width of the image.</value>
        public double ImageWidth
        {
            get 
            {
                var isPortrait = _environmentInfo.ScreenHeight > _environmentInfo.ScreenWidth;
                var scale = isPortrait ? 2 : 4;
                var divider = isPortrait ? 3 : 6;

                return _environmentInfo.ScreenWidth/scale-divider; 
            }
        }

        public Command SearchByLabelCommand
        {
            get 
            { 
                return _searchByLabelCommand ?? (_searchByLabelCommand = new Command(async () => {
                    var exists = await _pictureDatabase.IsLabelExists(SearchText);

                    if (!exists) {
                        await Application.Current.MainPage.DisplayAlert("Ooops", "The given label is not exists","Cancel");
                    }
                    else {
                        var pictures = await _pictureDatabase.GetByAnyLabel(new List<string> {SearchText});

                        var newList = new ObservableCollection<ImageCellData>();
                        foreach (var pic in pictures)
                        {
                            var cell = MakeImageCell(pic);
                            if (cell != null)
                                newList.Add(cell);
                        }

                        Images = newList;
                    }
                })); 
            }
        }

        public Command<IImageCellData> OpenImageCommand
        {
            get
            {
                return _openImageCommand ?? (_openImageCommand = new Command<IImageCellData>(async item =>
                    await _navigator.PushAsync<PictureViewModel>(vm => {
                        vm.ImgSource = item.ImageSource;
                        vm.LongitudeText = "Longitude: " + item.Longitude;
                        vm.LatitudeText = "Latitude: " + item.Latitude;
                        vm.Labels = item.Labels;
                    })
                ));
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set { SetObservableProperty (ref _searchText, value); }
        }

        ImageCellData MakeImageCell(IDbPictureData data)
        {
            if (data != null)
            {
                var source = string.IsNullOrEmpty(data.Path)
                    ? null
                    : ImageSource.FromFile(data?.Path);
                var labels = string.IsNullOrEmpty(data.Labels)
                    ? new List<string>()
                    : _jsonHelper.Deserialize<List<string>>(data.Labels);

                var cellData = new ImageCellData {
                    ImageSource = source,
                    Longitude = data.XPosition,
                    Latitude = data.YPosition,
                    Labels = labels
                };

                return cellData;
            }

            return null;
        }
    }
}

