using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TraceableGalleryApp.Interfaces
{
    public interface IStorageHandler
    {
        Task<List<string>> GetFilesAsync();
        Task<Stream> GetImageSourceAsync(string filePath);
        Task DeleteFileAsync(string filePath);
        Task<string> SaveAsync(string filePath);
    }
}

