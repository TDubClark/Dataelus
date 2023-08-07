using System;
using System.Collections.Generic;

namespace Dataelus.Table
{
	public class RowText : Generic.RowBase<string>
	{
		protected TextTable _parentTable;

		/// <summary>
		/// Gets the parent table.
		/// </summary>
		/// <value>The parent table.</value>
		[Newtonsoft.Json.JsonIgnore]
		[System.Xml.Serialization.XmlIgnore]
		public TextTable ParentTable {
			get { return _parentTable; }
		}

		#region implemented abstract members of RowBase

		protected override int FindColumnIndex (string columnName)
		{
			return ParentTable.FindColumnIndex (columnName);
		}

		protected override bool IsEditTracking ()
		{
			return _parentTable.IsTrackingEdits ();
		}

		protected override IEqualityComparer<string> GetEqualityComparer (int columnIndex)
		{
			return new StringEqualityComparer (new StringComparisonMethod (false, false, false));
		}

		protected override int GetColumnID (int columnIndex)
		{
			return _parentTable.Columns [columnIndex].ColumnID;
		}

		#endregion

		public RowText ()
			: base ()
		{
		}

		public RowText (int length)
			: base (length)
		{
			while (_values.Count < length) {
				_values.Add (null);
			}
		}

		public RowText (TextTable parent, int length)
			: base (length)
		{
			_parentTable = parent;
		}

		public RowText (TextTable parent, RowText item)
			: base (item)
		{
			_parentTable = parent;
		}
	}
}

