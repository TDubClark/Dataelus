using System;

namespace Dataelus.TableIssue
{
	/// <summary>
	/// Base implementation of an issue item for an Object.
	/// </summary>
	public class ObjectIssueItemBase : IssueBase, IObjectIssueItem
	{
		public ObjectIssueItemBase ()
			: base ()
		{
		}

		public ObjectIssueItemBase (IIssue issue, object objectCode)
			: this (issue, objectCode, null)
		{
		}

		public ObjectIssueItemBase (IIssue issue, object objectCode, object fieldCode)
			: base (issue)
		{
			_objectCode = objectCode;
			_fieldCode = fieldCode;
		}

		#region IObjectIssueItem implementation

		protected object _objectCode;

		public object ObjectCode {
			get { return _objectCode; }
			set { _objectCode = value; }
		}

		protected object _fieldCode;

		public object FieldCode {
			get { return _fieldCode; }
			set { _fieldCode = value; }
		}

		#endregion
	}
}

