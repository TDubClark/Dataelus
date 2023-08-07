using System;
using Dataelus.Table.Comparison.Generic;

namespace Dataelus.Table
{
	public class ObjectTableComparer : TableComparer<ObjectTable, ObjectRow>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Table.ObjectTableComparer"/> class.
		/// </summary>
		public ObjectTableComparer ()
		{
			this.Table1Iterator = new ObjectTableIterator ();
			this.Table2Iterator = new ObjectTableIterator ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Table.ObjectTableComparer"/> class.
		/// Uses a ObjectRowComparer as the default object;
		/// </summary>
		/// <param name="table1">Table1.</param>
		/// <param name="table2">Table2.</param>
		public ObjectTableComparer (ObjectTable table1, ObjectTable table2)
			: this (table1, table2, new ObjectRowComparer ())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Table.ObjectTableComparer"/> class.
		/// </summary>
		/// <param name="table1">Table1.</param>
		/// <param name="table2">Table2.</param>
		/// <param name="comparer">Comparer.</param>
		public ObjectTableComparer (ObjectTable table1, ObjectTable table2, IRecordEqualityComparer<ObjectRow> comparer)
			: this ()
		{
			this.Table1 = table1;
			this.Table2 = table2;
			this.RecordComparer = comparer;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Table.ObjectTableComparer"/> class.
		/// </summary>
		/// <param name="table1">Table1.</param>
		/// <param name="table2">Table2.</param>
		/// <param name="comparer">Comparer.</param>
		/// <param name="evaluator">Evaluator.</param>
		public ObjectTableComparer (ObjectTable table1, ObjectTable table2, IRecordEqualityComparer<ObjectRow> comparer, IRecordEvaluator<ObjectRow> evaluator)
			: this (table1, table2, comparer)
		{
			this.Evaluator = evaluator;
		}
	}

	/// <summary>
	/// Data column value comparer.
	/// </summary>
	public class ObjectColumnValueComparer
	{
		public string ColumnName1{ get; set; }

		public string ColumnName2{ get; set; }

		public System.Collections.Generic.IEqualityComparer<object> ValueComparer{ get; set; }

		public ObjectColumnValueComparer ()
		{

		}

		public ObjectColumnValueComparer (string column1, string column2, System.Collections.Generic.IEqualityComparer<object> valueComparer)
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
		public bool RowsEqual (ObjectRow row1, ObjectRow row2)
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
			return _stringComparer.Equals (NullableString (obj1), NullableString (obj2));
		}

		protected virtual string NullableString (object value)
		{
			return value == null ? null : value.ToString ();
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
			if (x == null || y == null) {
				// If Both are NULL
				if (x == null && y == null)
					return true;

				if (typeof(T).Equals (typeof(System.String))) {
					string strX = (x == null) ? "" : x.ToString ();
					string strY = (y == null) ? "" : y.ToString ();
				}
				return false;
			}

			// If they have different types
			if (!x.GetType ().Equals (y.GetType ())) { return false; }
			T xVal = (x == null) ? this.DefaultValue : (T)x;
			T yVal = (y == null) ? this.DefaultValue : (T)y;

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
	public class ObjectRowComparer : IRecordEqualityComparer<ObjectRow>, System.Collections.Generic.IEqualityComparer<ObjectRow>
	{
		public System.Collections.Generic.List<ObjectColumnValueComparer> ColumnComparers{ get; set; }

		public void AddColumnComparer (string column1, string column2, System.Collections.Generic.IEqualityComparer<object> valueComparer)
		{
			ColumnComparers.Add (new ObjectColumnValueComparer (column1, column2, valueComparer));
		}

		public void AddColumnStringComparer (string column1, string column2)
		{
			ColumnComparers.Add (new ObjectColumnValueComparer (column1, column2, new ObjectStringComparer ()));
		}

		public ObjectRowComparer ()
		{
			ColumnComparers = new System.Collections.Generic.List<ObjectColumnValueComparer> ();
		}

		#region IRecordEqualityComparer implementation

		public bool RecordEquals (ObjectRow table1Record, ObjectRow table2Record)
		{
			foreach (var ccmpr in ColumnComparers) {
				if (!ccmpr.RowsEqual (table1Record, table2Record)) { return false; }
			}
			return true;
		}

		#endregion

		#region IEqualityComparer implementation

		public bool Equals (ObjectRow x, ObjectRow y)
		{
			return RecordEquals (x, y);
		}

		public int GetHashCode (ObjectRow obj)
		{
			return obj.ObjectUniqueID.GetHashCode ();
		}

		#endregion
	}

	/// <summary>
	/// Standard DataTable iterator.
	/// </summary>
	public class ObjectTableIterator : ITableIterator<ObjectTable, ObjectRow>
	{
		protected int index;

		public ObjectTableIterator ()
		{
			index = -1;
		}

		#region ITableIterator implementation

		public void Start ()
		{
			index = -1;
		}

		public bool GetNextRecord (ObjectTable table, out ObjectRow record)
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

