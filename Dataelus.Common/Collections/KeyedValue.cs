using System;

namespace Dataelus.Collections
{
	/// <summary>
	/// Stores a Keyed value.
	/// </summary>
	public class KeyedValue
	{
		public string Key { get; set; }

		public object Value { get; set; }

		public KeyedValue ()
		{
		}

		public KeyedValue (string key, object value)
		{
			this.Key = key;
			this.Value = value;
		}
	}

	/// <summary>
	/// Keyed value collection.
	/// </summary>
	public class KeyedValueCollection : ListBase<KeyedValue>, System.Collections.IEnumerable
	{
		public KeyedValueCollection ()
		{
		}
	}
}
