using System;

namespace Dataelus.FilterCascade
{
	/// <summary>
	/// Boolean data filter.
	/// </summary>
	public class FilterDataBoolean : FilterDataBase, IFilterItem, ICodedHierarchyNode
	{
		protected bool _isTrue;

		public bool IsTrue {
			get { return _isTrue; }
			set { _isTrue = value; }
		}

		protected bool _isFalse;

		public bool IsFalse {
			get { return _isFalse; }
			set { _isFalse = value; }
		}

		protected bool[] _selectableValues;

		/// <summary>
		/// Gets or sets the selectable values.
		/// </summary>
		/// <value>The selectable values.</value>
		public bool[] SelectableValues {
			get {
				return _selectableValues; }
			set { _selectableValues = value;
			}
		}

		protected bool _isAllowBoth;

		/// <summary>
		/// Gets or sets a value indicating whether to allow both True and False to be selected.
		/// </summary>
		/// <value><c>true</c> if this instance is allow both; otherwise, <c>false</c>.</value>
		public bool IsAllowBoth {
			get { return _isAllowBoth; }
			set { _isAllowBoth = value;
			}
		}

		protected bool _isAllowNeither;

		/// <summary>
		/// Gets or sets a value indicating whether to allow neither True nor False to be selected.
		/// </summary>
		/// <value><c>true</c> if this instance is allow neither; otherwise, <c>false</c>.</value>
		public bool IsAllowNeither {
			get { return _isAllowNeither; }
			set { _isAllowNeither = value; }
		}

		public FilterDataBoolean ()
			: base ()
		{
			_selectableValues = new bool[]{ true, false };

			_dataTypeClass = Dataelus.Table.TypeClass.Boolean;
		}
	}
}

