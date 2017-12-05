using BelegApp.Forms.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BelegApp.Forms.Utils
{
     public interface IFileHelper
    {
        string GetLocalFilePath(string filename);
    }

    public class Storage
    {
        static int idGenerator = -1;

        static Storage storage;

        public Storage Database
        {
            get
            {
                if (storage == null)
                {
                    storage = new Storage(DependencyService.Get<IFileHelper>().GetLocalFilePath("BelegSQLite.db3"));
                }
                return storage;
            }
        }

        SQLiteAsyncConnection database;

        public Storage(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Beleg>().Wait();
        }

        public Task<List<Beleg>> GetBelege()
        {
            return database.Table<Beleg>().ToListAsync();
        }

        public Task<int> StoreBeleg(Beleg beleg)
        {
            if (beleg.Belegnummer == null || !beleg.Belegnummer.HasValue)
            {
                beleg.Belegnummer = idGenerator--;
            }
            return database.InsertOrReplaceAsync(beleg);
        }

        public Task<int> RemoveBeleg(Beleg beleg)
        {
            return database.DeleteAsync(beleg);
        }

        public Boolean isNew(Beleg beleg)
        {
            return beleg.Belegnummer == null || beleg.Belegnummer.Value < 0;
        }

        public Task<Beleg> GetBeleg(int belegnummer)
        {
            return database.Table<Beleg>().Where(beleg => beleg.Belegnummer == belegnummer).FirstAsync();
        }
    }
}
