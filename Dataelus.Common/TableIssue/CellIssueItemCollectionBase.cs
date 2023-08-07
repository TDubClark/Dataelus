using System;

namespace Dataelus.TableIssue
{
	/// <summary>
	/// Issue item collection base.
	/// </summary>
	public class CellIssueItemCollectionBase : CollectionBase<ICellIssueItem>, ICellIssueItemCollection
	{
		public CellIssueItemCollectionBase ()
			: base ()
		{
		}

		public int FindIndexByObjectId (int objectID)
		{
			return _items.FindIndex (x => x.ItemID == objectID);
		}

		#region IEnumerable implementation

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}

		#endregion
	}
}

