using BelegApp.Forms.Models;
using BelegApp.Forms.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("BelegApp.Forms.Tests")]
namespace BelegApp.Forms.Services
{
    public class BelegServiceHelper
    {

        private readonly Storage database;

        public BelegServiceHelper() : this(new Storage())
        {

        }

        public BelegServiceHelper(Storage database)
        {
            this.database = database;
        }

        public async Task ExportBelege(Beleg[] belege)
        {
            if (belege != null)
            {
                var tasks = new List<Task>();
                foreach (Beleg beleg in belege)
                {
                    tasks.Add(ExportBeleg(beleg));
                }
                await Task.WhenAll(tasks);
            }
        }

        internal async Task ExportBeleg(Beleg beleg)
        {
            beleg.Belegnummer = await BelegService.CreateBeleg(BelegService.USER, beleg);

            if (!beleg.Belegnummer.HasValue)
            {
                throw new Exception("Vermisse Beleg-ID!");
            }

            await BelegService.SaveBelegImage(BelegService.USER, beleg.Belegnummer.Value, beleg.Image);

            beleg.Status = Beleg.StatusEnum.EXPORTIERT;
            await database.StoreBeleg(beleg);
        }

        public Task<int> RefreshStatus()
        {
            return Task.WhenAll(BelegService.GetBelegList(BelegService.USER), Storage.Database.GetBelege()).ContinueWith((r) => DoRefreshStatus(r.Result[0], r.Result[1]));
        }

        internal int DoRefreshStatus(Beleg[] backend, Beleg[] local)
        {
            IDictionary<int, Beleg> locals = new Dictionary<int, Beleg>();
            foreach (Beleg beleg in local)
            {
                locals.Add(beleg.Belegnummer.Value, beleg);
            }
            List<Task<int>> updates = new List<Task<int>>();
            foreach (Beleg beleg in backend)
            {
                int nr = beleg.Belegnummer.Value;
                if (locals.ContainsKey(nr))
                {
                    Beleg loc = locals[nr];
                    loc.Status = beleg.Status;
                    updates.Add(database.StoreBeleg(loc));
                }
            }
            return Task.WhenAll(updates.ToArray()).ContinueWith((r) => r.Result.Length).Result;
        }
    }
}
