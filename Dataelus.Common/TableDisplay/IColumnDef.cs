using System;

namespace Dataelus.TableDisplay
{
	public interface IColumnDef : IEquatable<IColumnDef>, Collections.IOrderedListItem
	{
		/// <summary>
		/// Gets or sets the name of the column (Unique).
		/// </summary>
		/// <value>The name of the column.</value>
		string columnName{ get; set; }

		/// <summary>
		/// Gets or sets the index of the column order.
		/// </summary>
		/// <value>The index of the column order.</value>
		int columnOrderIndex{ get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Dataelus.TableDisplay.IColumnDef"/> column is visible.
		/// </summary>
		/// <value><c>true</c> if column visible; otherwise, <c>false</c>.</value>
		bool columnVisible{ get; set; }

		/// <summary>
		/// Copies values from the other object.
		/// </summary>
		/// <param name="other">Other.</param>
		void copyFrom (IColumnDef other);

		/// <summary>
		/// Whether this object Equals the specified other object.
		/// </summary>
		/// <param name="other">Other.</param>
		/// <param name="comparer">Comparer.</param>
		bool Equals (IColumnDef other, System.Collections.Generic.IEqualityComparer<string> comparer);
	}
}

