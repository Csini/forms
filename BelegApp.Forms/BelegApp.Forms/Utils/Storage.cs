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

        public static Storage Database
        {
            get
            {
                if (storage == null)
                {
                    storage = new Storage();
                }
                return storage;
            }
        }

        private readonly SQLiteAsyncConnection database;

        public Storage() : this(new DependencyServiceWrapper())
        {
        }

        public Storage(IDependencyService dependencyService)
        {
            string dbPath = dependencyService.Get<IFileHelper>().GetLocalFilePath("BelegSQLite.db3");
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Beleg>().Wait();
        }

        public Task<Beleg[]> GetBelege()
        {
            return database.Table<Beleg>().ToListAsync().ContinueWith((lt) => lt.Result.ToArray());
        }

        public async Task StoreBelege(Beleg[] belege)
        {
            List<Task<int>> storeTasks = new List<Task<int>>();
            foreach (Beleg beleg in belege)
            {
                storeTasks.Add(StoreBeleg(beleg));
            }
            await Task.WhenAll(storeTasks);
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
