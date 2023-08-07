using System;

namespace Dataelus.Table
{
	/// <summary>
	/// Record value collection.
	/// </summary>
	public class RecordValueCollection : CollectionBase<IRecordValue>, IRecordValueCollection
	{
		public RecordValueCollection ()
			: base ()
		{
		}

		public RecordValueCollection (int rowIndex)
			: base ()
		{
			_rowIndex = rowIndex;
		}

		public RecordValueCollection (int rowIndex, System.Collections.Generic.IEnumerable<IRecordValue> items)
			: base (items)
		{
			_rowIndex = rowIndex;
		}

		protected int _rowIndex;

		public int RowIndex {
			get { return _rowIndex; }
			set { _rowIndex = value; }
		}
	}
}

