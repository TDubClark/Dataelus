using System;

namespace Dataelus.EDD.Processor
{
	/// <summary>
	/// Interface for an object which handles uploading multi-table EDDs to the database.
	/// </summary>
	public interface IDatabaseUploader
	{
		/// <summary>
		/// Uploads the batches of data to the database.
		/// </summary>
		/// <returns>The upload result.</returns>
		/// <param name="fileData">File data.</param>
		/// <param name="eddBatchList">Edd batch list.</param>
		DatabaseUploaderResult UploadBatch (EDDFileUploadRecord fileData, EDDBatchCollection eddBatchList);
	}
}
