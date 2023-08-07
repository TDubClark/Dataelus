using System;
using System.Collections.Generic;

namespace Dataelus.UI.TableColumnDisplay
{
	public class ControllerBase : IController
	{
		IEqualityComparer<string> _comparer;

		public ControllerBase ()
		{
			_comparer = new StringEqualityComparer ();
		}

		public void LoadView ()
		{
			LoadView (true);
		}

		public void LoadView (bool includeVisibleCheckbox)
		{
			_viewObject.InitializeWidgets (includeVisibleCheckbox);
			_viewObject.LoadData (_dataObject);
		}

		#region IController implementation

		protected Dataelus.TableDisplay.ColumnDefCollection _dataObject;

		public Dataelus.TableDisplay.ColumnDefCollection DataObject {
			get { return _dataObject; }
			set { _dataObject = value; }
		}

		public virtual void MoveColumn (string columnName, MoveDirection direction, int positionMoveCount)
		{
			MoveColumn (columnName, direction, positionMoveCount, false, false);
		}

		public void MoveColumn (string columnName, MoveDirection direction, int positionMoveCount, bool respectStickyPosition, bool respectColumnAdhesion)
		{
			int index = FindColumnIndexThrow (columnName);

			// Get the new index
			int newIndex = GetNewIndex (index, direction, positionMoveCount);

			// Store reference to the item
			var item = _dataObject [index];

			// Remove the item
			_dataObject.Items.RemoveAt (index);

			// Insert to new position
			item.columnOrderIndex = newIndex;
			_dataObject.Items.Insert (newIndex, item);

			// Re-index the columns, to reflect the new item in the proper position
			Dataelus.TableDisplay.ColumnSorterSticky<Dataelus.TableDisplay.IColumnDef>.ReindexColumns (_dataObject.Items);

			// Sort as necessary
			Dataelus.TableDisplay.ColumnSorterSticky<Dataelus.TableDisplay.IColumnDef>.Sort (_dataObject.Items, respectStickyPosition, respectColumnAdhesion);

			Dataelus.TableDisplay.IColumnDef outItem;
			if (_dataObject.FindByColumnName<Dataelus.TableDisplay.IColumnDef> (columnName, out outItem)) {
				newIndex = outItem.columnOrderIndex;
				positionMoveCount = GetNewMovePositionCount (index, direction, newIndex);
			}

			_viewObject.MoveColumn (columnName, direction, positionMoveCount);
		}

		/// <summary>
		/// Gets the new index, to which the object at [index] shall be moved.
		/// </summary>
		/// <returns>The new index.</returns>
		/// <param name="index">The index of the object</param>
		/// <param name="direction">Direction.</param>
		/// <param name="positionMoveCount">Position move count.</param>
		protected virtual int GetNewIndex (int index, MoveDirection direction, int positionMoveCount)
		{
			int newIndex = index;
			switch (direction) {
				case MoveDirection.Left:
					newIndex -= positionMoveCount;
					break;
				case MoveDirection.LeftMost:
					newIndex = 0;
					break;
				case MoveDirection.Right:
					newIndex += positionMoveCount;
					break;
				case MoveDirection.RightMost:
					newIndex = _dataObject.Count - 1;
					break;
				default:
					break;
			}
			if (newIndex > _dataObject.Count - 1)
				newIndex = _dataObject.Count - 1;
			if (newIndex < 0)
				newIndex = 0;

			return newIndex;
		}

		protected virtual int GetNewMovePositionCount (int index, MoveDirection direction, int newIndex)
		{
			switch (direction) {
				case MoveDirection.Left:
				// New index should be LOWER
					return (index - newIndex);
				case MoveDirection.LeftMost:
				// Doesn't matter - moving to the left end
					break;
				case MoveDirection.Right:
				// New index should be HIGHER
					return (newIndex - index);
				case MoveDirection.RightMost:
				// Doesn't matter - moving to the right end
					break;
				default:
					break;
			}
			return -1;
		}

		public virtual void ChangeColumnVisible (string columnName, bool isVisible)
		{
			int index = FindColumnIndexThrow (columnName);
			_dataObject.Items [index].columnVisible = isVisible;
			_viewObject.LoadDataItem (_dataObject.Items [index]);
		}

		public virtual int FindColumnIndex (string columnName)
		{
			return _dataObject.Items.FindIndex (x => _comparer.Equals (x.columnName, columnName));
		}

		protected virtual int FindColumnIndexThrow (string columnName)
		{
			int index = FindColumnIndex (columnName);
			if (index < 0)
				throw new ArgumentException (String.Format ("Column name '{0}' not found.", columnName), "columnName");
			return index;
		}

		protected IColumnDefView _viewObject;

		public IColumnDefView ViewObject {
			get { return _viewObject; }
			set { _viewObject = value; }
		}

		#endregion
	}
}

