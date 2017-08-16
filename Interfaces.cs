using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenLibrary
{
    public interface IRavenObject{
        string Id { get; set; }
    }

    public interface IRavenObjectDatabase{
        //non async
        bool SaveOneObjectToDB(IRavenObject item);
        bool SaveMultipleObjectsToDB(List<IRavenObject> listitems);
        IRavenObject LoadOneObjectFromDB(string itemId);
        List<IRavenObject> LoadMultipleObjectsFromDB(List<string> itemsId);
        List<IRavenObject> LoadMultipleObjectsFromDB();
        bool DeleteOneObjectFromDB(IRavenObject item);
        bool DeleteOneObjectFromDB(string itemId);
        bool DeleteMultipleObjectsFromDB(List<IRavenObject> listitems);
        bool DeleteMultipleObjectsFromDB(List<string> listitems);
        bool UpdateOneObject(IRavenObject item);
        bool UpdateMultipipleObjects(List<IRavenObject> listitem);

    }

    public interface IRavenObjectDatabaseAsync{
        //async 
        Task<bool> SaveOneObjectToDBAsync(IRavenObject item);
        Task<bool> SaveMultipleObjectsToDBAsync(List<IRavenObject> listitems);
        Task<IRavenObject> LoadOneObjectFromDBAsync(string itemId);
        Task<List<IRavenObject>> LoadMultipleObjectsFromDBAsync(List<string> itemsId);
        Task<List<IRavenObject>> LoadMultipleObjectsFromDBAsync();
        Task<bool> DeleteOneObjectFromDBAsync(IRavenObject item);
        Task<bool> DeleteOneObjectFromDBAsync(string itemId);
        Task<bool> DeleteMultipleObjectsFromDBAsync(List<IRavenObject> listitems);
        Task<bool> DeleteMultipleObjectsFromDBAsync(List<string> listitems);
        Task<bool> UpdateOneObjectAsync(IRavenObject item);
        Task<bool> UpdateMultipipleObjectsAsync(List<IRavenObject> listitem);
    }

}
