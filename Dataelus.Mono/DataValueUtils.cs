using System;

namespace Dataelus.Mono
{
	public class DataValueUtils
	{
		public static string NullableStringToEmpty (object value)
		{
			return (value == null || DBNull.Value.Equals (value)) ? String.Empty : value.ToString ();
		}

		public static string NullableString (object value)
		{
			return (value == null || DBNull.Value.Equals (value)) ? null : value.ToString ();
		}
	}
}

