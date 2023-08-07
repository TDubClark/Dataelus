using System;
using System.Text.RegularExpressions;

namespace Dataelus
{
	/// <summary>
	/// Interface for an object which interprets text input (either from the user or a file) as a value.
	/// </summary>
	public interface ITextInputInterpreter
	{
		/// <summary>
		/// Interprets the specified input, and outputs as interprettedInput; outputs interpretation message.
		/// Returns whether the given value is valid and acceptable
		/// </summary>
		/// <param name="input">Input.</param>
		/// <param name="defaultValue">The default value, if the input cannot be interpretted</param>
		/// <param name="interprettedInput">Interpretted input (returns [defaultValue] if [input] cannot be interpretted).</param>
		/// <param name="message">The validation message.</param>
		bool Interpret (string input, string defaultValue, out string interprettedInput, out string message);

		/// <summary>
		/// Gets or sets a value indicating whether this instance allows null values.
		/// </summary>
		/// <value><c>true</c> if this instance is allow null; otherwise, <c>false</c>.</value>
		bool IsAllowNull { get; set; }

		/// <summary>
		/// Gets or sets the text pre-format method.  This pre-formats the text prior to interpretation.
		/// </summary>
		/// <value>The text pre-format method.</value>
		StringComparisonMethod TextPreFormatMethod { get; set; }

		/// <summary>
		/// Determines whether the given input value is Null or equivalent.
		/// </summary>
		/// <returns><c>true</c> if this instance is null the specified input; otherwise, <c>false</c>.</returns>
		/// <param name="input">Input.</param>
		bool IsNull (string input);

		/// <summary>
		/// Interprets the specified input, and returns the interpretted text (or the defaultValue, where not interprettable).
		/// </summary>
		/// <param name="input">Input.</param>
		/// <param name="defaultValue">The default value, if the input cannot be interpretted</param>
		string Interpret (string input, string defaultValue);
	}

	/// <summary>
	/// Abstract Base implementation of ITextInputInterpreter.
	/// </summary>
	public abstract class TextInputInterpreter : ITextInputInterpreter
	{
		/// <summary>
		/// Whether to allow Null values.
		/// </summary>
		protected bool _isAllowNull;

		/// <summary>
		/// Gets or sets a value indicating whether this instance allows null values.
		/// </summary>
		/// <value>true</value>
		/// <c>false</c>
		public bool IsAllowNull {
			get { return _isAllowNull; }
			set { _isAllowNull = value; }
		}

		/// <summary>
		/// The text pre-format method.
		/// </summary>
		protected StringComparisonMethod _textPreFormatMethod;

		/// <summary>
		/// Gets or sets the text pre-format method. This pre-formats the text prior to interpretation.
		/// </summary>
		/// <value>The text pre-format method.</value>
		public StringComparisonMethod TextPreFormatMethod {
			get { return _textPreFormatMethod; }
			set { _textPreFormatMethod = value; }
		}

		/// <summary>
		/// The comparer.
		/// </summary>
		protected System.Collections.Generic.IEqualityComparer<string> _comparer;

