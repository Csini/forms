using BelegApp.Forms.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BelegApp.Forms.Utils;

namespace BelegApp.Forms.Services
{
    public class BelegServiceHelper
    {

        public Task<int> ExportBelege(Beleg[] belege)
        {
            if (belege != null)
            {
                foreach (Beleg beleg in belege)
                {
                    

                }
            }
            throw new NotImplementedException();
        }

        public Task<int> RefreshStatus()
        {
            throw new NotImplementedException();
        }
    }
}
