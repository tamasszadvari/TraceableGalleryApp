using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TraceableGalleryApp.Interfaces;
using TraceableGalleryApp.Utilities;
using TraceableGalleryApp.Views.Pages;
using Xamarin.Forms;
using XLabs.Forms.Mvvm;

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

            Images = new ObservableCollection<ImageCellData>();

            MessagingCenter.Subscribe<CameraViewModel,string>(this, StringConstants.NewPictureTakenSysMessage, async (sender, filePath) => {
                var item = await _pictureDatabase.GetByPath(filePath);
                var cellData = MakeImageCell(item);
                if (cellData != null)
                    Images.Add(cellData); 
            });

            Task.Factory.StartNew(LoadImages);
        }

        async void LoadImages()
        {
            var dbItems = await _pictureDatabase.GetAll();
            foreach (var item in dbItems)
            {
                var cellData = MakeImageCell(item);
                if (cellData != null)
                    Images.Add(cellData); 
            }
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

        public Command SearchByLabelCommand
        {
            get 
            { 
                return _searchByLabelCommand ?? (_searchByLabelCommand = new Command(async () => {

                    IList<IDbPictureData> pictures;
                    if (!string.IsNullOrEmpty(SearchText))
                    {
                        var exists = await _pictureDatabase.IsLabelExists(SearchText);
                        if (!exists)
                        {
                            await Application.Current.MainPage.DisplayAlert("Ooops", "The given label is not exists","Cancel");
                            return;
                        }

                        pictures = await _pictureDatabase.GetByAnyLabel(new List<string> {SearchText});
                    }
                    else
                    {
                        pictures = await _pictureDatabase.GetAll();
                    }
                        
                    var newList = new ObservableCollection<ImageCellData>();
                    foreach (var pic in pictures)
                    {
                        var cell = MakeImageCell(pic);
                        if (cell != null)
                            newList.Add(cell);
                    }

                    Images = newList;
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

