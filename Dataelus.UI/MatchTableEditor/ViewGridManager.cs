using System;
using System.Linq;

namespace Dataelus.UI.MatchTableEditor
{
	/// <summary>
	/// View grid item (associates a row or column index with a unique ID).
	/// </summary>
	public class ViewGridItem
	{
		public int ViewGridIndex { get; set; }

		public long ObjectID { get; set; }

		public ViewGridItem ()
		{
			
		}

		public ViewGridItem (int viewGridIndex, long objectID)
		{
			this.ViewGridIndex = viewGridIndex;
			this.ObjectID = objectID;
		}
	}

	/// <summary>
	/// View grid collection.
	/// </summary>
	public class ViewGridCollection : ListBase<ViewGridItem>
	{
		public ViewGridCollection ()
		{
		}

		void CheckUnique (ViewGridItem item)
		{
			if (item == null)
				throw new ArgumentNullException ("item");
			if (_items.Any (x => x.ObjectID == item.ObjectID))
				throw new ArgumentException (String.Format ("Item with object ID [{0:d}] already exists.", item.ObjectID));
			if (_items.Any (x => x.ViewGridIndex == item.ViewGridIndex))
				throw new ArgumentException (String.Format ("Item with View Grid Index [{0:d}] already exists.", item.ViewGridIndex));
		}

		public void AddNew (int viewGridIndex, long objectID)
		{
			Add (new ViewGridItem (viewGridIndex, objectID));
		}

		public override void Add (ViewGridItem item)
		{
			CheckUnique (item);
			base.Add (item);
		}

		public override void Insert (int index, ViewGridItem item)
		{
			CheckUnique (item);
			base.Insert (index, item);
		}

		public int GetViewGridIndex (long objectID)
		{
			ViewGridItem item;
			if (TryGetItem (x => x.ObjectID == objectID, out item)) {
				return item.ViewGridIndex;
			}

			throw new Exception (String.Format ("No item found with object ID [ {0:d} ]", objectID));
		}

		public long GetObjectID (int viewGridIndex)
		{
			ViewGridItem item;
			if (TryGetItem (x => x.ViewGridIndex == viewGridIndex, out item)) {
				return item.ObjectID;
			}

			throw new Exception (String.Format ("No item found with ViewGridIndex [ {0:d} ]", viewGridIndex));
		}
	}

	/// <summary>
	/// View grid manager.
	/// </summary>
	public class ViewGridManager
	{
		public ViewGridCollection ViewRows { get; set; }

		public ViewGridCollection ViewColumns { get; set; }

		public ViewGridManager ()
		{
			this.ViewRows = new ViewGridCollection ();
			this.ViewColumns = new ViewGridCollection ();
		}
	}
}

