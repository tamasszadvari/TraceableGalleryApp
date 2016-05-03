using System.Threading.Tasks;
using System.Collections.Generic;

namespace TraceableGalleryApp.Interfaces
{
    public interface IPictureDatabase
    {
        Task<int> AddRow(IDbPictureData pictureData);
        Task<int> DeleteRow(int id);
        Task<IDbPictureData> GetById (int id);
        Task<IDbPictureData> GetByPath (string path);
        Task<IList<IDbPictureData>> GetByAnyLabel (IList<string> labels);
        Task<IList<IDbPictureData>> GetByAllLabel (IList<string> labels);
        Task<IDbPictureData> GetByPosition (double x, double y);
        Task<IList<IDbPictureData>> GetByCenterAndRadius (double x, double y, double radius);
        Task<IList<IDbPictureData>> GetByRectangle (double bottomLeftX, double bottomLeftY, double topRightX, double topRightY);
        Task<int> UpdateValue(IDbPictureData pictureData);
    }
}

