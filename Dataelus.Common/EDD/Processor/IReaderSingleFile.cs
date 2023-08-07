using System;

namespace Dataelus.EDD.Processor
{
	/// <summary>
	/// Interface for an EDD reader of a single file.
	/// </summary>
	public interface IReaderSingleFile
	{
		/// <summary>
		/// Reads the file (assigned elsewhere) and returns the EDD Read Result.
		/// </summary>
		EDDReadResult Read ();
	}



	public class ReaderSingleFileDefault : IReaderSingleFile
	{
		System.IO.TextReader _reader;

		public ReaderSingleFileDefault (System.IO.TextReader reader)
		{
			if (reader == null)
				throw new ArgumentNullException ("reader");
			_reader = reader;
		}

		#region IReaderSingleFile implementation

		public EDDReadResult Read ()
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

