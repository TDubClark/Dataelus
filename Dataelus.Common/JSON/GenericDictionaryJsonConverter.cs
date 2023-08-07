using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Dataelus.JSON
{
	/// <summary>
	/// Generic dictionary JSON converter.
	/// </summary>
	public class GenericDictionaryJsonConverter<K, V> : JsonConverter
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.JSON.GenericDictionaryJsonConverter`2"/> class.
		/// </summary>
		public GenericDictionaryJsonConverter ()
			: base ()
		{
		}

		#region implemented abstract members of JsonConverter

		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		/// <param name="writer">Writer.</param>
		/// <param name="value">Value.</param>
		/// <param name="serializer">Serializer.</param>
		public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
		{
			// Best approach: copy to a list of key/value pair objects
			// Source: http://stackoverflow.com/questions/24674405/newtonsoft-jsonconvert-how-to-serialize-class-object-key-from-dictionaryclas?rq=1
			serializer.Serialize (writer, ((IDictionary<K, V>)value).ToList ());

			/*
			Type objType = value.GetType ();
			var keys = (IEnumerable<K>)objType.GetProperty ("Keys").GetValue (value, null);
			var values = (IEnumerable<V>)objType.GetProperty ("Values").GetValue (value, null);

			var valueEnumer = values.GetEnumerator ();

			writer.WriteStartArray ();

			foreach (var key in keys) {
				// Move to the next value
				valueEnumer.MoveNext ();

				writer.WriteStartObject ();
				writer.WritePropertyName ("key");

				var keyConverter = serializer.Converters.FirstOrDefault (x => x.CanConvert (typeof(K)));
				if (keyConverter != null) {
					keyConverter.WriteJson (writer, key, serializer);
				} else {
					serializer.Serialize (writer, key, typeof(K));
					//writer.WriteValue (key);
				}

				writer.WritePropertyName ("value");
				var valueConverter = serializer.Converters.FirstOrDefault (x => x.CanConvert (typeof(V)));
				if (valueConverter != null) {
					valueConverter.WriteJson (writer, valueEnumer.Current, serializer);
				} else {
					serializer.Serialize (writer, valueEnumer.Current, typeof(V));
					//writer.WriteValue (valueEnumer.Current);
				}

				writer.WriteEndObject ();
			}

			writer.WriteEndArray ();
			*/
		}

		/// <summary>
		/// Reads the JSON representation of the object.
		/// </summary>
		/// <returns>The json.</returns>
		/// <param name="reader">Reader.</param>
		/// <param name="objectType">Object type.</param>
		/// <param name="existingValue">Existing value.</param>
		/// <param name="serializer">Serializer.</param>
		public override object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var dict = new Dictionary<K, V> ();

			var list = dict.ToList ();
			serializer.Populate (reader, list);

			dict.AddRange (list);

			return dict;
		}

		/// <summary>
		/// Determines whether this instance can convert the specified object type.
		/// </summary>
		/// <param name="objectType">Type of the object.</param>
		/// <returns><c>true</c> if this instance can convert the specified objectType; otherwise, <c>false</c>.</returns>
		public override bool CanConvert (Type objectType)
		{
			return typeof(IDictionary<K, V>).IsAssignableFrom (objectType)
			|| IsImplementInterface (objectType);
		}

		#endregion

		/// <summary>
		/// Determines whether objectType implements the appropriate interfaces.
		/// </summary>
		/// <returns><c>true</c> if this instance is implement interface the specified objectType; otherwise, <c>false</c>.</returns>
		/// <param name="objectType">Object type.</param>
		bool IsImplementInterface (Type objectType)
		{
			// Source: http://stackoverflow.com/questions/982389/c-sharp-get-the-types-defining-a-dictionary-at-run-time
			return objectType.GetInterfaces ()
				.Any (x => (x.IsGenericType
			&& x.GetGenericTypeDefinition ().Equals (typeof(IDictionary<,>))
			&& ((x.GetGenericArguments ().Length == 2)
			&& x.GetGenericArguments () [0].Equals (typeof(K))
			&& x.GetGenericArguments () [1].Equals (typeof(V)))
			));
		}
	}

	/// <summary>
	/// Dictionary extensions.
	/// </summary>
	public static class DictionaryExtensions
	{
		/// <summary>
		/// Adds the range of key/value pairs to the dictionary.
		/// </summary>
		/// <param name="dictionary">Dictionary.</param>
		/// <param name="collection">Collection.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		/// <typeparam name="V">The 2nd type parameter.</typeparam>
		public static void AddRange<K, V> (this Dictionary<K, V> dictionary, IEnumerable<KeyValuePair<K, V>> collection)
		{
			foreach (var item in collection) {
				dictionary.Add (item.Key, item.Value);
			}
		}
	}
}

