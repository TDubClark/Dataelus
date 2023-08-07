using System;
using System.Collections.Generic;

namespace Dataelus.EDD.Processor
{
	public interface IReaderSingleCSV
	{
		/// <summary>
		/// Reads the CSV file (parsed into the given lines) and returns the EDD Read Result.
		/// </summary>
		EDDReadResult Read (List<string[]> fileLines);
	}
}

