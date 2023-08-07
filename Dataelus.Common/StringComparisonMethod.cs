using System;

namespace Dataelus
{
	/// <summary>
	/// String comparison method.
	/// </summary>
	public class StringComparisonMethod : IStringComparisonMethod
	{
		/// <summary>
		/// Gets the comparable string, according to the given comparison method
		/// </summary>
		/// <returns>The comparable.</returns>
		/// <param name="value">Value.</param>
		/// <param name="method">Method.</param>
		public static string GetComparable (string value, IStringComparisonMethod method)
		{
			if (value != null) {
				// Trim first
				if (method.Trim) {
					value = value.Trim ();
				}
				if (method.NullEqualsEmpty) {
					if (String.IsNullOrEmpty (value))
						value = null;
				}
				if (value != null) {
					if (method.IgnoreCase) {
						value = value.ToLower ();
					}
					if (method.IgnoreWhiteSpace) {
						value = System.Text.RegularExpressions.Regex.Replace (value, @"\s+", "");
					}
					if (method.AlphaNumericMatch) {
						value = System.Text.RegularExpressions.Regex.Replace (value, @"\W+", "");
					}
				}
			}

			return value;
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.StringComparisonMethod"/> class.
		/// Defaults: Ignore Case, Trim, Null==Empty
		/// </summary>
		public StringComparisonMethod ()
		{
			_ignoreCase = true;
			_trim = true;
			_nullEqualsEmpty = true;
			_alphaNumericMatch = false;
			_ignoreWhiteSpace = false;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.StringComparisonMethod"/> class.
		/// Defaults: Null==Empty
		/// </summary>
		/// <param name="ignoreCase">If set to <c>true</c> ignore case.</param>
		/// <param name="isTrim">If set to <c>true</c> is trim.</param>
		public StringComparisonMethod (bool ignoreCase, bool isTrim)
			: this ()
		{
			_ignoreCase = ignoreCase;
			_trim = isTrim;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.StringComparisonMethod"/> class.
		/// </summary>
		/// <param name="ignoreCase">If set to <c>true</c> ignore case.</param>
		/// <param name="isTrim">If set to <c>true</c> is trim.</param>
		/// <param name="nullEqEmpty">If set to <c>true</c> null eq empty.</param>
		public StringComparisonMethod (bool ignoreCase, bool isTrim, bool nullEqEmpty)
			: this (ignoreCase, isTrim)
		{
			_nullEqualsEmpty = nullEqEmpty;
		}

		#region IStringComparisonMethod implementation

		public string GetComparableString (string value)
		{
			return GetComparable (value, this);
		}

		protected bool _ignoreCase;

		/// <summary>
		/// Gets or sets whether to ignore case.
		/// </summary>
		/// <value>true</value>
		/// <c>false</c>
		public bool IgnoreCase {
			get { return _ignoreCase; }
			set { _ignoreCase = value; }
		}

		protected bool _trim;

		/// <summary>
		/// Gets or sets whether to trim any value prior to compare.
		/// </summary>
		/// <value>true</value>
		/// <c>false</c>
		public bool Trim {
			get { return _trim; }
			set { _trim = value; }
		}

		protected bool _nullEqualsEmpty;

		/// <summary>
		/// Gets or sets whether to consider Null equal to an empty string.
		/// </summary>
		/// <value>true</value>
		/// <c>false</c>
		public bool NullEqualsEmpty {
			get { return _nullEqualsEmpty; }
			set { _nullEqualsEmpty = value; }
		}

		protected bool _ignoreWhiteSpace;

		/// <summary>
		/// Gets or sets whether to ignore all white-space during comparison.
		/// </summary>
		/// <value>true</value>
		/// <c>false</c>
		public bool IgnoreWhiteSpace {
			get { return _ignoreWhiteSpace; }
			set { _ignoreWhiteSpace = value; }
		}

		protected bool _alphaNumericMatch;

		/// <summary>
		/// Gets or sets whether to compare only alpha-numeric characters.
		/// </summary>
		/// <value>true</value>
		/// <c>false</c>
		public bool AlphaNumericMatch {
			get { return _alphaNumericMatch; }
			set { _alphaNumericMatch = value; }
		}

		#endregion
	}
}

