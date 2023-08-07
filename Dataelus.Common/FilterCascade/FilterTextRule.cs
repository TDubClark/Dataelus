using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.FilterCascade
{
	/// <summary>
	/// Filter text rule: a rule for filtering data from a Text Table.
	/// </summary>
	public class FilterTextRule : IFilterData
	{
		public FilterTextRule ()
		{
			_selectedValue = new List<string> ();
		}

		public FilterTextRule (IFilterData other)
			: this ()
		{
			CopyFrom (other);
		}

		protected string _filterCode;

		public string FilterCode {
			get { return _filterCode; }
			set { _filterCode = value; }
		}

		protected IEqualityComparer<string> _valueComparer;

		public IEqualityComparer<string> ValueComparer {
			get { return _valueComparer; }
			set { _valueComparer = value; }
		}

		protected List<string> _selectedValue;

		public List<string> SelectedValues {
			get {
				return _selectedValue; }
			set { _selectedValue = value;
			}
		}

		SelectionMode _mode;

		public SelectionMode Mode {
			get { return _mode; }
			set { _mode = value; }
		}

		int _selectionCountMin;

		public int SelectionCountMin {
			get { return _selectionCountMin; }
			set { _selectionCountMin = value; }
		}

		int _selectionCountMax;

		public int SelectionCountMax {
			get { return _selectionCountMax; }
			set { _selectionCountMax = value; }
		}

		public void CopyFrom (IFilterData other)
		{
			this.FilterCode = other.FilterCode;
			this.SelectedValues = new List<string> (other.SelectedValues);
			this.Mode = other.Mode;
			this.SelectionCountMax = other.SelectionCountMax;
			this.SelectionCountMin = other.SelectionCountMin;
		}

		public bool Equals (IFilterData other, IEqualityComparer<string> comparer)
		{
			return comparer.Equals (this.FilterCode, other.FilterCode);
		}

		public bool Equals (IFilterData other)
		{
			return Equals (other, new StringEqualityComparer ());
		}


		protected Func<Table.RowText, string> _valueGetter;

		/// <summary>
		/// Gets or sets the value getter.
		/// This is a function for creating a single String value from a Table RowText object
		/// </summary>
		/// <value>The value getter.</value>
		public Func<Table.RowText, string> ValueGetter {
			get { return _valueGetter; }
			set { _valueGetter = value; }
		}

		/// <summary>
		/// Gets the row filter predicate.
		/// </summary>
		/// <returns>The row filter.</returns>
		public virtual Predicate<Table.RowText> GetRowFilter ()
		{
			return x => SelectedValues.Contains (ValueGetter (x), ValueComparer);
		}
	}
}

