using System;

namespace Dataelus.TableIssue
{
	/// <summary>
	/// Represents a single issue for an Object (with an Object Code), for a single field (denoted by a Field Code).
	/// </summary>
	public interface IObjectIssueItem : IIssue
	{
		/// <summary>
		/// Gets or sets the object code.
		/// </summary>
		/// <value>The object code.</value>
		object ObjectCode{ get; set; }

		/// <summary>
		/// Gets or sets the field code.
		/// </summary>
		/// <value>The field code.</value>
		object FieldCode{ get; set; }
	}
}

