using BelegApp.Forms.Models;
using BelegApp.Forms.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BelegApp.Forms.Services
{
    class BelegService
    {
        // Fest kodierte URL
        private Uri serviceBaseUrl = new Uri("http://52.169.65.115:8080/belegerfassung-ui/rest/belege");

        public async Task<string[]> GetTypeList()
        {
            string[] result = await WebRequester.HttpGet<string[]>(
                serviceBaseUrl,
                "/types");
            return result;
        }

        public async Task<Beleg.StatusEnum[]> GetStatusList()
        {
            Beleg.StatusEnum[] result = await WebRequester.HttpGet<Beleg.StatusEnum[]>(
                serviceBaseUrl,
                "/status");
            return result;
        }
        
        public async Task<Beleg[]> GetBelegList(string user)
        {
            // Parameter prüfen
            if (string.IsNullOrWhiteSpace(user))
            {
                throw new ArgumentNullException(nameof(user));
            }

            Beleg[] result = await WebRequester.HttpGet<Beleg[]>(
                serviceBaseUrl,
                string.Format("/{0}", user));
            return result;
        }

        public Beleg GetBeleg(string user, int belegnummer)
        {
            throw new NotImplementedException();
        }

        public int CreateBeleg(string user, Beleg beleg)
        {
            throw new NotImplementedException();
        }

        public void SaveBeleg(string user, Beleg beleg)
        {
            throw new NotImplementedException();
        }

        public byte[] GetBelegImage(string user, int belegnummer)
        {
            throw new NotImplementedException();
        }

        public void SaveGelegImage(string user, int belegnummer, byte[] image)
        {
            throw new NotImplementedException();
        }

        public void DeleteBeleg(string user, int belegnummer)
        {
            throw new NotImplementedException();
        }
    }
}
