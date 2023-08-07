using System;

namespace Dataelus.EDD
{
	using Processor;

	/// <summary>
	/// This is the main EDD processor class.
	/// </summary>
	public class EDDProcessor
	{
		ICSVParser _csvParser;
		IDatabaseUploaderSingle _uploaderSingle;
		IReaderSingleCSV _csvReader;
		IValidatorSimple _validatorSimple;

		public EDDProcessResult ProcessEDDSingle (System.IO.TextReader reader)
		{
			var result = new EDDProcessResult ();

			// Parse
			var fileLines = _csvParser.Parse (reader);

			// Read
			result.ReadResult = _csvReader.Read (fileLines);
			if (result.ReadResult.OverallResult == EDDOverallResultType.Success) {

				// Validate
				result.ValidationResult = _validatorSimple.Validate (result.ReadResult.FileData);
				if (result.ValidationResult.OverallResult == EDDOverallResultType.Success) {

					// Upload
					//result.UploadResult = _uploaderSingle.UploadBatch()
				}
			}

			return result;
		}

		public void ProcessEDD ()
		{
			
		}

		public EDDProcessor ()
		{
		}
	}
}

