using BelegApp.Forms.Models;
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
            throw new NotImplementedException();
        }
    }
}