		/// <summary>
		/// Gets or sets the comparer for string values.
		/// </summary>
		/// <value>The comparer.</value>
		public System.Collections.Generic.IEqualityComparer<string> Comparer {
			get { return _comparer; }
			set { _comparer = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.TextInputInterpreter"/> class.
		/// </summary>
		protected TextInputInterpreter ()
			: this (true)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.TextInputInterpreter"/> class.
		/// </summary>
		/// <param name="isAllowNull">If set to <c>true</c>, then allows null values.</param>
		protected TextInputInterpreter (bool isAllowNull)
			: this (isAllowNull, new StringComparisonMethod ())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.TextInputInterpreter"/> class.
		/// </summary>
		/// <param name="isAllowNull">If set to <c>true</c>, then allows null values.</param>
		/// <param name="textFormatMethod">Text format method.</param>
		protected TextInputInterpreter (bool isAllowNull, StringComparisonMethod textFormatMethod)
		{
			_isAllowNull = isAllowNull;
			_textPreFormatMethod = textFormatMethod;
			this.Comparer = new StringEqualityComparer (textFormatMethod);
		}

		/// <summary>
		/// Preformats the specified input.
		/// </summary>
		/// <param name="input">Input.</param>
		public virtual string Preformat (string input)
		{
			if (_textPreFormatMethod != null)
				return _textPreFormatMethod.GetComparableString (input);
			return input;
		}

		/// <summary>
		/// Interprets the non-null input.
		/// </summary>
		/// <returns><c>true</c>, if non null was interpreted as valid, <c>false</c> otherwise.</returns>
		/// <param name="input">Input.</param>
		/// <param name="defaultValue">Default value.</param>
		/// <param name="interprettedInput">Interpretted input.</param>
		/// <param name="message">Message.</param>
		protected abstract bool InterpretNonNull (string input, string defaultValue, ref string interprettedInput, ref string message);

		#region ITextInputInterpreter implementation

		/// <summary>
		/// Determines whether the given input value is Null or equivalent.
		/// </summary>
		/// <returns>true</returns>
		/// <c>false</c>
		/// <param name="input">Input.</param>
		public bool IsNull (string input)
		{
			input = Preformat (input);

			return this.Comparer.Equals (input, null);
		}

		/// <summary>
		/// Interprets the specified input, and outputs as interprettedInput; outputs interpretation message.
		/// Returns whether the given value is valid and acceptable
		/// </summary>
		/// <param name="input">Input.</param>
		/// <param name="defaultValue">The default value, if the input cannot be interpretted</param>
		/// <param name="interprettedInput">Interpretted input (returns [defaultValue] if [input] cannot be interpretted).</param>
		/// <param name="message">The validation message.</param>
		public virtual bool Interpret (string input, string defaultValue, out string interprettedInput, out string message)
		{
			message = null;
			interprettedInput = null;

			// Changed from re-assigning to Input
			//   >> It was removing case from any string value
			string inputFormatted = Preformat (input);

			if (this.Comparer.Equals (inputFormatted, null)) { //(String.IsNullOrWhiteSpace (input)) {
				// input is Null (or equavalent)
				if (!_isAllowNull) {
					interprettedInput = defaultValue;
					message = "Null values not allowed.";
					return false;
				}
				return _isAllowNull;
			} else {
				return InterpretNonNull (input, defaultValue, ref interprettedInput, ref message);
			}
		}

		/// <summary>
		/// Interprets the specified input, and returns the interpretted text (or the defaultValue, where not interprettable).
		/// </summary>
		/// <param name="input">Input.</param>
		/// <param name="defaultValue">The default value, if the input cannot be interpretted</param>
		public virtual string Interpret (string input, string defaultValue)
		{
			string message;
			string output;
			if (Interpret (input, defaultValue, out output, out message)) {
				return output;
			}
			return defaultValue;
		}

		#endregion

		#region Static Members

		/// <summary>
		/// Gets the type dictionary for default Text Input Interpreters.
		/// </summary>
		/// <returns>The type dictionary.</returns>
		public static System.Collections.Generic.Dictionary<Type, ITextInputInterpreter> GetTypeDictionary ()
		{
			var dict = new System.Collections.Generic.Dictionary<Type, ITextInputInterpreter> ();

			dict.Add (typeof(System.Int64), new TextInputInterpreterInteger ());
			dict.Add (typeof(System.Int32), new TextInputInterpreterInteger ());
			dict.Add (typeof(System.Int16), new TextInputInterpreterInteger ());
			dict.Add (typeof(System.Byte), new TextInputInterpreterInteger ());
			dict.Add (typeof(System.Double), new TextInputInterpreterDouble ());
			dict.Add (typeof(System.Single), new TextInputInterpreterDouble ());
			dict.Add (typeof(System.Decimal), new TextInputInterpreterDecimal ());

			dict.Add (typeof(System.String), new TextInputInterpreterString ());
			dict.Add (typeof(System.DateTime), new TextInputInterpreterDateTime ());

			dict.Add (typeof(System.Boolean), new TextInputInterpreterBoolean ());

			return dict;
		}

		public static ITextInputInterpreter GetInterpreterByType (Type dataType)
		{
			ITextInputInterpreter interpreter = null;
			return GetTypeDictionary ().TryGetValue (dataType, out interpreter) ? interpreter : null;
		}

		/// <summary>
		/// Gets the interpreter for the given data type.
		/// </summary>
		/// <returns>The interpreter by type.</returns>
		/// <param name="dataType">Data type.</param>
		/// <param name="field">The Database Field.</param>
		public static ITextInputInterpreter GetInterpreterByType (Type dataType, Dataelus.Database.IDBField field)
		{
			return GetInterpreterByType (dataType, field.Nullable);
		}

		/// <summary>
		/// Gets the interpreter for the given data type.
		/// </summary>
		/// <returns>The interpreter by type.</returns>
		/// <param name="dataType">Data type.</param>
		/// <param name="nullable">Whether the interpreter should allow Null values.</param>
		public static ITextInputInterpreter GetInterpreterByType (Type dataType, bool nullable)
		{
			ITextInputInterpreter interpreter = null;
			if (!GetTypeDictionary ().TryGetValue (dataType, out interpreter)) { 
				interpreter = null;
			} else {
				interpreter.IsAllowNull = nullable;
			}
			return interpreter;
		}

		#endregion
	}

	/// <summary>
	/// Text input interpreter for String data types.
	/// </summary>
	public class TextInputInterpreterString : TextInputInterpreter
	{
		public TextInputInterpreterString ()
			: base ()
		{
		}

		public TextInputInterpreterString (bool isAllowNull)
			: base (isAllowNull)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.TextInputInterpreterString"/> class.
		/// </summary>
		/// <param name="isAllowNull">If set to <c>true</c> is allow null.</param>
		/// <param name="trim">Whether to trim any incoming strings.</param>
		public TextInputInterpreterString (bool isAllowNull, bool trim)
			: base (isAllowNull)
		{
			_textPreFormatMethod.Trim = trim;
		}

		#region implemented abstract members of TextInputInterpreter

		protected override bool InterpretNonNull (string input, string defaultValue, ref string interprettedInput, ref string message)
		{
			// Just set the Interpretted input to the Input
			interprettedInput = input;
			return true;
		}

		#endregion
	}

	/// <summary>
	/// Text input interpreter date/time data types.
	/// </summary>
	public class TextInputInterpreterDateTime : TextInputInterpreter
	{
		protected bool _isDatePart;

		/// <summary>
		/// Gets or sets a value indicating whether date/time values are assumed to have a date part.
		/// </summary>
		/// <value><c>true</c> if date part; otherwise, <c>false</c>.</value>
		public bool IsDatePart {
			get { return _isDatePart; }
			set { _isDatePart = value; }
		}

		protected bool _isTimePart;

		/// <summary>
		/// Gets or sets a value indicating whether date/time values are assumed to have a time part.
		/// </summary>
		/// <value><c>true</c> if time part; otherwise, <c>false</c>.</value>
		public bool IsTimePart {
			get { return _isTimePart; }
			set { _isTimePart = value; }
		}

		protected bool _allowFutureDates;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Dataelus.TextInputInterpreterDateTime"/> allows future dates.
		/// (Not Implemented)
		/// </summary>
		/// <value><c>true</c> if allow future dates; otherwise, <c>false</c>.</value>
		public bool AllowFutureDates {
			get { return _allowFutureDates; }
			set { _allowFutureDates = value; }
		}


		// Upper and lower bounds for the time part
		protected DateTime _timeUpperBound;
		protected DateTime _timeLowerBound;

		// Upper and lower bounds for the date part
		protected DateTime _dateUpperBound;
		protected DateTime _dateLowerBound;


		protected string _formatString;

		/// <summary>
		/// Gets or sets the format string.
		/// </summary>
		/// <value>The format string.</value>
		public string FormatString {
			get { return _formatString; }
			set { _formatString = value; }
		}

		public TextInputInterpreterDateTime ()
			: this (true, true)
		{
		}

		public TextInputInterpreterDateTime (bool datePart, bool timePart)
			: this (datePart, timePart, GetFormat (datePart, timePart))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.TextInputInterpreterDateTime"/> class. Allows Null values.
		/// </summary>
		/// <param name="datePart">If set to <c>true</c> date part.</param>
		/// <param name="timePart">If set to <c>true</c> time part.</param>
		/// <param name="format">Format.</param>
		public TextInputInterpreterDateTime (bool datePart, bool timePart, string format)
			: this (datePart, timePart, format, true)
		{
		}

		public TextInputInterpreterDateTime (bool datePart, bool timePart, string format, bool isAllowNull)
			: base (isAllowNull)
		{
			_isDatePart = datePart;
			_isTimePart = timePart;
			_formatString = format;
			_allowFutureDates = true;
		}

		protected static string GetFormat (bool datePart, bool timePart)
		{
			if (datePart) {
				if (timePart) {
					return "g"; // Short Date/Time
				} else {
					return "d"; // Short Date
				}
			} else if (timePart) {
				return "t";  // Short Time
			}
			return "g";
		}

		/// <summary>
		/// Interprets the raw integer string as a date part.
		/// </summary>
		/// <returns>The interpretted/formatted Date Text.</returns>
		/// <param name="dateText">The Raw Integer Input.</param>
		public virtual string InterpretRawIntegerDatePart (string dateText)
		{
			var now = DateTime.Now;
			switch (dateText.Length) {
			case 1:
				dateText = String.Format ("{0:MM}/0{1}/{0:yyyy}", now, dateText);
				break;
			case 2:
				dateText = String.Format ("0{1}/0{2}/{0:yyyy}", now, dateText [0], dateText [1]);
				break;
			case 3:
				dateText = String.Format ("0{1}/{2}{3}/{0:yyyy}", now, dateText [0], dateText [1], dateText [2]);
				break;
			case 4:
				dateText = String.Format ("{1}/{0:yyyy}", now, Regex.Replace (dateText, @"^(?<m>\d\d)(?<d>\d\d)", "${m}/${d}"));
				break;
			case 5:
				dateText = Regex.Replace (dateText, @"^(?<m>\d)(?<d>\d\d)(?<y>\d{2})$", "${m}/${d}/${y}");
				break;
			case 6:
				dateText = Regex.Replace (dateText, @"^(?<m>\d)(?<d>\d)(?<y>\d{4})$", "${m}/${d}/${y}");
				break;
			case 7:
				dateText = Regex.Replace (dateText, @"^(?<m>\d)(?<d>\d\d)(?<y>\d{4})$", "${m}/${d}/${y}");
				break;
			case 8:
				dateText = Regex.Replace (dateText, @"^(?<m>\d\d)(?<d>\d\d)(?<y>\d{4})$", "${m}/${d}/${y}");
				break;
			default:
				break;
			}
			return dateText;
		}

		/// <summary>
		/// Interprets the raw integer string as a time part.
		/// </summary>
		/// <returns>The interpretted/formatted time part.</returns>
		/// <param name="timeText">Raw IntegerTime text.</param>
		public virtual string InterpretRawIntegerTimePart (string timeText)
		{
			var now = DateTime.Now;
			switch (timeText.Length) {
			case 1:
				timeText = String.Format ("0{1}:{0:mm}", now, timeText);
				break;
			case 2:
				timeText = String.Format ("0{0}:0{1}", timeText [0], timeText [1]);
				break;
			case 3:
				timeText = Regex.Replace (timeText, @"^(?<h>\d)(?<m>\d\d)$", "${h}:${m}");
				break;
			case 4:
				timeText = Regex.Replace (timeText, @"^(?<h>\d\d)(?<m>\d\d)$", "${h}:${m}");
				break;
			case 5:
				timeText = Regex.Replace (timeText, @"^(?<h>\d)(?<m>\d\d)(?<s>\d\d)$", "${h}:${m}:${s}");
				break;
			case 6:
				timeText = Regex.Replace (timeText, @"^(?<h>\d\d)(?<m>\d\d)(?<s>\d\d)$", "${h}:${m}:${s}");
				break;
			default:
				break;
			}
			return timeText;
		}

		#region implemented abstract members of TextInputInterpreter

		protected override bool InterpretNonNull (string input, string defaultValue, ref string interprettedInput, ref string message)
		{
			/*
			 * Cases:
			 * [HANDLED] Date only - just digits
			 * Date only - digits and separators
			 * 
			 * [HANDLED] Time only - just digits
			 * Time only - digits and separators, possibly AM/PM
			 * 
			 * Date and time - just digits and a space
			 * [HANDLED] Date and time - digits and separators and a space, possibly AM/PM
			 */
			input = input.Trim ();

			var now = DateTime.Now;
			if (_isDatePart) {
				if (Regex.IsMatch (input, @"^\d{1,8}$")) {
					input = InterpretRawIntegerDatePart (input);
				} else if (_isTimePart) {
					if (Regex.IsMatch (input, @"^\d{1,8}\s+\d{1,6}$")) {
						Match mtc = Regex.Match (input, @"^(?<date>\d+)\s+(?<time>\d+)$");
						if (mtc.Success) {
							var dateText = mtc.Groups ["date"].Value;
							var timeText = mtc.Groups ["time"].Value;

							dateText = InterpretRawIntegerDatePart (dateText);
							timeText = InterpretRawIntegerTimePart (timeText);
							input = String.Format ("{0} {1}", dateText, timeText);
						}
					}
				}
			} else if (_isTimePart) {
				if (Regex.IsMatch (input, @"^\d{1,6}$")) {
					input = InterpretRawIntegerTimePart (input);
				}
			}

			// TODO make this more robust
			DateTime temp;
			if (DateTime.TryParse (input, out temp)) {
				interprettedInput = temp.ToString (_formatString);
				return true;
			} else {
				message = String.Format ("Invalid Date/Time: '{0}'", input);
			}
			return false;
		}

		#endregion
	}

	/// <summary>
	/// Text input interpreter for Integer data types.
	/// </summary>
	public class TextInputInterpreterInteger : TextInputInterpreter
	{
		public TextInputInterpreterInteger ()
			: base ()
		{
		}

		public TextInputInterpreterInteger (bool isAllowNull)
			: base (isAllowNull)
		{
		}

		#region implemented abstract members of TextInputInterpreter

		protected override bool InterpretNonNull (string input, string defaultValue, ref string interprettedInput, ref string message)
		{
			input = input.Trim ();
			long result;
			if (Int64.TryParse (input, out result)) {
				interprettedInput = result.ToString ();
				return true;
			} else {
				message = String.Format ("Invalid Integer: '{0}'", input);
			}
			return false;
		}

		#endregion
	}

	/// <summary>
	/// Text input interpreter for .NET Double data types.
	/// </summary>
	public class TextInputInterpreterDouble : TextInputInterpreter
	{
		public TextInputInterpreterDouble ()
			: base ()
		{
		}

		public TextInputInterpreterDouble (bool isAllowNull)
			: base (isAllowNull)
		{
		}

		#region implemented abstract members of TextInputInterpreter

		protected override bool InterpretNonNull (string input, string defaultValue, ref string interprettedInput, ref string message)
		{
			input = input.Trim ();
			double result;
			if (Double.TryParse (input, out result)) {
				interprettedInput = result.ToString ();
				return true;
			} else {
				message = String.Format ("Invalid Decimal number: '{0}'", input);
			}
			return false;
		}

		#endregion
	}

	/// <summary>
	/// Text input interpreter for .NET Decimal data types.
	/// </summary>
	public class TextInputInterpreterDecimal : TextInputInterpreter
	{
		public TextInputInterpreterDecimal ()
			: base ()
		{
		}

		public TextInputInterpreterDecimal (bool isAllowNull)
			: base (isAllowNull)
		{
		}

		#region implemented abstract members of TextInputInterpreter

		protected override bool InterpretNonNull (string input, string defaultValue, ref string interprettedInput, ref string message)
		{
			input = input.Trim ();
			decimal result;
			if (Decimal.TryParse (input, out result)) {
				interprettedInput = result.ToString ();
				return true;
			} else {
				message = String.Format ("Invalid Decimal number: '{0}'", input);
			}
			return false;
		}

		#endregion
	}

	/// <summary>
	/// Text input interpreter for Boolean data types.
	/// </summary>
	public class TextInputInterpreterBoolean : TextInputInterpreter
	{
		public TextInputInterpreterBoolean ()
			: base ()
		{
		}

		public TextInputInterpreterBoolean (bool isAllowNull)
			: base (isAllowNull)
		{
		}

		#region implemented abstract members of TextInputInterpreter

		protected override bool InterpretNonNull (string input, string defaultValue, ref string interprettedInput, ref string message)
		{
			if (Regex.IsMatch (input, @"^([1yYtT]|yes)$", RegexOptions.IgnoreCase)) {
				interprettedInput = bool.TrueString;
				return true;
			} else if (Regex.IsMatch (input, @"^([0nNfF]|no)$", RegexOptions.IgnoreCase)) {
				interprettedInput = bool.FalseString;
				return true;
			}

			bool result;
			if (Boolean.TryParse (input, out result)) {
				interprettedInput = result.ToString ();
				return true;
			} else {
				message = String.Format ("Invalid True/False value: '{0}' (acceptable values: [ 1/0, y/n, yes/no, t/f, true/false ]; not case-sensitive)", input);
			}
			return false;
		}

		#endregion
	}
}

