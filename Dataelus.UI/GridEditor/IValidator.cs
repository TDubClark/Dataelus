using System;

namespace Dataelus.UI.GridEditor
{
	/// <summary>
	/// Interface for a data object validator.
	/// </summary>
	public interface IValidator<D>
	{
		ValidationResult IsValidRecord (D data, int rowIndex);

		ValidationResult IsValid (D data);

		TableIssue.ICellIssueItemCollection GetIssues(D data);
	}

	/// <summary>
	/// Validation result.
	/// </summary>
	public class ValidationResult
	{
		public bool IsValid {
			get;
			set;
		}

		public string Message {
			get;
			set;
		}

		public ValidationResult ()
			: this (false, null)
		{
		}

		public ValidationResult (bool valid, string message)
		{
			this.IsValid = valid;
			this.Message = message;
		}
	}
}

