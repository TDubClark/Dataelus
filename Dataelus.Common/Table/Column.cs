using System;

namespace Dataelus.Table
{
	/// <summary>
	/// Filter column.
	/// </summary>
	public class Column : IEquatable<Column, String>, IEquatable<Column>
	{
		protected string _columnName;

		/// <summary>
		/// Gets or sets the name of the column.
		/// </summary>
		/// <value>The name of the column.</value>
		public string ColumnName {
			get { return _columnName; }
			set { _columnName = value; }
		}

		protected int _columnIndex;

		/// <summary>
		/// Gets or sets the index of the column.
		/// </summary>
		/// <value>The index of the column.</value>
		public int ColumnIndex {
			get { return _columnIndex; }
			set { _columnIndex = value; }
		}

		protected int _columnId;

		/// <summary>
		/// Gets or sets the unique column ID.
		/// </summary>
		/// <value>The column ID.</value>
		public int ColumnID {
			get { return _columnId; }
			set { _columnId = value; }
		}

		protected object _CustomValueTag;

		/// <summary>
		/// Gets or sets the custom value tag - a custom value associated with this column.
		/// </summary>
		/// <value>The custom value tag.</value>
		public object CustomValueTag {
			get { return _CustomValueTag; }
			set { _CustomValueTag = value; }
		}

		public Column ()
			: this (null, -1)
		{
		}

		public Column (string columnName)
			: this (columnName, -1)
		{
		}

		public Column (string columnName, int index)
			: this (columnName, index, -1)
		{
		}

		public Column (string columnName, int index, int columnId)
		{
			_columnName = columnName;
			_columnIndex = index;
			_columnId = columnId;
		}

		public Column (Column item)
			: this (item.ColumnName, item.ColumnIndex)
		{
		}

		#region IEquatable implementation

		/// <summary>
		/// Determines whether the specified <see cref="Dataelus.Table.Column"/> is equal to the current <see cref="Dataelus.Table.Column"/>.
		/// </summary>
		/// <param name="other">The <see cref="Dataelus.Table.Column"/> to compare with the current <see cref="Dataelus.Table.Column"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="Dataelus.Table.Column"/> is equal to the current
		/// <see cref="Dataelus.Table.Column"/>; otherwise, <c>false</c>.</returns>
		public virtual bool Equals (Column other)
		{
			return Equals (other, new StringEqualityComparer ());
		}

		#endregion

		/// <summary>
		/// Determines whether the specified <see cref="Dataelus.Table.Column"/> is equal to the current <see cref="Dataelus.Table.Column"/>.
		/// </summary>
		/// <param name="other">Other.</param>
		/// <param name="comparer">The column name comparer.</param>
		public virtual bool Equals (Column other, System.Collections.Generic.IEqualityComparer<string> comparer)
		{
			return Equals (other, true, true, comparer);
		}

		/// <summary>
		/// Determines whether the specified <see cref="Dataelus.Table.Column"/> is equal to the current <see cref="Dataelus.Table.Column"/>.
		/// </summary>
		/// <param name="other">Other.</param>
		/// <param name="matchIndex">If set to <c>true</c> match the index.</param>
		/// <param name="matchID">If set to <c>true</c> match the ID.</param>
		/// <param name="comparer">Comparer.</param>
		public virtual bool Equals (Column other, bool matchIndex, bool matchID, System.Collections.Generic.IEqualityComparer<string> comparer)
		{
			if (!comparer.Equals (this.ColumnName, other.ColumnName)) {
				return false;
			}
			if (matchIndex) {
				if (this.ColumnIndex != other.ColumnIndex)
					return false;
			}
			if (matchID) {
				if (this.ColumnID != other.ColumnID)
					return false;
			}
			return true;
		}
	}
}

