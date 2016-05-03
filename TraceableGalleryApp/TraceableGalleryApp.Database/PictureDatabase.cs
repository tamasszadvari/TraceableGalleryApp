using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite.Net.Async;
using TraceableGalleryApp.Database.Models;
using TraceableGalleryApp.Interfaces;

namespace TraceableGalleryApp.Database
{
    /// <summary>
    /// Database of the stored pictures
    /// </summary>
    public class PictureDatabase : IPictureDatabase
    {
        readonly SQLiteAsyncConnection database;

        public PictureDatabase (ISQLite db)
        {
            database = db.GetConnection ();
            database.CreateTableAsync<DbPictureData> ();
        }   

        #region Row methods
        public async Task<int> AddRow(IDbPictureData pictureData)
        {
            return await database.InsertOrReplaceAsync (pictureData).ConfigureAwait (false);
        }

        public async Task<int> DeleteRow(int id)
        {
            var row = await database.Table<DbPictureData> ()
                .Where (x => x.Id == id)
                .FirstOrDefaultAsync ()
                .ConfigureAwait (false);
            return await database.DeleteAsync (row).ConfigureAwait (false);
        }

        public async Task<IDbPictureData> GetById (int id) 
        {
            return await database.Table<DbPictureData> ()
                .Where (x => x.Id == id)
                .FirstOrDefaultAsync ()
                .ConfigureAwait (false);
        }

        public async Task<IDbPictureData> GetByPath (string path) 
        {
            return await database.Table<DbPictureData> ()
                .Where (x => x.Path == path)
                .FirstOrDefaultAsync ()
                .ConfigureAwait (false);
        }

        public async Task<IList<IDbPictureData>> GetByAnyLabel (IList<string> labels) 
        {
            // TODO: has to deserialize the x.Labels -> it has to be a JSON array of strings
            var list = await database.Table<DbPictureData> ()
                .Where (x => labels.Contains(x.Labels))
                .ToListAsync()
                .ConfigureAwait (false);

            return list.ToList<IDbPictureData>();
        }

        public async Task<IList<IDbPictureData>> GetByAllLabel (IList<string> labels) 
        {
            // TODO: has to deserialize the x.Labels -> it has to be a JSON array of strings

            var list = await database.Table<DbPictureData> ()
                .Where (x => labels.Contains(x.Labels))
                .ToListAsync()
                .ConfigureAwait (false);

            return list.ToList<IDbPictureData>();
        }

        public async Task<IDbPictureData> GetByPosition (double x, double y) 
        {
            return await database.Table<DbPictureData> ()
                .Where (e => e.XPosition == x && e.YPosition == y)
                .FirstOrDefaultAsync ()
                .ConfigureAwait (false);
        }

        public async Task<IList<IDbPictureData>> GetByCenterAndRadius (double x, double y, double radius) 
        {
            var list = await database.Table<DbPictureData> ()
                .Where (e => IsInRadius(e.XPosition, e.YPosition, x, y, radius))
                .ToListAsync ()
                .ConfigureAwait (false);

            return list.ToList<IDbPictureData>();
        }

        public async Task<IList<IDbPictureData>> GetByRectangle (double bottomLeftX, double bottomLeftY, double topRightX, double topRightY) 
        {
            var list = await database.Table<DbPictureData> ()
                .Where (e => IsInRectangle(e.XPosition, e.YPosition, bottomLeftX, bottomLeftY, topRightX, topRightY))
                .ToListAsync ()
                .ConfigureAwait (false);

            return list.ToList<IDbPictureData>();
        }

        public async Task<int> UpdateValue(IDbPictureData pictureData)
        {
            return await database.InsertOrReplaceAsync (pictureData).ConfigureAwait (false);
        }
        #endregion

        static bool IsInRadius(double imgX, double imgY, double locX, double locY, double radius)
        {
            return (imgX > locX - radius && imgX < locX + radius) && (imgY > locY - radius && imgY < locY + radius);
        }

        static bool IsInRectangle(double imgX, double imgY, 
            double bottomLeftX, double bottomLeftY, double topRightX, double topRightY)
        {
            return (imgX >= bottomLeftX && imgX <= topRightX) && (imgY >= bottomLeftY && imgY <= topRightY);
        }
    }
}

