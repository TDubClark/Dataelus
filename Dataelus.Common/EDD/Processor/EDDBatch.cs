using System;

namespace Dataelus.EDD.Processor
{
	/// <summary>
	/// A batched EDD for upload.
	/// </summary>
	public class EDDBatch
	{
		public EDDFileUploadRecord FileData { get; set; }

		public object Table { get; set; }

		public EDDColumnMapCollection ColumnMap { get; set; }
	}
	
	public class EDDBatchCollection : ListBase<EDDBatch>
	{

	}
}
