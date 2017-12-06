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
            if (beleg.Image == null)
            {
                return;
            }

            try
            {
                beleg.Belegnummer = await BelegService.CreateBeleg(BelegService.USER, beleg);
            }
            catch (Exception ex)
            {
                //TODO what?
                return;
            }

            if (!beleg.Belegnummer.HasValue)
            {
                throw new Exception("Vermisse Beleg-ID!");
            }

            if (beleg.Image != null)
            {
                try
                {
                    beleg.Thumbnail = await BelegService.SaveBelegImage(BelegService.USER, beleg.Belegnummer.Value, beleg.Image);
                }
                catch (Exception ex)
                {
                    //TODO what?
                }
            }
            
            beleg.Status = Beleg.StatusEnum.EXPORTIERT;
            await database.StoreBeleg(beleg);
        }

        public async Task<int> RefreshStatus()
        {
            Task<Beleg[]> backend = BelegService.GetBelegList(BelegService.USER);
            Task<Beleg[]> local = Storage.Database.GetBelege();

            return await DoRefreshStatus(await backend, await local);
        }

        internal async Task<int> DoRefreshStatus(Beleg[] backend, Beleg[] local)
        {
            IDictionary<int, Beleg> locals = new Dictionary<int, Beleg>();
            foreach (Beleg beleg in local)
            {
                locals.Add(beleg.Belegnummer.Value, beleg);
            }
            List<Beleg> updates = new List<Beleg>();
            foreach (Beleg beleg in backend)
            {
                int nr = beleg.Belegnummer.Value;
                if (beleg.Status < Beleg.StatusEnum.GEBUCHT || locals.ContainsKey(nr))
                {
                    Beleg loc;
                    if (locals.TryGetValue(nr, out loc) && beleg.BelegSize == loc.BelegSize)
                    {
                        beleg.Image = loc.Image;
                    }
                    else
                    {
                        beleg.Image = await BelegService.GetBelegImage(BelegService.USER, nr);
                    }
                    if (loc != null)
                    {
                        locals.Remove(nr);
                    }
                    updates.Add(beleg);
                }
            }
            List<Beleg> deletes = new List<Beleg>();
            foreach (Beleg beleg in locals.Values)
            {
                if (beleg.Status > Beleg.StatusEnum.ERFASST)
                {
                    deletes.Add(beleg);
                }
            }
            await database.RemoveBelege(deletes.ToArray());
            await database.StoreBelege(updates.ToArray());
            return updates.Count + deletes.Count;
        }
    }
}
