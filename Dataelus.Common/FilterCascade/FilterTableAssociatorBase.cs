using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.FilterCascade
{
	/// <summary>
	/// Filter table associator.
	/// </summary>
	public class FilterTableAssociatorBase : IFilterTableAssociator
	{
		protected ManyToMany<string> _associations;

		/// <summary>
		/// Gets or sets the associations between tables (keys) and filters (values).
		/// </summary>
		/// <value>The associations.</value>
		public ManyToMany<string> Associations {
			get { return _associations; }
			set { _associations = value; }
		}

		/// <summary>
		/// Adds the association.
		/// </summary>
		/// <param name="tableCode">Table code (key).</param>
		/// <param name="filterCode">Filter code (value).</param>
		public void AddAssociation (string tableCode, string filterCode)
		{
			_associations.AddItem (tableCode, filterCode);
		}

		protected IEqualityComparer<string> _comparer;

		public FilterTableAssociatorBase ()
			: this (new StringEqualityComparer ())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.FilterCascade.FilterTableAssociatorBase"/> class.
		/// </summary>
		/// <param name="comparer">The Comparer between any two values.</param>
		public FilterTableAssociatorBase (IEqualityComparer<string> comparer)
		{
			_comparer = comparer;
			_associations = new ManyToMany<string> (comparer);
		}

		#region IFilterTableAssociator implementation

		public bool IsAssociated (string tableCode, string filterCode)
		{
			return _associations.GetValues (tableCode).Contains (filterCode, _comparer);
		}

		#endregion
	}
}

