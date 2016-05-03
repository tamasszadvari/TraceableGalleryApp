using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TraceableGalleryApp.Interfaces;
using TraceableGalleryApp.Views.Pages;
using Xamarin.Forms;
using XLabs.Forms.Mvvm;

namespace TraceableGalleryApp.ViewModels
{
    [ViewType(typeof(ImageGalleryPage))]
    public class GalleryViewModel : BaseViewModel
    {
        readonly IStorageHandler _storageHandler;
        readonly IPictureDatabase _pictureDatabase;
        readonly IEnvironmentInfo _environmentInfo;

        ObservableCollection<ImageCellData> _images;

        public GalleryViewModel(IStorageHandler storageHandler, IPictureDatabase pictureDatabase, IEnvironmentInfo environmentInfo)
        {
            _storageHandler = storageHandler;
            _pictureDatabase = pictureDatabase;
            _environmentInfo = environmentInfo;

            AddImagesCommand = new Command(() => AddImages());
            RemoveImagesCommand = new Command(() => RemoveImages());
            ChangeImageSourceCommand = new Command(ChangeImageSource);

            ChangeImageSource();

            Task.Factory.StartNew(async () => {
                var files = await _storageHandler.GetFilesAsync();
                foreach (var file in files)
                {
                    var cellData = new ImageCellData();
                    cellData.ImageSource = ImageSource.FromFile(file);

                    Images.Add(cellData);
                }
            });
        }

        /// <summary>
        /// Command to add images to the list
        /// </summary>
        /// <value>The add images command.</value>
        public Command AddImagesCommand { get; private set; }

        /// <summary>
        /// Command to remove images from the list
        /// </summary>
        /// <value>The remove images command.</value>
        public Command RemoveImagesCommand { get; private set; }

        /// <summary>
        /// Command to swap the image source to a new object
        /// </summary>
        /// <value>The change image source command.</value>
        public Command ChangeImageSourceCommand { get; private set; }

        public Task AddImages()
        {
            return Task.Run(() => {
            });
        }

        public Task RemoveImages()
        {
            return Task.Run(() => {
            });
        }

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
    }
}

