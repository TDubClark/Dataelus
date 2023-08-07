using System;

namespace Dataelus.EDD.Processor
{
	/// <summary>
	/// Interface for an object which handles uploading single-table EDDs to the database.
	/// </summary>
	public interface IDatabaseUploaderSingle
	{
		DatabaseUploaderResult UploadBatch (EDDFileUploadRecord fileData, EDDBatch eddBatch);
	}
}

