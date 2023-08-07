using System;
using System.Collections.Generic;

namespace Dataelus.FilterCascade
{
	public class FilterObjectItemBase : FilterDataBase, IFilterObjectItem, ICodedHierarchyNode, IFilterItem
	{
		public FilterObjectItemBase ()
		{
		}

		#region IFilterObjectItem implementation

		protected string _displayColumnName;

		public string DisplayColumnName {
			get { return _displayColumnName; }
			set { _displayColumnName = value; }
		}

		protected ValueDisplay _displayBy;

		public ValueDisplay DisplayBy {
			get { return _displayBy; }
			set { _displayBy = value; }
		}

		protected bool _isSelectedAll;

		public bool IsSelectedAll {
			get { return _isSelectedAll; }
			set { _isSelectedAll = value; }
		}

		protected bool _allowSelectAll;

		public bool AllowSelectAll {
			get { return _allowSelectAll; }
			set { _allowSelectAll = value; }
		}

		protected int _minSelect;

		public int MinSelect {
			get { return _minSelect; }
			set { _minSelect = value; }
		}

		protected int _maxSelect;

		public int MaxSelect {
			get { return _maxSelect; }
			set { _maxSelect = value; }
		}

		#endregion

		#region ICodedHierarchyNode implementation

		string ICodedHierarchyNode.ItemCode {
			get { return _filterCode; }
			set { _filterCode = value; }
		}

		List<string> ICodedHierarchyNode.ParentItemCodes {
			get { return _parentFilterCodes; }
			set { _parentFilterCodes = value; }
		}

		#endregion

		#region IFilterTextItem implementation

		string[] IFilterItem.ParentFilterCodes { 
			get { return _parentFilterCodes.ToArray (); } 
			set { _parentFilterCodes = new List<string> (value); }
		}

		#endregion

		protected List<string> _parentFilterCodes;

		/// <summary>
		/// Gets or sets the filter codes for parent filters.
		/// </summary>
		/// <value>The parent filter codes.</value>
		public new List<string> ParentFilterCodes {
			get { return _parentFilterCodes; }
			set { _parentFilterCodes = value; }
		}

	}
}

