using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus
{
	/// <summary>
	/// A list of items which are kept distinct - items are added/inserted only if not found in the list
	/// </summary>
	public class DistinctListBase<T> : ListBase<T>
	{
		IEqualityComparer<T> _uniquenessComparer;

		Predicate<T> _filterer;

		bool ContainsItem (T item)
		{
			return _items.Contains (item, _uniquenessComparer);
		}

		bool AllowItem (T item)
		{
			return _filterer (item);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.DistinctListBase`1"/> class.
		/// </summary>
		/// <param name="uniquenessComparer">Uniqueness comparer.</param>
		public DistinctListBase (IEqualityComparer<T> uniquenessComparer)
			: this (uniquenessComparer, x => true)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.DistinctListBase`1"/> class.
		/// </summary>
		/// <param name="uniquenessComparer">Uniqueness comparer.</param>
		/// <param name="filterer">Filterer - only items which satisfy this predicate (return true) are allowed in the list.</param>
		public DistinctListBase (IEqualityComparer<T> uniquenessComparer, Predicate<T> filterer)
		{
			this._uniquenessComparer = uniquenessComparer;
			this._filterer = filterer;
		}

		public DistinctListBase (IEnumerable<T> collection, IEqualityComparer<T> uniquenessComparer)
			: this (collection, uniquenessComparer, x => true)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.DistinctListBase`1"/> class.
		/// </summary>
		/// <param name="collection">Collection.</param>
		/// <param name="uniquenessComparer">Uniqueness comparer.</param>
		/// <param name="filterer">Filterer - only items which satisfy this predicate (return true) are allowed in the list.</param>
		public DistinctListBase (IEnumerable<T> collection, IEqualityComparer<T> uniquenessComparer, Predicate<T> filterer)
			: this (uniquenessComparer, filterer)
		{
			AddItems (collection);
		}

		public override void Add (T item)
		{
			if (!ContainsItem (item) && AllowItem(item))
				base.Add (item);
		}

		public override void Insert (int index, T item)
		{
			if (!ContainsItem (item) && AllowItem(item))
				base.Insert (index, item);
		}
	}
}

