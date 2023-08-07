using System;
using System.Collections.Generic;

namespace Dataelus.FilterCascade
{
	/// <summary>
	/// Base implementation of a non-text filter
	/// </summary>
	public abstract class NonTextFilter : INonTextFilter
	{
		protected NonTextFilter ()
			: this (Dataelus.Table.TypeClass.Object)
		{
		}


		protected NonTextFilter (Dataelus.Table.TypeClass dataTypeClass)
		{
			_parentItemCodes = new List<string> ();
			_dataTypeClass = dataTypeClass;
			_dataType = Dataelus.Table.ObjectColumn.GetType (dataTypeClass); // typeof(System.Object);
		}

		#region IFilterItem implementation

		protected string _filterCode;

		/// <summary>
		/// Gets or sets the filter code.
		/// This is the unique Identifier of this filter.
		/// </summary>
		/// <value>The filter code.</value>
		public string FilterCode {
			get { return _filterCode; }
			set { _filterCode = value; }
		}

		public string[] ParentFilterCodes {
			get { return _parentItemCodes.ToArray (); }
			set { _parentItemCodes = new List<string> (value); }
		}

		protected string _columnName;

		public string ColumnName {
			get { return _columnName; }
			set { _columnName = value; }
		}

		#endregion

		#region INonTextFilter implementation

		protected Dataelus.Table.TypeClass _dataTypeClass;

		public Dataelus.Table.TypeClass DataTypeClass {
			get { return _dataTypeClass; }
			set { _dataTypeClass = value; }
		}

		protected Type _dataType;

		public Type DataType {
			get { return _dataType; }
			set { _dataType = value; }
		}

		#endregion

		#region ICodedHierarchyNode implementation

		public string ItemCode {
			get { return _filterCode; }
			set { _filterCode = value; }
		}

		protected List<string> _parentItemCodes;

		public List<string> ParentItemCodes {
			get { return _parentItemCodes; }
			set { _parentItemCodes = value; }
		}

		#endregion
	}
}

