using System;

namespace Dataelus
{
	/// <summary>
	/// Interface for a parser of text input.
	/// </summary>
	public interface ITextInputParser
	{
		/// <summary>
		/// Gets the Format suggestion.
		/// </summary>
		/// <returns>The suggestion.</returns>
		string GetFormatSuggestion ();

		/// <summary>
		/// Tries to parse the given text.
		/// </summary>
		/// <returns><c>true</c>, if parse was tryed, <c>false</c> otherwise.</returns>
		/// <param name="text">Text.</param>
		/// <param name="value">Value.</param>
		bool TryParse (string text, out object value);
	}

	public static class TextInputParser
	{
		#region Static Members

		/// <summary>
		/// Gets the type dictionary for default Text Input Parsers.
		/// </summary>
		/// <returns>The type dictionary.</returns>
		public static System.Collections.Generic.Dictionary<Type, ITextInputParser> GetTypeDictionary ()
		{
			var dict = new System.Collections.Generic.Dictionary<Type, ITextInputParser> ();

			dict.Add (typeof(System.Int64), new TextInputParserInteger (typeof(System.Int64)));
			dict.Add (typeof(System.Int32), new TextInputParserInteger (typeof(System.Int32)));
			dict.Add (typeof(System.Int16), new TextInputParserInteger (typeof(System.Int16)));
			dict.Add (typeof(System.SByte), new TextInputParserInteger (typeof(System.SByte)));
			dict.Add (typeof(System.UInt64), new TextInputParserInteger (typeof(System.UInt64)));
			dict.Add (typeof(System.UInt32), new TextInputParserInteger (typeof(System.UInt32)));
			dict.Add (typeof(System.UInt16), new TextInputParserInteger (typeof(System.UInt16)));
			dict.Add (typeof(System.Byte), new TextInputParserInteger (typeof(System.Byte)));
			dict.Add (typeof(System.Double), new TextInputParserDouble ());
			dict.Add (typeof(System.Single), new TextInputParserDouble (true));
			dict.Add (typeof(System.Decimal), new TextInputParserDecimal ());

			dict.Add (typeof(System.String), new TextInputParserString ());
			dict.Add (typeof(System.DateTime), new TextInputParserDateTime ());

			dict.Add (typeof(System.Boolean), new TextInputParserBoolean ());

			return dict;
		}

		public static ITextInputParser GetParserByType (Type dataType)
		{
			ITextInputParser parser = null;
			return GetTypeDictionary ().TryGetValue (dataType, out parser) ? parser : null;
		}

		#endregion
	}

	/// <summary>
	/// Text input parser for an integer type (8, 16, 32, or 64 bit; signed or unsigned).
	/// </summary>
	public class TextInputParserInteger : ITextInputParser
	{
		protected Type _dataType;

		public TextInputParserInteger ()
			: this (typeof(System.Int64))
		{
		}

		public TextInputParserInteger (Type dataType)
		{
			if (dataType.Equals (typeof(SByte))
			    || dataType.Equals (typeof(Int16))
			    || dataType.Equals (typeof(Int32))
			    || dataType.Equals (typeof(Int64))
			    || dataType.Equals (typeof(Byte))
			    || dataType.Equals (typeof(UInt16))
			    || dataType.Equals (typeof(UInt32))
			    || dataType.Equals (typeof(UInt64))) {
				this._dataType = dataType;
			} else {
				throw new ArgumentOutOfRangeException ("dataType", String.Format ("Expecting an integer type: SByte, Int16, Int32, Int64 (signed) or Byte, UInt16, UInt32, UInt64 (unsigned); received: {0}", dataType));
			}
		}

		#region ITextInputParser implementation

		public string GetFormatSuggestion ()
		{
			return "[-]#";
		}

		public bool TryParse (string text, out object value)
		{
			bool isParse = false;
			value = default(long);

			if (_dataType.Equals (typeof(SByte))) {
				sbyte temp;
				isParse = SByte.TryParse (text, out temp);
				value = temp;

			} else if (_dataType.Equals (typeof(Int16))) {
				Int16 temp;
				isParse = Int16.TryParse (text, out temp);
				value = temp;

			} else if (_dataType.Equals (typeof(Int32))) {
				int temp;
				isParse = Int32.TryParse (text, out temp);
				value = temp;

			} else if (_dataType.Equals (typeof(Int64))) {
				long temp;
				isParse = Int64.TryParse (text, out temp);
				value = temp;

			} else if (_dataType.Equals (typeof(Byte))) {
				byte temp;
				isParse = Byte.TryParse (text, out temp);
				value = temp;

			} else if (_dataType.Equals (typeof(UInt16))) {
				UInt16 temp;
				isParse = UInt16.TryParse (text, out temp);
				value = temp;

			} else if (_dataType.Equals (typeof(UInt32))) {
				UInt32 temp;
				isParse = UInt32.TryParse (text, out temp);
				value = temp;

			} else if (_dataType.Equals (typeof(UInt64))) {
				UInt64 temp;
				isParse = UInt64.TryParse (text, out temp);
				value = temp;
			}

			return isParse;
		}

		#endregion
	}

	/// <summary>
	/// Text input parser for a floating-point number (either single or double precision).
	/// </summary>
	public class TextInputParserFloatingPoint : ITextInputParser
	{
		bool _singlePrecision;

		public TextInputParserFloatingPoint (bool singlePrecision)
		{
			this._singlePrecision = singlePrecision;
		}

		#region ITextInputParser implementation

		public string GetFormatSuggestion ()
		{
			return "[-]#[.#]";
		}

		public bool TryParse (string text, out object value)
		{
			bool isParse = false;

			if (_singlePrecision) {
				float temp;
				isParse = Single.TryParse (text, out temp);
				value = temp;

			} else {
				double temp;
				isParse = Double.TryParse (text, out temp);
				value = temp;
			}

			return isParse;
		}

		#endregion
	}

	/// <summary>
	/// Text input parser for a floating-point number (defaults to double precision).
	/// </summary>
	public class TextInputParserDouble : TextInputParserFloatingPoint
	{
		public TextInputParserDouble ()
			: base (false)
		{
		}

		public TextInputParserDouble (bool singlePrecision)
			: base (singlePrecision)
		{
		}
	}

	public class TextInputParserDecimal : ITextInputParser
	{
		#region ITextInputParser implementation

		public string GetFormatSuggestion ()
		{
			return "[-]#[.#]";
		}

		public bool TryParse (string text, out object value)
		{
			decimal temp;
			bool isParse = Decimal.TryParse (text, out temp);

			value = temp;
			return isParse;
		}

		#endregion
	}

	public class TextInputParserDateTime : ITextInputParser
	{
		#region ITextInputParser implementation

		public string GetFormatSuggestion ()
		{
			return "M/d/yyyy [h:mm [am|pm]]";
		}

		public bool TryParse (string text, out object value)
		{
			DateTime temp;
			bool isParse = DateTime.TryParse (text, out temp);

			value = temp;
			return isParse;
		}

		#endregion
	}

	public class TextInputParserBoolean : ITextInputParser
	{
		#region ITextInputParser implementation

		public string GetFormatSuggestion ()
		{
			return "true|false";
		}

		public bool TryParse (string text, out object value)
		{
			bool temp;
			bool isParse = Boolean.TryParse (text, out temp);

			value = temp;
			return isParse;
		}

		#endregion
	}

	public class TextInputParserString : ITextInputParser
	{
		#region ITextInputParser implementation

		public string GetFormatSuggestion ()
		{
			return "";
		}

		public bool TryParse (string text, out object value)
		{
			value = text;
			return true;
		}

		#endregion
	}
}

