using System;

namespace Dataelus.Generic
{
	public class IntegerListIntersector<TRow, TColumn> : IntersectionIntegerBase<TRow, TColumn>
	{
		IIntegerIntersectList<TRow, TColumn> _collection;

		public IntegerListIntersector (IIntegerIntersectList<TRow, TColumn> collection)
		{
			this._collection = collection;
		}

		#region implemented abstract members of IntersectionIntegerBase

		protected override int GetIntersect (TRow row, TColumn column)
		{
			return _collection.GetInteger (row, column);
		}

		protected override void SetIntersect (TRow row, TColumn column, int value)
		{
			_collection.SetInteger (row, column, value);
		}

		#endregion
	}
}

