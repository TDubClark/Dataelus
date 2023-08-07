using System;

namespace Dataelus.EDD
{
	/// <summary>
	/// The results of processing an EDD (electronic data deliverable).
	/// </summary>
	public class EDDProcessResult
	{
		public EDDReadResult ReadResult { get; set; }

		public EDDValidationResult ValidationResult { get; set; }

		public EDDUploadResult UploadResult { get; set; }

		public EDDOverallResultType OverallResult { get; set; }

		public EDDProcessResult ()
		{
			OverallResult = EDDOverallResultType.Unknown;
		}
	}

	/// <summary>
	/// The results of reading an EDD.
	/// </summary>
	public class EDDReadResult
	{
		public EDDOverallResultType OverallResult { get; set; }

		public Table.ObjectTable FileData { get; set; }

		public EDDReadResult ()
		{
			OverallResult = EDDOverallResultType.Unknown;
		}
	}

	/// <summary>
	/// The results of validating an EDD.
	/// </summary>
	public class EDDValidationResult
	{
		public EDDOverallResultType OverallResult { get; set; }

		public EDDValidationResult ()
		{
			OverallResult = EDDOverallResultType.Unknown;
		}
	}

	/// <summary>
	/// The results of uploading an EDD.
	/// </summary>
	public class EDDUploadResult
	{
		public EDDOverallResultType OverallResult { get; set; }

		public EDDUploadResult ()
		{
			OverallResult = EDDOverallResultType.Unknown;
		}
	}

	/// <summary>
	/// Types of overall results for processing EDDs.
	/// </summary>
	public enum EDDOverallResultType
	{
		/// <summary>
		/// The EDD succeeded.
		/// </summary>
		Success,

		/// <summary>
		/// The EDD failed.
		/// </summary>
		EDDFailure,

		/// <summary>
		/// There was a warning.
		/// </summary>
		Warning,

		/// <summary>
		/// An Exception was thrown by the application.
		/// </summary>
		Exception,

		/// <summary>
		/// Unknown result.
		/// </summary>
		Unknown
	}
}

