using Raven.Client.Document;
using Raven.Client.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RavenLibrary
{
    public abstract class RavenObject<T> : AbstractIndexCreationTask<T>, IRavenObject
    {
        public string Id { get; set; }
    }


    public abstract class RavenObjectsDatabase<T> : IRavenObjectDatabase
    {
        public readonly string CollectionName;
        public readonly string DbName;
        public string HostName { get; set; }
        public int Port { get; set; }
        private DocumentStore documentStore { get; set; }
        private Raven.Client.IDocumentSession DatabaseSession { get; set; }

        public RavenObjectsDatabase(string DBCollectionName, string dbname, string dbhostname, int dbport)
        {
            if (string.IsNullOrEmpty(DBCollectionName) || string.IsNullOrEmpty(dbhostname)
                 || dbport <= 0 || string.IsNullOrEmpty(dbname))
                throw new ArgumentException("Parameters are not correct, please check parameters");

            CollectionName = DBCollectionName;
            HostName = dbhostname;
            Port = dbport;
            DbName = dbname;
            try
            {
                documentStore = new DocumentStore { Url = string.Format("http://{0}:{1}", dbhostname, dbport) };
                documentStore.Initialize();
                DatabaseSession = documentStore.OpenSession(DbName);
            }
            catch (Exception ex) { throw ex; }
        }

        public bool DeleteMultipleObjectsFromDB(List<IRavenObject> listitems)
        {
            if (listitems != null && listitems.Count > 0)
            {
                try
                {
                    foreach (IRavenObject obj in listitems)
                    {
                        DatabaseSession.Delete(obj.Id);
                    }
                    DatabaseSession.SaveChanges();
                    return true;
                }
                catch (Exception ex) { throw ex; }
            }
            else
                throw new Exception("Update multiple objects failed. Check your parameters");
        }

        public bool DeleteMultipleObjectsFromDB(List<string> listitems)
        {
            throw new NotImplementedException();
        }

        public bool DeleteOneObjectFromDB(IRavenObject item)
        {
            if (item != null){
                try{
                    DatabaseSession.Delete(item.Id);
                    DatabaseSession.SaveChanges();
                    return true;
                }
                catch (Exception ex) { throw ex; }

            }
            else
                throw new ArgumentNullException("Item provided fopr deletion is NULL");
        }

        public bool DeleteOneObjectFromDB(string itemId)
        {
            try{
                IRavenObject item = DatabaseSession.Load<IRavenObject>(itemId);
                return DeleteOneObjectFromDB(item);
            }
            catch (Exception ex) { throw ex; }
        }

        public bool SaveMultipleObjectsToDB(List<IRavenObject> listitems)
        {
            if (listitems != null && listitems.Count > 0){
                try{
                    foreach (IRavenObject obj in listitems){
                        DatabaseSession.Store(obj);
                    }
                    DatabaseSession.SaveChanges();
                    return true;
                }
                catch (Exception ex) { throw ex; }
            }
            else
                throw new Exception("Update multiple objects failed. Check your parameters");
        }

        public bool SaveOneObjectToDB(IRavenObject item)
        {
            try{
                DatabaseSession.Store(item);
                DatabaseSession.SaveChanges();
                return true;
            }
            catch (Exception ex) { throw ex; }
        }

        public bool UpdateMultipipleObjects(List<IRavenObject> listitem)
        {
            if (listitem != null && listitem.Count > 0)
            {
                try
                {
                    foreach (IRavenObject obj in listitem)
                    {
                        DatabaseSession.Store(obj);
                    }
                    DatabaseSession.SaveChanges();
                    return true;
                }
                catch (Exception ex) { throw ex; }
            }
            else
                throw new Exception("Update multiple objects failed. Check your parameters");
        }

        public bool UpdateOneObject(IRavenObject item)
        {
            return SaveOneObjectToDB(item);
        }

        public IRavenObject LoadOneObjectFromDB(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
                throw new ArgumentException("ID provided is less than 0");
            try
            {
                IRavenObject item = DatabaseSession.Load<RavenObject<T>>(string.Format("{0}/{1}", CollectionName, itemId));
                return item;
            }
            catch (Exception ex) { throw ex; }
        }

        public List<IRavenObject> LoadMultipleObjectsFromDB(List<string> itemsId)
        {
            List<IRavenObject> resultlist = new List<IRavenObject>();
            if (itemsId != null && itemsId.Count > 0){
                try{
                    resultlist = DatabaseSession.Load<IRavenObject>(itemsId).ToList();
                    return resultlist;
                }
                catch (Exception ex) { throw ex; }

            }
            else
                throw new ArgumentException("Parameter provided for loading multiple object is not correct");
        }

        public List<IRavenObject> LoadMultipleObjectsFromDB()
        {
            List<IRavenObject> resultlist = new List<IRavenObject>();
            try
            {
                var conventions = documentStore.Conventions ?? new DocumentConvention();
                var defaultIndexStartsWith = conventions.GetTypeTagName(typeof(T));
                using (var enumerator = DatabaseSession.Advanced.Stream<T>(defaultIndexStartsWith))
                {
                    while (enumerator.MoveNext())
                    {
                        IRavenObject item = (IRavenObject)enumerator.Current.Document;
                        resultlist.Add((RavenObject<T>)item);
                    }
                }
                return resultlist;
            }
            catch (Exception ex) { throw ex; }
        }
    }

    public abstract class RavenObjectsDatabaseAsync<T> : IRavenObjectDatabaseAsync
    {

        public readonly string CollectionName;
        public readonly string DbName;
        public string HostName { get; set; }
        public int Port { get; set; }
        private DocumentStore documentStore { get; set; }
        private Raven.Client.IAsyncDocumentSession DatabaseSession { get; set; }

        public RavenObjectsDatabaseAsync(string DBCollectionName, string dbname, string dbhostname, int dbport)
        {
            if (string.IsNullOrEmpty(DBCollectionName) || string.IsNullOrEmpty(dbhostname)
                 || dbport <= 0 || string.IsNullOrEmpty(dbname))
                throw new ArgumentException("Parameters are not correct, please check parameters");

            CollectionName = DBCollectionName;
            HostName = dbhostname;
            Port = dbport;
            DbName = dbname;
            try
            {
                documentStore = new DocumentStore { Url = string.Format("http://{0}:{1}", dbhostname, dbport) };
                documentStore.Initialize();
                DatabaseSession = documentStore.OpenAsyncSession(DbName);
            }
            catch (Exception ex) { throw ex; }

        }

        public async Task<bool> DeleteMultipleObjectsFromDBAsync(List<IRavenObject> listitems)
        {
            if (listitems != null && listitems.Count > 0)
            {
                foreach (IRavenObject obj in listitems)
                {
                    DatabaseSession.Delete(obj.Id);
                }
                await DatabaseSession.SaveChangesAsync();
                return true;
            }
            else
                throw new Exception("Delete multiple objects failed. Check your parameters");
        }

        public async Task<bool> DeleteMultipleObjectsFromDBAsync(List<string> listitems)
        {
            if (listitems != null && listitems.Count > 0)
            {
                foreach(string id in listitems)
                {
                    DatabaseSession.Delete(id);
                }
                await DatabaseSession.SaveChangesAsync();
                return true;
            } else
                throw new Exception("Delete multiple objects failed. Check your parameters");
        }

        public async Task<bool> DeleteOneObjectFromDBAsync(IRavenObject item)
        {
            try{
                RavenObject<T> obj = await DatabaseSession.LoadAsync<RavenObject<T>>(item.Id);
                return await DeleteOneObjectFromDBAsync(item);
            }
            catch (Exception ex) { throw ex; }

        }

        public async Task<bool> DeleteOneObjectFromDBAsync(string itemId)
        {
           try{
                DatabaseSession.Delete(itemId);
                await DatabaseSession.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<IRavenObject>> LoadMultipleObjectsFromDBAsync(List<string> itemsId)
        {
            List<IRavenObject> result_list = new List<IRavenObject>();
            if (itemsId != null && itemsId.Count > 0)
            {
                try
                {
                    IRavenObject[] array = await DatabaseSession.LoadAsync<IRavenObject>(itemsId);
                    result_list = array.ToList();
                    return result_list;
                }
                catch (Exception ex) { throw ex; }

            }
            else
                throw new ArgumentException("Parameter provided for loading multiple object is not correct");

        }

        public async Task<List<IRavenObject>> LoadMultipleObjectsFromDBAsync()
        {
            List<IRavenObject> result_list = new List<IRavenObject>();
            try
            {
                var conventions = documentStore.Conventions ?? new DocumentConvention();
                var defaultIndexStartsWith = conventions.GetTypeTagName(typeof(T));
                using (var enumerator = await DatabaseSession.Advanced.StreamAsync<T>(defaultIndexStartsWith))
                {
                    while (await enumerator.MoveNextAsync()){
                        IRavenObject item = (IRavenObject) enumerator.Current.Document;
                        result_list.Add((RavenObject<T>)item);
                    }
                }
                return result_list;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<IRavenObject> LoadOneObjectFromDBAsync(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
                throw new ArgumentException("ID provided is less than 0");
            try
            {
                RavenObject<T> item = await DatabaseSession.LoadAsync<RavenObject<T>>(string.Format("{0}/{1}", CollectionName, itemId));
                return item;
            }
            catch (Exception ex) { throw ex; }

        }

        public async Task<bool> SaveMultipleObjectsToDBAsync(List<IRavenObject> listitems)
        {
            if (listitems != null && listitems.Count > 0){
                try{
                    foreach (IRavenObject obj in listitems){
                        await DatabaseSession.StoreAsync(obj);
                    }
                    await DatabaseSession.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex) { throw ex; }
            }
            else
                throw new Exception("Save multiple objects failed. Check your parameters");
        }

        public async Task<bool> SaveOneObjectToDBAsync(IRavenObject item)
        {
            try{
                await DatabaseSession.StoreAsync(item);
                await DatabaseSession.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<bool> UpdateMultipipleObjectsAsync(List<IRavenObject> listitem)
        {
            if (listitem != null && listitem.Count > 0)
            {
                foreach(IRavenObject obj in listitem)
                {
                    await DatabaseSession.StoreAsync(obj);
                }
                await DatabaseSession.SaveChangesAsync();
                return true;
            } else
                throw new Exception("Save multiple objects failed. Check your parameters");
        }

        public async Task<bool> UpdateOneObjectAsync(IRavenObject item)
        {
            return await SaveOneObjectToDBAsync(item);
        }
    }
}
