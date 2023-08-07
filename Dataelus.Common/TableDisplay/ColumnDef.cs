using System;

namespace Dataelus.TableDisplay
{
	public class ColumnDef : IColumnDef
	{
		public ColumnDef ()
		{
			_columnName = null;
			_columnOrderIndex = -1;
			_columnVisible = false;
		}

		public ColumnDef (string columnName, int columnOrderIndex, bool columnVisible)
		{
			this._columnName = columnName;
			this._columnOrderIndex = columnOrderIndex;
			this._columnVisible = columnVisible;
		}


		public ColumnDef (IColumnDef other)
			: this ()
		{
			copyFrom (other);
		}

		#region IColumnDef implementation

		public void copyFrom (IColumnDef other)
		{
			this.columnName = other.columnName;
			this.columnOrderIndex = other.columnOrderIndex;
			this.columnVisible = other.columnVisible;
		}

		protected string _columnName;

		public string columnName {
			get { return _columnName; }
			set { _columnName = value; }
		}

		protected int _columnOrderIndex;

		public int columnOrderIndex {
			get { return _columnOrderIndex; }
			set { _columnOrderIndex = value; }
		}

		protected bool _columnVisible;

		public bool columnVisible {
			get { return _columnVisible; }
			set { _columnVisible = value; }
		}

		public int OrderIndex {
			get { return _columnOrderIndex; }
			set { _columnOrderIndex = value; }
		}

		public virtual bool Equals (IColumnDef other, System.Collections.Generic.IEqualityComparer<string> comparer)
		{
			return comparer.Equals (this.columnName, other.columnName);
		}

		#endregion

		#region IEquatable implementation

		public virtual bool Equals (IColumnDef other)
		{
			return Equals (other, new StringEqualityComparer ());
		}

		#endregion
	}
}

