using System;
using Dataelus.Collections;

namespace Dataelus.TableDisplay
{
	public class ColumnDefSticky : ColumnDef, IColumnDefSticky
	{
		public ColumnDefSticky ()
			: base ()
		{
			Init ();
		}

		public ColumnDefSticky (string columnName, int columnOrderIndex, bool columnVisible)
			: base (columnName, columnOrderIndex, columnVisible)
		{
			Init ();
		}

		public ColumnDefSticky (IColumnDef other)
			: this ()
		{
			copyFrom (other);
		}

		public ColumnDefSticky (IColumnDefSticky other)
			: this ()
		{
			copyFrom (other);
		}

		void Init ()
		{
			_isSticky = false;
			_stickinessObject = StickyTo.AbsolutePosition;
			_targetColumnName = null;
			_absolutePosition = -1;
			_priorityIndex = -1;
		}

		protected object _targetItem;

		public object targetItem {
			get { return _targetItem; }
			set { _targetItem = value; }
		}

		public int OrderIndex {
			get { return _columnOrderIndex; }
			set { _columnOrderIndex = value; }
		}

		public void CopyFrom (IOrderedListItemSticky other)
		{
			this.isSticky = other.isSticky;
			this.stickinessObject = other.stickinessObject;
			this.targetItem = other.targetItem;
			this.absolutePosition = other.absolutePosition;
			this.priorityIndex = other.priorityIndex;
		}

		#region IColumnDefSticky implementation

		public void copyFrom (IColumnDefSticky other)
		{
			base.copyFrom (other);
			this.isSticky = other.isSticky;
			this.stickinessObject = other.stickinessObject;
			this.targetColumnName = other.targetColumnName;
			this.absolutePosition = other.absolutePosition;
			this.priorityIndex = other.priorityIndex;
		}

		protected bool _isSticky;

		public bool isSticky {
			get { return _isSticky; }
			set { _isSticky = value; }
		}

		protected StickyTo _stickinessObject;

		public StickyTo stickinessObject {
			get { return _stickinessObject; }
			set { _stickinessObject = value; }
		}

		protected string _targetColumnName;

		public string targetColumnName {
			get { return _targetColumnName; }
			set { _targetColumnName = value; }
		}

		protected int _absolutePosition;

		public int absolutePosition {
			get { return _absolutePosition; }
			set { _absolutePosition = value; }
		}

		protected int _priorityIndex;

		public int priorityIndex {
			get { return _priorityIndex; }
			set { _priorityIndex = value; }
		}

		#endregion
	}
}

