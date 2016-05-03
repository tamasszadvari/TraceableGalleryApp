using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using PCLStorage;
using TraceableGalleryApp.Interfaces;

namespace TraceableGalleryApp.Database
{
    public class StorageHandler : IStorageHandler
    {
        #region IStorageHandler implementation
        public async Task<string> SaveAsync(string filePath)
        {
            var file = await FileSystem.Current.GetFileFromPathAsync(filePath);
            var newPath = FileSystem.Current.LocalStorage.Path + filePath.Substring(filePath.LastIndexOf('/'));
            if (file != null)
                await file.MoveAsync(newPath);

            var deletable = await FileSystem.Current.GetFileFromPathAsync(filePath);
            if (deletable != null)
                await deletable.DeleteAsync();

            return newPath;
        }

        public async Task<List<string>> GetFilesAsync()
        {
            var folder = await FileSystem.Current.GetFolderFromPathAsync(FileSystem.Current.LocalStorage.Path);
            IList<IFile> files = new List<IFile>();
            if (folder != null)
                files = await folder.GetFilesAsync();

            var filePathList = new List<string>();
            foreach (var file in files)
            {
                filePathList.Add(file.Path);
            }

            return filePathList;
        }

        public async Task<Stream> GetImageSourceAsync(string filePath)
        {
            var file = await FileSystem.Current.GetFileFromPathAsync(filePath);
            return file != null
                ? await file.OpenAsync(FileAccess.Read)
                : null;
        }

        public async Task DeleteFileAsync(string filePath)
        {
            var file = await FileSystem.Current.GetFileFromPathAsync(filePath);
            await file.DeleteAsync();
        }
        #endregion
    }
}

