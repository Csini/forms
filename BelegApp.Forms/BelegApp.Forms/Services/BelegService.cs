using BelegApp.Forms.Models;
using BelegApp.Forms.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BelegApp.Forms.Services
{
    static class BelegService
    {
        // Fest kodierte URL
        private static Uri serviceBaseUrl = new Uri("http://52.169.65.115:8080/belegerfassung-ui/rest/belege");

        public const string USER = "forms";

        public static async Task<string[]> GetTypeList()
        {
            string[] result = await WebRequester.HttpGet<string[]>(
                serviceBaseUrl,
                "/types");
            return result;
        }

        public static async Task<Beleg.StatusEnum[]> GetStatusList()
        {
            Beleg.StatusEnum[] result = await WebRequester.HttpGet<Beleg.StatusEnum[]>(
                serviceBaseUrl,
                "/status");
            return result;
        }
        
        public static async Task<Beleg[]> GetBelegList(string user)
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

        public static async Task<Beleg> GetBeleg(string user, int belegnummer)
        {
            // Parameter prüfen
            if (string.IsNullOrWhiteSpace(user))
            {
                throw new ArgumentNullException(nameof(user));
            }

            Beleg result = await WebRequester.HttpGet<Beleg>(
                serviceBaseUrl,
                string.Format("/{0}/{1}", user, belegnummer));
            return result;
        }

        public static async Task<int> CreateBeleg(string user, Beleg beleg)
        {
            // Parameter prüfen
            if (string.IsNullOrWhiteSpace(user))
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (beleg == null)
            {
                throw new ArgumentNullException(nameof(beleg));
            }

            int result = await WebRequester.HttpPost<Beleg, int>(
                serviceBaseUrl,
                string.Format("/{0}", user),
                beleg);
            return result;
        }

        public static async Task SaveBeleg(string user, Beleg beleg)
        {
            // Parameter prüfen
            if (string.IsNullOrWhiteSpace(user))
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (beleg == null)
            {
                throw new ArgumentNullException(nameof(beleg));
            }

            await WebRequester.HttpPut<Beleg, object>(
                serviceBaseUrl,
                string.Format("/{0}/{1}", user, beleg.Belegnummer),
                beleg);
        }

        public static async Task<byte[]> GetBelegImage(string user, int belegnummer)
        {
            // Parameter prüfen
            if (string.IsNullOrWhiteSpace(user))
            {
                throw new ArgumentNullException(nameof(user));
            }

            byte[] result = await WebRequester.HttpGet<byte[]>(
                serviceBaseUrl,
                string.Format("/{0}/{1}/beleg", user, belegnummer));
            return result;
        }

        public static async Task<byte[]> SaveBelegImage(string user, int belegnummer, byte[] image)
        {
            // Parameter prüfen
            if (string.IsNullOrWhiteSpace(user))
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            byte[] result = await WebRequester.HttpPut<byte[], byte[]>(
                serviceBaseUrl,
                string.Format("/{0}/{1}/beleg", user, belegnummer),
                image);
            return result;
        }

        public static async Task<byte[]> CreateThumbnail(byte[] image)
        {
            // Parameter prüfen
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            byte[] result = await WebRequester.HttpPost<byte[], byte[]>(
                serviceBaseUrl,
                "/thumbnail",
                image);
            return result;
        }

        public static async Task DeleteBeleg(string user, int belegnummer)
        {
            // Parameter prüfen
            if (string.IsNullOrWhiteSpace(user))
            {
                throw new ArgumentNullException(nameof(user));
            }

            await WebRequester.HttpDelete(
                serviceBaseUrl,
                string.Format("/{0}/{1}", user, belegnummer));
        }
    }
}
