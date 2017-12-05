using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BelegApp.Forms.Utils
{
	/// <summary>
	/// Diese Klasse stellt Funktionen zur Verfügung, um die Serviceaufrufe einfach durchführen zu können.
	/// </summary>
	static class WebRequester
	{
		#region CreateQueryItemDictionary
		/// <summary>
		/// Erzeugt ein Dictionary zur Übergabe von QueryItems an die Funktionen <see cref="HttpGet"/> und <see cref="HttpPost"/>.
		/// </summary>
		/// <returns>Dictionary zur Übergabe von QueryItems.</returns>
		public static IDictionary<string, string> CreateQueryItemDictionary()
		{
			// Dictionary erzeugen und initialisieren
			IDictionary<string, string> result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			return result;
		} // public static IDictionary<string, string> CreateQueryItemDictionary()
		#endregion
		#region HttpGet
		/// <summary>
		/// Führt den Serviceaufruf durch.
		/// </summary>
		/// <typeparam name="T">Typ, der als Ergebnis des Services erwartet wird.</typeparam>
		/// <param name="baseUrl">[in] Basis-URL des Services</param>
		/// <param name="action">[in] Name der Serviceaktion</param>
		/// <exception cref="System.ArgumentNullException">
		/// Wird ausgelöst, wenn der Parameter <paramref name="baseUrl"/> <i>null</i> oder leer ist.
		/// Wird ausgelöst, wenn der Parameter <paramref name="action"/> <i>null</i> oder leer ist.
		/// </exception>
		/// <returns>Ergebnis des Serviceaufrufes.</returns>
		public async static Task<T> HttpGet<T>(
			string baseUrl,
			string action)
		{
			// Service aufrufen
			T result = await HttpGet<T>(baseUrl, action, null);
			return result;
        } // public static Task<T> HttpGet<T>( ...

        /// <summary>
        /// Führt den Serviceaufruf durch.
        /// </summary>
        /// <typeparam name="T">Typ, der als Ergebnis des Services erwartet wird.</typeparam>
        /// <param name="baseUrl">[in] Basis-URL des Services</param>
        /// <param name="action">[in] Name der Serviceaktion</param>
        /// <param name="queryItems">[in] Liste der zusätzlichen Aufrufparameter</param>
        /// <exception cref="System.ArgumentNullException">
        /// Wird ausgelöst, wenn der Parameter <paramref name="baseUrl"/> <i>null</i> oder leer ist.
        /// Wird ausgelöst, wenn der Parameter <paramref name="action"/> <i>null</i> oder leer ist.
        /// </exception>
        /// <returns>Ergebnis des Serviceaufrufes.</returns>
        public async static Task<T> HttpGet<T>(
			string baseUrl,
			string action,
			IDictionary<string, string> queryItems)
		{
			// Parameter prüfen
			if (string.IsNullOrWhiteSpace(baseUrl))
			{
				throw new ArgumentNullException(nameof(baseUrl));
			} // if (string.IsNullOrWhiteSpace(baseUrl))

			// URL in ein Uri-Objekt umwandeln
			Uri uri = new Uri(baseUrl);

			// Service aufrufen
			T result = await HttpGet<T>(uri, action, queryItems);
			return result;
        } // public async static Task<T> HttpGet<T>( ...

        /// <summary>
        /// Führt den Serviceaufruf durch.
        /// </summary>
        /// <typeparam name="T">Typ, der als Ergebnis des Services erwartet wird.</typeparam>
        /// <param name="baseUrl">[in] Basis-URL des Services</param>
        /// <param name="action">[in] Name der Serviceaktion</param>
        /// <exception cref="System.ArgumentNullException">
        /// Wird ausgelöst, wenn der Parameter <paramref name="baseUrl"/> <i>null</i> ist.
        /// Wird ausgelöst, wenn der Parameter <paramref name="action"/> <i>null</i> oder leer ist.
        /// </exception>
        /// <returns>Ergebnis des Serviceaufrufes.</returns>
        public async static Task<T> HttpGet<T>(
			Uri baseUrl,
			string action)
		{
			// Service aufrufen
			T result = await HttpGet<T>(baseUrl, action, null);
			return result;
        } // public async static Task<T> HttpGet<T>( ...

        /// <summary>
        /// Führt den Serviceaufruf durch.
        /// </summary>
        /// <typeparam name="T">Typ, der als Ergebnis des Services erwartet wird.</typeparam>
        /// <param name="baseUrl">[in] Basis-URL des Services</param>
        /// <param name="action">[in] Name der Serviceaktion</param>
        /// <param name="queryItems">[in] Liste der zusätzlichen Aufrufparameter</param>
        /// <exception cref="System.ArgumentNullException">
        /// Wird ausgelöst, wenn der Parameter <paramref name="baseUrl"/> <i>null</i> ist.
        /// Wird ausgelöst, wenn der Parameter <paramref name="action"/> <i>null</i> oder leer ist.
        /// </exception>
        /// <returns>Ergebnis des Serviceaufrufes.</returns>
        public async static Task<T> HttpGet<T>(
			Uri baseUrl,
			string action,
			IDictionary<string, string> queryItems)
		{
			// Parameter prüfen
			if (null == baseUrl)
			{
				throw new ArgumentNullException(nameof(baseUrl));
			} // if (null == baseUrl)
			if (string.IsNullOrEmpty(action))
			{
				throw new ArgumentNullException(nameof(action));
			} // if (string.IsNullOrEmpty(action))

			// Service aufrufen
			T result = await httpGet<T>(baseUrl, action, queryItems);
			return result;
        } // public async static Task<T> HttpGet<T>( ...
        #endregion
        #region HttpPost
        /// <summary>
        /// Führt den Serviceaufruf durch wobei Daten mitgegeben werden können.
        /// </summary>
        /// <typeparam name="TRequest">Typ, der als Daten mitgegeben wird.</typeparam>
        /// <typeparam name="TResponse">Typ, der als Ergebnis des Services erwartet wird.</typeparam>
        /// <param name="baseUrl">[in] Basis-URL des Services</param>
        /// <param name="action">[in] Name der Serviceaktion</param>
        /// <param name="payload">[in] Zusatzdaten, die beim Aufruf mitgegeben werden sollen</param>
        /// <exception cref="System.ArgumentNullException">
        /// Wird ausgelöst, wenn der Parameter <paramref name="baseUrl"/> <i>null</i> oder leer ist.
        /// Wird ausgelöst, wenn der Parameter <paramref name="action"/> <i>null</i> oder leer ist.
        /// </exception>
        /// <returns>Ergebnis des Serviceaufrufes.</returns>
        public async static Task<TResponse> HttpPost<TRequest, TResponse>(
			string baseUrl,
			string action,
			TRequest payload)
		{
			// Request ausführen
			TResponse result = await HttpPost<TRequest, TResponse>(baseUrl, action, null, payload);
			return result;
        } // public async static Task<TResponse> HttpPost<TRequest, TResponse>( ...

        /// <summary>
        /// Führt den Serviceaufruf durch wobei Daten mitgegeben werden können.
        /// </summary>
        /// <typeparam name="TRequest">Typ, der als Daten mitgegeben wird.</typeparam>
        /// <typeparam name="TResponse">Typ, der als Ergebnis des Services erwartet wird.</typeparam>
        /// <param name="baseUrl">[in] Basis-URL des Services</param>
        /// <param name="action">[in] Name der Serviceaktion</param>
        /// <param name="queryItems">[in] Liste der zusätzlichen Aufrufparameter</param>
        /// <param name="payload">[in] Zusatzdaten, die beim Aufruf mitgegeben werden sollen</param>
        /// <exception cref="System.ArgumentNullException">
        /// Wird ausgelöst, wenn der Parameter <paramref name="baseUrl"/> <i>null</i> oder leer ist.
        /// Wird ausgelöst, wenn der Parameter <paramref name="action"/> <i>null</i> oder leer ist.
        /// </exception>
        /// <returns>Ergebnis des Serviceaufrufes.</returns>
        public async static Task<TResponse> HttpPost<TRequest, TResponse>(
			string baseUrl,
			string action,
			IDictionary<string, string> queryItems,
			TRequest payload)
		{
			// Parameter prüfen
			if (string.IsNullOrWhiteSpace(baseUrl))
			{
				throw new ArgumentNullException(nameof(baseUrl));
			} // if (string.IsNullOrWhiteSpace(baseUrl))

			// URL in ein Uri-Objekt umwandeln
			Uri uri = new Uri(baseUrl);

			// Request ausführen
			TResponse result = await HttpPost<TRequest, TResponse>(uri, action, queryItems, payload);
			return result;
        } // public async static Task<TResponse> HttpPost<TRequest, TResponse>( ...

        /// <summary>
        /// Führt den Serviceaufruf durch wobei Daten mitgegeben werden können.
        /// </summary>
        /// <typeparam name="TRequest">Typ, der als Daten mitgegeben wird.</typeparam>
        /// <typeparam name="TResponse">Typ, der als Ergebnis des Services erwartet wird.</typeparam>
        /// <param name="baseUrl">[in] Basis-URL des Services</param>
        /// <param name="action">[in] Name der Serviceaktion</param>
        /// <param name="payload">[in] Zusatzdaten, die beim Aufruf mitgegeben werden sollen</param>
        /// <exception cref="System.ArgumentNullException">
        /// Wird ausgelöst, wenn der Parameter <paramref name="baseUrl"/> <i>null</i> ist.
        /// Wird ausgelöst, wenn der Parameter <paramref name="action"/> <i>null</i> oder leer ist.
        /// Wird ausgelöst, wenn der Parameter <paramref name="payload"/> <i>null</i> ist.
        /// </exception>
        /// <returns>Ergebnis des Serviceaufrufes.</returns>
        public async static Task<TResponse> HttpPost<TRequest, TResponse>(
			Uri baseUrl,
			string action,
			TRequest payload)
		{
			// Request ausführen
			TResponse result = await HttpPost<TRequest, TResponse>(baseUrl, action, null, payload);
			return result;
        } // public async static Task<TResponse> HttpPost<TRequest, TResponse>( ...

        /// <summary>
        /// Führt den Serviceaufruf durch wobei Daten mitgegeben werden können.
        /// </summary>
        /// <typeparam name="TRequest">Typ, der als Daten mitgegeben wird.</typeparam>
        /// <typeparam name="TResponse">Typ, der als Ergebnis des Services erwartet wird.</typeparam>
        /// <param name="baseUrl">[in] Basis-URL des Services</param>
        /// <param name="action">[in] Name der Serviceaktion</param>
        /// <param name="queryItems">[in] Liste der zusätzlichen Aufrufparameter</param>
        /// <param name="payload">[in] Zusatzdaten, die beim Aufruf mitgegeben werden sollen</param>
        /// <exception cref="System.ArgumentNullException">
        /// Wird ausgelöst, wenn der Parameter <paramref name="baseUrl"/> <i>null</i> ist.
        /// Wird ausgelöst, wenn der Parameter <paramref name="action"/> <i>null</i> oder leer ist.
        /// Wird ausgelöst, wenn der Parameter <paramref name="payload"/> <i>null</i> ist.
        /// </exception>
        /// <returns>Ergebnis des Serviceaufrufes.</returns>
        public async static Task<TResponse> HttpPost<TRequest, TResponse>(
			Uri baseUrl,
			string action,
			IDictionary<string, string> queryItems,
			TRequest payload)
		{
			// Parameter prüfen
			if (null == baseUrl)
			{
				throw new ArgumentNullException(nameof(baseUrl));
			} // if (null == baseUrl)
			if (string.IsNullOrEmpty(action))
			{
				throw new ArgumentNullException(nameof(action));
			} // if (string.IsNullOrEmpty(action))
			if (null == payload)
			{
				throw new ArgumentNullException(nameof(payload));
			} // if (null == payload)

			// Request ausführen
			TResponse result = await httpPost<TRequest, TResponse>(baseUrl, action, queryItems, payload);
			return result;
        } // public async static Task<TResponse> HttpPost<TRequest, TResponse>( ...
        #endregion
        #region HttpPut
        /// <summary>
        /// Führt den Serviceaufruf durch wobei Daten mitgegeben werden können.
        /// </summary>
        /// <typeparam name="TRequest">Typ, der als Daten mitgegeben wird.</typeparam>
        /// <typeparam name="TResponse">Typ, der als Ergebnis des Services erwartet wird.</typeparam>
        /// <param name="baseUrl">[in] Basis-URL des Services</param>
        /// <param name="action">[in] Name der Serviceaktion</param>
        /// <param name="payload">[in] Zusatzdaten, die beim Aufruf mitgegeben werden sollen</param>
        /// <exception cref="System.ArgumentNullException">
        /// Wird ausgelöst, wenn der Parameter <paramref name="baseUrl"/> <i>null</i> oder leer ist.
        /// Wird ausgelöst, wenn der Parameter <paramref name="action"/> <i>null</i> oder leer ist.
        /// </exception>
        /// <returns>Ergebnis des Serviceaufrufes.</returns>
        public async static Task<TResponse> HttpPut<TRequest, TResponse>(
            string baseUrl,
            string action,
            TRequest payload)
        {
            // Request ausführen
            TResponse result = await HttpPut<TRequest, TResponse>(baseUrl, action, null, payload);
            return result;
        } // public async static Task<TResponse> HttpPut<TRequest, TResponse>( ...

        /// <summary>
        /// Führt den Serviceaufruf durch wobei Daten mitgegeben werden können.
        /// </summary>
        /// <typeparam name="TRequest">Typ, der als Daten mitgegeben wird.</typeparam>
        /// <typeparam name="TResponse">Typ, der als Ergebnis des Services erwartet wird.</typeparam>
        /// <param name="baseUrl">[in] Basis-URL des Services</param>
        /// <param name="action">[in] Name der Serviceaktion</param>
        /// <param name="queryItems">[in] Liste der zusätzlichen Aufrufparameter</param>
        /// <param name="payload">[in] Zusatzdaten, die beim Aufruf mitgegeben werden sollen</param>
        /// <exception cref="System.ArgumentNullException">
        /// Wird ausgelöst, wenn der Parameter <paramref name="baseUrl"/> <i>null</i> oder leer ist.
        /// Wird ausgelöst, wenn der Parameter <paramref name="action"/> <i>null</i> oder leer ist.
        /// </exception>
        /// <returns>Ergebnis des Serviceaufrufes.</returns>
        public async static Task<TResponse> HttpPut<TRequest, TResponse>(
            string baseUrl,
            string action,
            IDictionary<string, string> queryItems,
            TRequest payload)
        {
            // Parameter prüfen
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentNullException(nameof(baseUrl));
            } // if (string.IsNullOrWhiteSpace(baseUrl))

            // URL in ein Uri-Objekt umwandeln
            Uri uri = new Uri(baseUrl);

            // Request ausführen
            TResponse result = await HttpPut<TRequest, TResponse>(uri, action, queryItems, payload);
            return result;
        } // public async static Task<TResponse> HttpPut<TRequest, TResponse>( ...

        /// <summary>
        /// Führt den Serviceaufruf durch wobei Daten mitgegeben werden können.
        /// </summary>
        /// <typeparam name="TRequest">Typ, der als Daten mitgegeben wird.</typeparam>
        /// <typeparam name="TResponse">Typ, der als Ergebnis des Services erwartet wird.</typeparam>
        /// <param name="baseUrl">[in] Basis-URL des Services</param>
        /// <param name="action">[in] Name der Serviceaktion</param>
        /// <param name="payload">[in] Zusatzdaten, die beim Aufruf mitgegeben werden sollen</param>
        /// <exception cref="System.ArgumentNullException">
        /// Wird ausgelöst, wenn der Parameter <paramref name="baseUrl"/> <i>null</i> ist.
        /// Wird ausgelöst, wenn der Parameter <paramref name="action"/> <i>null</i> oder leer ist.
        /// Wird ausgelöst, wenn der Parameter <paramref name="payload"/> <i>null</i> ist.
        /// </exception>
        /// <returns>Ergebnis des Serviceaufrufes.</returns>
        public async static Task<TResponse> HttpPut<TRequest, TResponse>(
            Uri baseUrl,
            string action,
            TRequest payload)
        {
            // Request ausführen
            TResponse result = await HttpPut<TRequest, TResponse>(baseUrl, action, null, payload);
            return result;
        } // public async static Task<TResponse> HttpPut<TRequest, TResponse>( ...

        /// <summary>
        /// Führt den Serviceaufruf durch wobei Daten mitgegeben werden können.
        /// </summary>
        /// <typeparam name="TRequest">Typ, der als Daten mitgegeben wird.</typeparam>
        /// <typeparam name="TResponse">Typ, der als Ergebnis des Services erwartet wird.</typeparam>
        /// <param name="baseUrl">[in] Basis-URL des Services</param>
        /// <param name="action">[in] Name der Serviceaktion</param>
        /// <param name="queryItems">[in] Liste der zusätzlichen Aufrufparameter</param>
        /// <param name="payload">[in] Zusatzdaten, die beim Aufruf mitgegeben werden sollen</param>
        /// <exception cref="System.ArgumentNullException">
        /// Wird ausgelöst, wenn der Parameter <paramref name="baseUrl"/> <i>null</i> ist.
        /// Wird ausgelöst, wenn der Parameter <paramref name="action"/> <i>null</i> oder leer ist.
        /// Wird ausgelöst, wenn der Parameter <paramref name="payload"/> <i>null</i> ist.
        /// </exception>
        /// <returns>Ergebnis des Serviceaufrufes.</returns>
        public async static Task<TResponse> HttpPut<TRequest, TResponse>(
            Uri baseUrl,
            string action,
            IDictionary<string, string> queryItems,
            TRequest payload)
        {
            // Parameter prüfen
            if (null == baseUrl)
            {
                throw new ArgumentNullException(nameof(baseUrl));
            } // if (null == baseUrl)
            if (string.IsNullOrEmpty(action))
            {
                throw new ArgumentNullException(nameof(action));
            } // if (string.IsNullOrEmpty(action))
            if (null == payload)
            {
                throw new ArgumentNullException(nameof(payload));
            } // if (null == payload)

            // Request ausführen
            TResponse result = await httpPut<TRequest, TResponse>(baseUrl, action, queryItems, payload);
            return result;
        } // public async static Task<TResponse> HttpPut<TRequest, TResponse>( ...
        #endregion
        #region HttpDelete
        /// <summary>
        /// Führt den Serviceaufruf durch.
        /// </summary>
        /// <param name="baseUrl">[in] Basis-URL des Services</param>
        /// <param name="action">[in] Name der Serviceaktion</param>
        /// <exception cref="System.ArgumentNullException">
        /// Wird ausgelöst, wenn der Parameter <paramref name="baseUrl"/> <i>null</i> oder leer ist.
        /// Wird ausgelöst, wenn der Parameter <paramref name="action"/> <i>null</i> oder leer ist.
        /// </exception>
        public async static Task HttpDelete(
            string baseUrl,
            string action)
        {
            // Service aufrufen
           await HttpDelete(baseUrl, action, null);
        } // public static void HttpDelete( ...

        /// <summary>
        /// Führt den Serviceaufruf durch.
        /// </summary>
        /// <param name="baseUrl">[in] Basis-URL des Services</param>
        /// <param name="action">[in] Name der Serviceaktion</param>
        /// <param name="queryItems">[in] Liste der zusätzlichen Aufrufparameter</param>
        /// <exception cref="System.ArgumentNullException">
        /// Wird ausgelöst, wenn der Parameter <paramref name="baseUrl"/> <i>null</i> oder leer ist.
        /// Wird ausgelöst, wenn der Parameter <paramref name="action"/> <i>null</i> oder leer ist.
        /// </exception>
        public async static Task HttpDelete(
            string baseUrl,
            string action,
            IDictionary<string, string> queryItems)
        {
            // Parameter prüfen
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentNullException(nameof(baseUrl));
            } // if (string.IsNullOrWhiteSpace(baseUrl))

            // URL in ein Uri-Objekt umwandeln
            Uri uri = new Uri(baseUrl);

            // Service aufrufen
            await HttpDelete(uri, action, queryItems);
        } // public async static Task HttpDelete( ...

        /// <summary>
        /// Führt den Serviceaufruf durch.
        /// </summary>
        /// <param name="baseUrl">[in] Basis-URL des Services</param>
        /// <param name="action">[in] Name der Serviceaktion</param>
        /// <exception cref="System.ArgumentNullException">
        /// Wird ausgelöst, wenn der Parameter <paramref name="baseUrl"/> <i>null</i> ist.
        /// Wird ausgelöst, wenn der Parameter <paramref name="action"/> <i>null</i> oder leer ist.
        /// </exception>
        public async static Task HttpDelete(
            Uri baseUrl,
            string action)
        {
            // Service aufrufen
            await HttpDelete(baseUrl, action, null);
        } // public async static Task HttpDelete( ...

        /// <summary>
        /// Führt den Serviceaufruf durch.
        /// </summary>
        /// <param name="baseUrl">[in] Basis-URL des Services</param>
        /// <param name="action">[in] Name der Serviceaktion</param>
        /// <param name="queryItems">[in] Liste der zusätzlichen Aufrufparameter</param>
        /// <exception cref="System.ArgumentNullException">
        /// Wird ausgelöst, wenn der Parameter <paramref name="baseUrl"/> <i>null</i> ist.
        /// Wird ausgelöst, wenn der Parameter <paramref name="action"/> <i>null</i> oder leer ist.
        /// </exception>
        public async static Task HttpDelete(
            Uri baseUrl,
            string action,
            IDictionary<string, string> queryItems)
        {
            // Parameter prüfen
            if (null == baseUrl)
            {
                throw new ArgumentNullException(nameof(baseUrl));
            } // if (null == baseUrl)
            if (string.IsNullOrEmpty(action))
            {
                throw new ArgumentNullException(nameof(action));
            } // if (string.IsNullOrEmpty(action))

            // Service aufrufen
            await httpDelete(baseUrl, action, queryItems);
        } // public async static Task HttpDelete( ...
        #endregion
        #region Private Hilfsfunktionen
        /// <summary>
        /// Führt den Serviceaufruf durch.
        /// </summary>
        /// <typeparam name="T">Typ, der als Ergebnis des Services erwartet wird.</typeparam>
        /// <param name="baseUrl">[in] Basis-URL des Services</param>
        /// <param name="action">[in] Name der Serviceaktion</param>
        /// <param name="queryItems">[in] Liste der zusätzlichen Aufrufparameter</param>
        /// <returns>Ergebnis des Serviceaufrufes.</returns>
        private async static Task<T> httpGet<T>(
			Uri baseUrl,
			string action,
			IDictionary<string, string> queryItems)
		{
			// Action und QueryItems zur URL hinzufügen
			Uri requestUrl = buildUrl(baseUrl, action, queryItems);

			// Serviceaufruf durchführen
			T result;
            HttpClient httpClient = new HttpClient();
            if (typeof(T) == typeof(byte[]))
            {
                // Antwortdaten als Byte-Array zurückgeben
                // TODO: Anschauen, warum dynamic vielleicht funktioniert!
                dynamic response = httpClient.GetByteArrayAsync(requestUrl).Result;
                result = response;
            }
            else
            {
                // Antwortdaten als Objekt zurückgeben
                string response = httpClient.GetStringAsync(requestUrl).Result;
                result = JsonFormatter.FromJson<T>(response);
            }

			return result;
        } // private async static Task<T> httpGet<T>( ...

        /// <summary>
        /// Führt den Serviceaufruf durch wobei Daten mitgegeben werden können.
        /// </summary>
        /// <typeparam name="TRequest">Typ, der als Daten mitgegeben wird.</typeparam>
        /// <typeparam name="TResponse">Typ, der als Ergebnis des Services erwartet wird.</typeparam>
        /// <param name="baseUrl">[in] Basis-URL des Services</param>
        /// <param name="action">[in] Name der Serviceaktion</param>
        /// <param name="queryItems">[in] Liste der zusätzlichen Aufrufparameter</param>
        /// <param name="payload">[in] Zusatzdaten, die beim Aufruf mitgegeben werden sollen</param>
        /// <returns>Ergebnis des Serviceaufrufes.</returns>
        private async static Task<TResponse> httpPost<TRequest, TResponse>(
			Uri baseUrl,
			string action,
			IDictionary<string, string> queryItems,
			TRequest payload)
		{
			// Action und QueryItems zur URL hinzufügen
			Uri requestUrl = buildUrl(baseUrl, action, queryItems);

            // Request ausführen
            HttpClient httpClient = new HttpClient();
            HttpContent httpContent;
            if (payload is byte[])
            {
                // zu übertragende Daten sind ein byte[]
                httpContent = new ByteArrayContent(payload as byte[]);
                httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
            }
            else
            {
                // zu übertragende Daten als JSON formatieren
                string payloadString = JsonFormatter.ToJson<TRequest>(payload);
                httpContent = new StringContent(payloadString, Encoding.UTF8, "application/json");
            }
            HttpResponseMessage response = httpClient.PostAsync(requestUrl, httpContent).Result;

            // Response lesen und auswerten
            TResponse result;
            if (response.IsSuccessStatusCode)
            {
                result = JsonFormatter.FromJson<TResponse>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new InvalidOperationException();
            }

			return result;
        } // private async static Task<TResponse> httpPost<TRequest, TResponse>( ...

        /// <summary>
        /// Führt den Serviceaufruf durch wobei Daten mitgegeben werden können.
        /// </summary>
        /// <typeparam name="TRequest">Typ, der als Daten mitgegeben wird.</typeparam>
        /// <typeparam name="TResponse">Typ, der als Ergebnis des Services erwartet wird.</typeparam>
        /// <param name="baseUrl">[in] Basis-URL des Services</param>
        /// <param name="action">[in] Name der Serviceaktion</param>
        /// <param name="queryItems">[in] Liste der zusätzlichen Aufrufparameter</param>
        /// <param name="payload">[in] Zusatzdaten, die beim Aufruf mitgegeben werden sollen</param>
        /// <returns>Ergebnis des Serviceaufrufes.</returns>
        private async static Task<TResponse> httpPut<TRequest, TResponse>(
            Uri baseUrl,
            string action,
            IDictionary<string, string> queryItems,
            TRequest payload)
        {
            // Action und QueryItems zur URL hinzufügen
            Uri requestUrl = buildUrl(baseUrl, action, queryItems);

            // Request ausführen
            HttpClient httpClient = new HttpClient();
            HttpContent httpContent;
            if (payload is byte[])
            {
                // zu übertragende Daten sind ein byte[]
                httpContent = new ByteArrayContent(payload as byte[]);
                httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
            }
            else
            {
                // zu übertragende Daten als JSON formatieren
                string payloadString = JsonFormatter.ToJson<TRequest>(payload);
                httpContent = new StringContent(payloadString, Encoding.UTF8, "application/json");
            }
            HttpResponseMessage response = httpClient.PutAsync(requestUrl, httpContent).Result;

            // Response lesen und auswerten
            TResponse result;
            if (response.IsSuccessStatusCode)
            {
                result = JsonFormatter.FromJson<TResponse>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new InvalidOperationException(response.ReasonPhrase);
            }

            return result;
        } // private async static Task<TResponse> httpPut<TRequest, TResponse>( ...

        /// <summary>
        /// Führt den Serviceaufruf durch.
        /// </summary>
        /// <typeparam name="T">Typ, der als Ergebnis des Services erwartet wird.</typeparam>
        /// <param name="baseUrl">[in] Basis-URL des Services</param>
        /// <param name="action">[in] Name der Serviceaktion</param>
        /// <param name="queryItems">[in] Liste der zusätzlichen Aufrufparameter</param>
        /// <returns>Ergebnis des Serviceaufrufes.</returns>
        private async static Task httpDelete(
            Uri baseUrl,
            string action,
            IDictionary<string, string> queryItems)
        {
            // Action und QueryItems zur URL hinzufügen
            Uri requestUrl = buildUrl(baseUrl, action, queryItems);

            // Serviceaufruf durchführen
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = httpClient.DeleteAsync(requestUrl).Result;

            // Response lesen und auswerten
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException();
            }
        } // private async static Task httpDelete( ...

        /// <summary>
        /// Erzeugt die URL, die letztelich für den Serviceaufruf verwendet wird.
        /// </summary>
        /// <param name="baseUrl">[in] Basis-URL des Services</param>
        /// <param name="action">[in] Name der Serviceaktion</param>
        /// <param name="queryItems">[in] Liste der zusätzlichen Aufrufparameter</param>
        /// <returns>URL für den Serviceaufruf.</returns>
        private static Uri buildUrl(
			Uri baseUrl,
			string action,
			IDictionary<string, string> queryItems)
		{
			// URL zusammensetzen
			UriBuilder resultBuilder = new UriBuilder(baseUrl);

			// Action definieren
			resultBuilder.Path = resultBuilder.Path + action;

			// eventuell die QueryItems anhängen
			if (null != queryItems)
			{
				StringBuilder queryBuilder = new StringBuilder(1024);
				if (!string.IsNullOrEmpty(resultBuilder.Query))
				{
					// bestehende Query beibehalten
					queryBuilder.Append(resultBuilder.Query);
				} // if (!string.IsNullOrEmpty(resultBuilder.Query))
				foreach (string key in queryItems.Keys)
				{
					// QueryItem anhängen
					string value = queryItems[key];
					queryBuilder.AppendFormat(
						"{0}{1}{2}{3}",
						((0 == queryBuilder.Length) ? "?" : "&"),
						key,
						(string.IsNullOrEmpty(value) ? string.Empty : "="),
						value);
				} // foreach (string key in queryItems.Keys)

				// zusammengesetzte Query in die URL aufnehmen
				resultBuilder.Query = queryBuilder.ToString();
			} // if (null != queryItems)

			// Ergebnis erzeugen
			Uri result = resultBuilder.Uri;
			return result;
		} // private static Uri buildUrl( ...
		#endregion
	} // static class WebRequester
} // namespace CeTraSS.Client
