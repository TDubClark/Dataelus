using System;
using System.Collections.Generic;

namespace Dataelus.TableDisplay
{
	public class ColumnDefCollection : CollectionBase<IColumnDef>
	{
		public ColumnDefCollection ()
			: base ()
		{
		}

		public ColumnDefCollection (IEnumerable<IColumnDef> columns)
			: base (columns)
		{
		}

		public bool FindByColumnName<T> (string columnName, out T item) where T : IColumnDef
		{
			return FindByColumnName<T> (columnName, out item, new StringEqualityComparer ());
		}

		/// <summary>
		/// Finds the object by the name of the column.
		/// </summary>
		/// <returns><c>true</c>, if by column name was found, <c>false</c> otherwise.</returns>
		/// <param name="columnName">Column name.</param>
		/// <param name="item">Item.</param>
		/// <param name="comparer">Comparer.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public bool FindByColumnName<T> (string columnName, out T item, IEqualityComparer<string> comparer) where T : IColumnDef
		{
			item = default(T);
			int index = _items.FindIndex (x => comparer.Equals (x.columnName, columnName));
			if (index < 0)
				return false;

			item = (T)_items [index];
			return true;
		}


		/// <summary>
		/// Copies the values in this object to the given list of objects.
		/// </summary>
		/// <param name="columns">Columns.</param>
		public virtual void CopyTo<T> (List<T> values, IEqualityComparer<string> comparer) where T : IColumnDef
		{
			for (int i = 0; i < values.Count; i++) {
				var val = values [i];

				// Find the item in this list
				int index = _items.FindIndex (x => comparer.Equals (x.columnName, val.columnName));
				if (index >= 0) {
					var item = _items [index];

					var sval = val as IColumnDefSticky;
					if (sval == null) {
						val.copyFrom (item);
						values [i] = val;
					} else {
						var sitem = item as IColumnDefSticky;
						if (sitem == null) {
							sval.copyFrom (item);
						} else {
							sval.copyFrom (sitem);
						}
						values [i] = (T)sval;
					}
				}
			}
		}

		/// <summary>
		/// Copies the values in this object from the given list of objects.
		/// </summary>
		/// <param name="columns">Columns.</param>
		public virtual void LoadFrom<T> (List<T> values, IEqualityComparer<string> comparer) where T : IColumnDef
		{
			for (int i = 0; i < values.Count; i++) {
				IColumnDef srcVal = values [i];

				// Find the item in this list
				int index = _items.FindIndex (x => comparer.Equals (x.columnName, srcVal.columnName));

				var stkVal = srcVal as IColumnDefSticky;
				if (stkVal == null) {
					// Source item is just a regular column definition
					if (index >= 0) {
						var item = _items [index];
						item.copyFrom (srcVal);
						_items [index] = item;
					} else {
						_items.Add (new ColumnDef (srcVal));
					}
				} else {
					// Source column is sticky
					if (index >= 0) {
						var item = _items [index];
						var sitem = item as IColumnDefSticky;
						if (sitem == null) {
							// Not a sticky object, so create as a new sticky object
							sitem = new ColumnDefSticky (stkVal);
						} else {
							sitem.copyFrom (srcVal);
						}
						_items [index] = sitem;
					} else {
						_items.Add (new ColumnDefSticky (stkVal));
					}
				}
			}
		}

		public static ColumnDefCollection JsonDeserialize (string jsonString)
		{
			return (ColumnDefCollection)Newtonsoft.Json.JsonConvert.DeserializeObject (jsonString);
		}

		public static string JsonSerialize (ColumnDefCollection value)
		{
			return Newtonsoft.Json.JsonConvert.SerializeObject (value);
		}
	}
}

