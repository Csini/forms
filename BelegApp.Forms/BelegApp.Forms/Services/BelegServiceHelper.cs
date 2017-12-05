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

        public Task<int> ExportBelege(Beleg[] belege)
        {
            throw new NotImplementedException();
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
