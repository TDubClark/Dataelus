using System;

namespace Dataelus.Grid
{
	using Dataelus.Search;

	/// <summary>
	/// Grid cell text collection.
	/// </summary>
	public class GridCellTextCollection : Dataelus.CollectionBase<GridCellText>
	{
		public void SortByRow ()
		{
			_items.Sort ((x, y) => x.RowIndex.CompareTo (y.RowIndex));
		}

		/// <summary>
		/// Gets a new copy, with the tranform applied.
		/// </summary>
		/// <returns>The transform.</returns>
		/// <param name="transformer">Transformer.</param>
		public GridCellTextCollection GetTransform (ITextTransformer transformer)
		{
			var coll = new GridCellTextCollection ();

			foreach (var item in _items) {
				var newCell = new GridCellText (item);
				newCell.Text = transformer.Transform (newCell.Text);
				coll.Add (newCell);
			}

			return coll;
		}

		public ISearchStatus Search (ISearchLevel level, ISearchStatus status)
		{
			throw new NotImplementedException ();
		}

		public ISearchStatus SearchNext (ISearchLevel level, ISearchStatus status)
		{
			throw new NotImplementedException ();
		}
	}
}

