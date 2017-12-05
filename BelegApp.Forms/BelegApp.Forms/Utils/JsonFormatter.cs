using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BelegApp.Forms.Utils
{
	/// <summary>
	/// Diese Klasse stellt Funktionen zum Umwandeln von Objekten in JSON-Strings und umgekehrt zur Verfügung.
	/// </summary>
	static class JsonFormatter
	{
		/// <summary>
		/// Serialisiert das übergebene Objekt in einen JSON-String.
		/// </summary>
		/// <typeparam name="T">Gibt den Typ an, der serialisiert wird.</typeparam>
		/// <param name="model">[in] Objekt, das serialisiert werden soll.</param>
		/// <returns>Die Objektdaten als JSON-String.</returns>
		public static string ToJson<T>(T model)
		{
			// Objekt in einen JSON-String serialisieren
			string result = JsonConvert.SerializeObject(model);
			return result;
		} // public static string ToJson<T>(T model)

		/// <summary>
		/// Konvertiert einen JSON-String in ein Objekt des angegebenen Typs.
		/// </summary>
		/// <typeparam name="T">Gibt den Typ an, der instanziert und zurückgegeben wird.</typeparam>
		/// <param name="jsonModel">[in] Der JSON-String mit den Objektdaten.</param>
		/// <returns>Instanz des Objektes mit den übergebenen Daten.</returns>
		public static T FromJson<T>(string jsonModel)
		{
			// JSON-String in ein Objekt umwandeln
			T result = JsonConvert.DeserializeObject<T>(jsonModel);
			return result;
		} // public static T FromJson<T>(string jsonModel)

		/// <summary>
		/// Konvertiert einen JSON-String aus einem <see cref="System.IO.Stream"/> in ein Objekt des angegebenen Typs.
		/// </summary>
		/// <typeparam name="T">Gibt den Typ an, der instanziert und zurückgegeben wird.</typeparam>
		/// <param name="jsonStream">[in] <see cref="System.IO.Stream"/> mit dem JSON-String mit den Objektdaten.</param>
		/// <returns>Instanz des Objektes mit den übergebenen Daten.</returns>
		public static T FromJson<T>(Stream jsonStream)
		{
			// Stream in einen String einlesen
			string jsonString = null;
			using
				(StreamReader streamReader = new StreamReader(jsonStream))
			{
				jsonString = streamReader.ReadToEnd();
			} // using (StreamReader)

			// String in ein Objekt umwandeln
			T result = FromJson<T>(jsonString);
			return result;
		} // public static T FromJson<T>(Stream jsonStream)
	} // static class JsonFormatter
} // namespace CeTraSS.Client
