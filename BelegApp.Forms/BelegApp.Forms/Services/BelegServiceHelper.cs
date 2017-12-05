using BelegApp.Forms.Models;
using BelegApp.Forms.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BelegApp.Forms.Services
{
    public class BelegServiceHelper
    {

        public static async Task ExportBelege(Beleg[] belege)
        {
            if (belege != null)
            {
                List<Beleg> belegList = new List<Beleg>(belege);
                IEnumerable<Task> ExportTasksQuery = from beleg in belegList select ExportBeleg(beleg);

                // Use ToArray to execute the query and start the download tasks.  
                Task<int>[] downloadTasks = downloadTasksQuery.ToArray();
                var tasks = new List<Task>();
                foreach (Beleg beleg in belege)
                {
                    tasks.Add(ExportBeleg(beleg));
                }
                
            }
        }

        static async Task ExportBeleg(Beleg beleg)
        {
            beleg.Belegnummer = await BelegService.CreateBeleg("forms", beleg);

            if (!beleg.Belegnummer.HasValue)
            {
                throw new Exception("Vermisse Beleg-ID!");
            }

            await BelegService.SaveBelegImage("forms", beleg.Belegnummer.Value, beleg.Image);
        }

        public Task<int> RefreshStatus()
        {
            return Task.WhenAll(BelegService.GetBelegList(BelegService.USER), Storage.Database.GetBelege()).ContinueWith((r) => DoRefreshStatus(r.Result[0], r.Result[1]));
        }

        private int DoRefreshStatus(Beleg[] backend, Beleg[] local)
        {
            IDictionary<int, Beleg> locals = new Dictionary<int, Beleg>();
            foreach (Beleg beleg in local)
            {
                locals.Add(beleg.Belegnummer.Value, beleg);
            }
            List<Task<int>> updates = new List<Task<int>>();
            foreach (Beleg beleg in backend)
            {
                Beleg loc = locals[beleg.Belegnummer.Value];
                loc.Status = beleg.Status;
                updates.Add(Storage.Database.StoreBeleg(loc));
            }
            return Task.WhenAll(updates.ToArray()).ContinueWith((r) => r.Result.Length).Result;
        }
    }
}
