using System;

namespace Dataelus.EDD.Processor
{
	/// <summary>
	/// The result from uploading a batch of data using a Database uploader.
	/// </summary>
	public class DatabaseUploaderResult
	{
		/// <summary>
		/// Gets or sets the overall result.
		/// </summary>
		/// <value>The overall result.</value>
		public EDDOverallResultType OverallResult { get; set; }

		/// <summary>
		/// Gets or sets the error.
		/// </summary>
		/// <value>The error.</value>
		public Exception Error { get; set; }

		/// <summary>
		/// Gets or sets the batch results.
		/// </summary>
		/// <value>The batch results.</value>
		public EDDBatchResultCollection BatchResults { get; set; }
	}
}
