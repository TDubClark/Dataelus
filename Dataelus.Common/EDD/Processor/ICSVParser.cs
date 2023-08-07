using System;

namespace Dataelus.EDD.Processor
{
	/// <summary>
	/// Interface for a CSV file parser.
	/// </summary>
	public interface ICSVParser
	{
		/// <summary>
		/// Parse the specified reader into a list of lines, where each line is represented by an array of cells.
		/// </summary>
		/// <param name="reader">Reader.</param>
		System.Collections.Generic.List<string[]> Parse (System.IO.TextReader reader);
	}
}

