using System;

namespace Dataelus.EDD.Processor
{
	/// <summary>
	/// The result of an EDD batch.
	/// </summary>
	public class EDDBatchResult
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
		/// Gets or sets the EDD batch object.
		/// </summary>
		/// <value>The batch.</value>
		public EDDBatch Batch { get; set; }
	}

	public class EDDBatchResultCollection : ListBase<EDDBatchResult>
	{

	}
}
