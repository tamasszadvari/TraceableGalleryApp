using System;
using System.IO;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Platform.XamarinAndroid;
using TraceableGalleryApp.Droid;
using TraceableGalleryApp.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(SQLite_Android))]
namespace TraceableGalleryApp.Droid
{
    public class SQLite_Android : ISQLite
    {
        #region ISQLite implementation

        public SQLiteAsyncConnection GetConnection ()
        {
            const string sqliteFilename = "traceable_gallery_app_db.db3";
            string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);

            // Create the connection
            var connectionWithLock = new SQLiteConnectionWithLock(
                new SQLitePlatformAndroid(), 
                new SQLiteConnectionString(path, false));

            // Return the database connection
            var connection = new SQLiteAsyncConnection (() => connectionWithLock);

            return connection;
        }

        #endregion
    }
}

