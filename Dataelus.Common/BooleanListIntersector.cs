using System;

namespace Dataelus.Generic
{
	/// <summary>
	/// Boolean list intersector.
	/// </summary>
	public class BooleanListIntersector<TRow, TColumn> : IntersectionBooleanBase<TRow, TColumn>
	{
		protected IBooleanIntersectList<TRow, TColumn> _collection;

		public IBooleanIntersectList<TRow, TColumn> Collection {
			get { return _collection; }
			set { _collection = value; }
		}

		public BooleanListIntersector (IBooleanIntersectList<TRow, TColumn> collection)
		{
			if (collection == null)
				throw new ArgumentNullException ("collection");
			this._collection = collection;
		}

		#region implemented abstract members of IntersectionBooleanBase

		protected override bool GetIntersect (TRow row, TColumn column)
		{
			return _collection.Contains (row, column);
		}

		protected override void SetIntersect (TRow row, TColumn column, bool value)
		{
			if (value) {
				if (!_collection.Contains (row, column)) {
					_collection.AddNew (row, column);
				}
			} else {
				_collection.Remove (row, column);
			}
		}

		#endregion
	}
}

