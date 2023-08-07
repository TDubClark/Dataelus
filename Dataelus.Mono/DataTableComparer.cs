using System;
using System.Data;
using Dataelus.Table.Comparison.Generic;

using Dataelus.Mono.Extensions;

namespace Dataelus.Mono
{

	/// <summary>
	/// Data table comparer.
	/// </summary>
	public class DataTableComparer : TableComparer<DataTable, DataRow>
	{
		public DataTableComparer ()
			: base ()
		{
			this.Table1Iterator = new DataTableIterator ();
			this.Table2Iterator = new DataTableIterator ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.DataTableComparer"/> class.
		/// Uses a DataRowComparer as the default object;
		/// </summary>
		/// <param name="table1">Table1.</param>
		/// <param name="table2">Table2.</param>
		public DataTableComparer (DataTable table1, DataTable table2)
			: this (table1, table2, new DataRowComparer ())
		{
		}

		public DataTableComparer (DataTable table1, DataTable table2, IRecordEqualityComparer<DataRow> comparer)
			: this ()
		{
			this.Table1 = table1;
			this.Table2 = table2;
			this.RecordComparer = comparer;
		}

		public DataTableComparer (DataTable table1, DataTable table2, IRecordEqualityComparer<DataRow> comparer, IRecordEvaluator<DataRow> evaluator)
			: this (table1, table2, comparer)
		{
			this.Evaluator = evaluator;
		}
	}

	/// <summary>
	/// Data column value comparer.
	/// </summary>
	public class DataColumnValueComparer
	{
		public string ColumnName1{ get; set; }

		public string ColumnName2{ get; set; }

		public System.Collections.Generic.IEqualityComparer<object> ValueComparer{ get; set; }

		public DataColumnValueComparer ()
		{
			
		}

		public DataColumnValueComparer (string column1, string column2, System.Collections.Generic.IEqualityComparer<object> valueComparer)
			: this ()
		{
			this.ColumnName1 = column1;
			this.ColumnName2 = column2;
			this.ValueComparer = valueComparer;
		}

		/// <summary>
		/// Gets whether the given rows have equal values for the column names in this object.
		/// </summary>
		/// <param name="row1">Row1.</param>
		/// <param name="row2">Row2.</param>
		public bool RowsEqual (DataRow row1, DataRow row2)
		{
			return ValuesEqual (row1 [ColumnName1], row2 [ColumnName2]);
		}

		/// <summary>
		/// Valueses the equal.
		/// </summary>
		/// <returns><c>true</c>, if equal was valuesed, <c>false</c> otherwise.</returns>
		/// <param name="value1">Value1.</param>
		/// <param name="value2">Value2.</param>
		public bool ValuesEqual (object value1, object value2)
		{
			return ValueComparer.Equals (value1, value2);
		}
	}

	public class ObjectStringComparer : System.Collections.Generic.IEqualityComparer<object>
	{
		private StringEqualityComparer _stringComparer;

		public ObjectStringComparer ()
		{
			_stringComparer = new StringEqualityComparer ();
		}

		public new bool Equals (object obj1, object obj2)
		{
			return _stringComparer.Equals (obj1.ToNullableString (), obj2.ToNullableString ());
		}

		public int GetHashCode (object obj)
		{
			return ((string)obj).GetHashCode ();
		}
	}

	public class ObjectGenericComparer<T> : System.Collections.Generic.EqualityComparer<object>
	{
		public System.Collections.Generic.IEqualityComparer<T> Comparer {
			get;
			set;
		}

		public T DefaultValue {
			get;
			set;
		}

		public ObjectGenericComparer ()
			: this (null, default(T))
		{
		}

		public ObjectGenericComparer (System.Collections.Generic.IEqualityComparer<T> comparer, T defaultValue)
		{
			this.Comparer = comparer;
			if (comparer == null) {
				if (typeof(T).Equals (typeof(Byte))) {

				} else if (typeof(T).Equals (typeof(Int16))) {

				} else if (typeof(T).Equals (typeof(Int32))) {

				} else if (typeof(T).Equals (typeof(Int64))) {

				} else if (typeof(T).Equals (typeof(String))) {
					comparer = (System.Collections.Generic.IEqualityComparer<T>)(new StringEqualityComparer ());
				}
			}
			this.DefaultValue = defaultValue;
		}

		#region implemented abstract members of EqualityComparer

		public override int GetHashCode (object obj)
		{
			return obj.GetHashCode ();
		}

		public override bool Equals (object x, object y)
		{
			// If either are NULL
			if (x.ToNullable () == null || y.ToNullable () == null) {
				// If Both are NULL
				if (x.ToNullable () == null && y.ToNullable () == null)
					return true;

				if (typeof(T).Equals (typeof(System.String))) {
					string strX = x.ToNullable<string> ("");
					string strY = y.ToNullable<string> ("");
				}
				return false;
			}

			// If they have different types
			if (!x.GetType ().Equals (y.GetType ())) {
				return false;
			}
			T xVal = x.ToNullable<T> (this.DefaultValue);
			T yVal = y.ToNullable<T> (this.DefaultValue);

			if (Comparer == null) {
				return (xVal.Equals (yVal));
			}
			return Comparer.Equals (xVal, yVal);
		}

		#endregion
	}

	public class ObjectInt32Comparer : System.Collections.Generic.EqualityComparer<object>
	{
		public ObjectInt32Comparer ()
			: base ()
		{
		}

		#region implemented abstract members of EqualityComparer

		public override int GetHashCode (object obj)
		{
			throw new NotImplementedException ();
		}

		public override bool Equals (object x, object y)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}

	/// <summary>
	/// Data row comparer.
	/// </summary>
	public class DataRowComparer : IRecordEqualityComparer<DataRow>
	{
		public System.Collections.Generic.List<DataColumnValueComparer> ColumnComparers{ get; set; }

		public void AddColumnComparer (string column1, string column2, System.Collections.Generic.IEqualityComparer<object> valueComparer)
		{
			ColumnComparers.Add (new DataColumnValueComparer (column1, column2, valueComparer));
		}

		public void AddColumnStringComparer (string column1, string column2)
		{
			ColumnComparers.Add (new DataColumnValueComparer (column1, column2, new ObjectStringComparer ()));
		}

		public DataRowComparer ()
		{
			ColumnComparers = new System.Collections.Generic.List<DataColumnValueComparer> ();
		}

		public DataRowComparer (DataTable dt)
			: this ()
		{
			for (int col = 0; col < dt.Columns.Count; col++) {
				string colName = dt.Columns [col].ColumnName;
				// TODO Determine what comparer to add, according to column type
				//AddColumnComparer(colName, colName,  )
			}
		}

		#region IRecordEqualityComparer implementation

		public bool RecordEquals (DataRow table1Record, DataRow table2Record)
		{
			foreach (var ccmpr in ColumnComparers) {
				if (!ccmpr.RowsEqual (table1Record, table2Record)) {
					return false;
				}
			}
			return true;
		}

		#endregion
	}

	/// <summary>
	/// Standard DataTable iterator.
	/// </summary>
	public class DataTableIterator : ITableIterator<DataTable, DataRow>
	{
		protected int index;

		public DataTableIterator ()
		{
			index = -1;
		}

		#region ITableIterator implementation

		public void Start ()
		{
			index = -1;
		}

		public bool GetNextRecord (DataTable table, out DataRow record)
		{
			index++;
			if (index < table.Rows.Count) {
				record = table.Rows [index];
				return true;
			}
			record = null;
			return false;
		}

		public int CurrentIndex {
			get { return index; }
		}

		#endregion
	}
}

