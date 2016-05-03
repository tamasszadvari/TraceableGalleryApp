using SQLite.Net.Async;

namespace TraceableGalleryApp.Interfaces
{
    public interface ISQLite
    {
        SQLiteAsyncConnection GetConnection();
    }
}

