using System;

namespace Dataelus.TableIssue
{
	/// <summary>
	/// Base implementation for IIssue.
	/// </summary>
	public class IssueBase : IIssue
	{
		public IssueBase ()
			: this (-1, null, null)
		{
		}

		public IssueBase (int itemId, string category, string description)
		{
			_itemID = itemId;
			_category = category;
			_description = description;
		}

		public IssueBase (IIssue other)
			: this (other.ItemID, other.Category, other.Description)
		{
		}

		#region IIssue implementation

		protected int _itemID;

		public int ItemID {
			get { return _itemID; }
			set { _itemID = value; }
		}

		protected string _category;

		public string Category {
			get { return _category; }
			set { _category = value; }
		}

		protected string _description;

		public string Description {
			get {
				return _description; }
			set { _description = value;
			}
		}

		#endregion
	}
}

