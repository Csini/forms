using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BelegApp.Forms.Backend
{
    public class Beleg : IEquatable<Beleg>
    {
        [PrimaryKey]
        public int? Belegnummer { get; set; }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            return this.Equals(obj as Beleg);
        }

        /// <summary>
        /// Returns true if Beleg instances are equal
        /// </summary>
        /// <param name="other">Instance of Beleg to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Beleg other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return
                (
                    this.Belegnummer == other.Belegnummer ||
                    this.Belegnummer != null &&
                    this.Belegnummer.Equals(other.Belegnummer)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            // credit: http://stackoverflow.com/a/263416/677735
            unchecked // Overflow is fine, just wrap
            {
                int hash = 41;
                // Suitable nullity checks etc, of course :)
                if (this.Belegnummer != null)
                    hash = hash * 59 + this.Belegnummer.GetHashCode();
                return hash;
            }
        }
    }

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
    }
}
