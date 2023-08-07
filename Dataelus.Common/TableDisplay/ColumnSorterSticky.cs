using System;
using System.Linq;
using System.Collections.Generic;
using Dataelus.Collections;

namespace Dataelus.TableDisplay
{
	/// <summary>
	/// Column sorter, which respects stickiness of columns (does not handle stickiness to eachother).
	/// </summary>
	public class ColumnSorterSticky<T> : OrderedListItemStickyComparer<T> where T : IColumnDef
	{
		public ColumnSorterSticky ()
			: base (true)
		{
		}

		public ColumnSorterSticky (bool isSticky)
			: base (isSticky)
		{
		}

		public int Compare (IColumnDef x, IColumnDef y)
		{
			return base.Compare (x, y);
		}

		/// <summary>
		/// Sort the specified lstItems, optionally respecting stickiness.
		/// </summary>
		/// <param name="lstItems">Lst items.</param>
		/// <param name="respectStickyPosition">Whether to respect preference of a column for a given absolute position.</param>
		/// <param name="respectItemAdhesion">If set to <c>true</c> to respect item adhesion. Whether to respect adhesion of one item to another.</param>
		public static void Sort (List<T> lstItems, bool respectStickyPosition, bool respectItemAdhesion)
		{
			Sort (lstItems, respectStickyPosition, respectItemAdhesion, new StringEqualityComparer ());
		}

		public static void Sort (List<T> lstItems, bool respectStickyPosition, bool respectItemAdhesion, IEqualityComparer<string> comparer)
		{
			lstItems.Sort (new ColumnSorterSticky<T> (respectStickyPosition));
			if (respectItemAdhesion)
				SortByColumnAdhesion (lstItems, comparer);
			ReindexColumns (lstItems);
		}

		/// <summary>
		/// Sorts the List by column adhesion.
		/// </summary>
		/// <param name="lstItems">List items.</param>
		public static void SortByColumnAdhesion (List<T> lstItems, IEqualityComparer<string> comparer)
		{
			// Gets the index of every item which is sticky to another column
			var indexes = FindIndexAll (lstItems, x => {
					var xs = x as IColumnDefSticky;
					if (xs == null)
						return false;
					return (xs.stickinessObject == StickyTo.OtherObject);
				});

			var lstIndexes = new List<int> (indexes);
			lstIndexes.Sort (new IntComparerReverse ());


			var lstSticky = new List<T> ();
			// Remove each of these items from the list
			foreach (var index in lstIndexes) {
				var obj = lstItems [index] as IColumnDefSticky;

				if (obj != null) {
					lstSticky.Add (lstItems [index]);
					lstItems.RemoveAt (index);
				} else {
					// Error - this should not happen
				}
			}

			// Get the unique list of columns to which other columns wish to adhere
			var targetColumns = lstSticky.Select (x => ((IColumnDefSticky)x).targetColumnName).Distinct ().ToList ();
			foreach (var targetColumn in targetColumns) {
				var stickyColumns = lstSticky.Where (x => comparer.Equals (((IColumnDefSticky)x).targetColumnName, targetColumn)).ToList ()
					.OrderBy (c => ((IColumnDefSticky)c).priorityIndex)
					.ToList ();
				int index = lstItems.FindIndex (x => comparer.Equals (x.columnName, targetColumn));
				if (index < 0)
					continue;
				lstItems.InsertRange (index + 1, stickyColumns);
			}
		}
	}
}

