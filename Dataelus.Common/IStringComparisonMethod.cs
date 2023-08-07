using System;

namespace Dataelus
{
	/// <summary>
	/// Interface for a string comparison method.
	/// </summary>
	public interface IStringComparisonMethod
	{
		/// <summary>
		/// Gets or sets whether to ignore case.
		/// </summary>
		/// <value><c>true</c> if ignore case; otherwise, <c>false</c>.</value>
		bool IgnoreCase{ get; set; }

		/// <summary>
		/// Gets or sets whether to trim any value prior to compare.
		/// </summary>
		/// <value><c>true</c> if trim; otherwise, <c>false</c>.</value>
		bool Trim{ get; set; }

		/// <summary>
		/// Gets or sets whether to consider Null equal to an empty string.
		/// </summary>
		/// <value><c>true</c> if null equals empty; otherwise, <c>false</c>.</value>
		bool NullEqualsEmpty{ get; set; }

		/// <summary>
		/// Gets or sets whether to ignore all white-space during comparison.
		/// </summary>
		/// <value><c>true</c> if ignore white space; otherwise, <c>false</c>.</value>
		bool IgnoreWhiteSpace{ get; set; }

		/// <summary>
		/// Gets or sets whether to compare only alpha-numeric characters.
		/// </summary>
		/// <value><c>true</c> if alpha numeric match; otherwise, <c>false</c>.</value>
		bool AlphaNumericMatch{ get; set; }

		/// <summary>
		/// Gets the comparable string, after all of the settings have been applied.
		/// </summary>
		/// <returns>The comparable string.</returns>
		/// <param name="value">Value.</param>
		string GetComparableString (string value);
	}
}

