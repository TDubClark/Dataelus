using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Dataelus.JSON
{
	// The source code for class "CustomDictionaryConverter" was copied verbatim from this web site:
	// 		Source: http://stackoverflow.com/questions/18385325/serialize-dictionary-as-array-in-json-net
	public class CustomDictionaryConverter : JsonConverter
	{
		public override bool CanConvert (Type objectType)
		{
			return (typeof(IDictionary).IsAssignableFrom (objectType) ||
			TypeImplementsGenericInterface (objectType, typeof(IDictionary<,>)));
		}

		private static bool TypeImplementsGenericInterface (Type concreteType, Type interfaceType)
		{
			return concreteType.GetInterfaces ()
			.Any (i => i.IsGenericType && i.GetGenericTypeDefinition () == interfaceType);
		}

		public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
		{
			Type type = value.GetType ();
			IEnumerable keys = (IEnumerable)type.GetProperty ("Keys").GetValue (value, null);
			IEnumerable values = (IEnumerable)type.GetProperty ("Values").GetValue (value, null);
			IEnumerator valueEnumerator = values.GetEnumerator ();

			writer.WriteStartArray ();
			foreach (object key in keys) {
				valueEnumerator.MoveNext ();

				writer.WriteStartObject ();
				writer.WritePropertyName ("key");
				writer.WriteValue (key);
				writer.WritePropertyName ("value");
				serializer.Serialize (writer, valueEnumerator.Current);
				writer.WriteEndObject ();
			}
			writer.WriteEndArray ();
		}

		public override object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException ();
		}
	}
}

