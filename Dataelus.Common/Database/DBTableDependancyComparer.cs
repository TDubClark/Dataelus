using System;
using System.Collections.Generic;

namespace Dataelus.Database
{
	/// <summary>
	/// Database Table dependancy comparer (requires an internal comparer to do all the work - this is just a shell at this point).
	/// </summary>
	public class DBTableDependancyComparer : IComparer<string>
	{
		/// <summary>
		/// Gets or sets the other comparer.
		/// </summary>
		/// <value>The other comparer.</value>
		public IComparer<string> OtherComparer { get; set; }

		/// <summary>
		/// Gets or sets the table constraints.
		/// </summary>
		/// <value>The table constraints.</value>
		public DBTableConstraintCollection TableConstraints { get; set; }

		/// <summary>
		/// Gets or sets the table order.
		/// </summary>
		/// <value>The table order.</value>
		public DependencyOrder TableOrder { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.DBTableDependancyComparer"/> class.
		/// </summary>
		/// <param name="otherComparer">Other comparer.</param>
		public DBTableDependancyComparer (IComparer<string> otherComparer)
		{
			if (otherComparer == null)
				throw new ArgumentNullException ("otherComparer");
			OtherComparer = otherComparer;
		}

		public DBTableDependancyComparer (DBTableConstraintCollection tableConstraints)
		{
			if (tableConstraints == null)
				throw new ArgumentNullException ("tableConstraints");
			TableConstraints = tableConstraints;
		}

		#region IComparer implementation

		public int Compare (string x, string y)
		{
			if (TableConstraints != null) {
				switch (this.TableOrder) {
					case DependencyOrder.LeastDependentToMost:
						return TableConstraints.Compare (x, y);
					case DependencyOrder.MostDependentToLeast:
						return TableConstraints.Compare (x, y);
					default:
						break;
				}
			}

			if (OtherComparer != null)
				return OtherComparer.Compare (x, y);
			
			throw new NullReferenceException ("Both the table constraints object and the alternative comparer are Null.");
		}

		#endregion
	}

	/// <summary>
	/// Dependency order.
	/// </summary>
	public enum DependencyOrder
	{
		/// <summary>
		/// Tables are ordered from least dependent (first) to most.
		/// </summary>
		LeastDependentToMost,

		/// <summary>
		/// Tables are ordered from most dependent (first) to least.
		/// </summary>
		MostDependentToLeast
	}

	/// <summary>
	/// Stores the database table dependancy order.  Can order two table names be dependency.
	/// </summary>
	public class DBTableDependancyOrder : IComparer<string>
	{
		/// <summary>
		/// Gets or sets the table names ordered.
		/// </summary>
		/// <value>The table names ordered.</value>
		public List<string> TableNamesOrdered {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the table order.
		/// </summary>
		/// <value>The table order.</value>
		public DependencyOrder TableOrder {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the table name equality comparer.
		/// </summary>
		/// <value>The equality comparer.</value>
		public IEqualityComparer<string> TableNameEqualityComparer {
			get;
			set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.DBTableDependancyOrder"/> class.
		/// </summary>
		public DBTableDependancyOrder ()
			: this (new List<string> ())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.DBTableDependancyOrder"/> class.
		/// </summary>
		/// <param name="tableNamesOrdered">Table names, ordered according to dependency.</param>
		public DBTableDependancyOrder (List<string> tableNamesOrdered)
		{
			this.TableNamesOrdered = tableNamesOrdered;
			this.TableNameEqualityComparer = new StringEqualityComparer ();
		}

		#region IComparer implementation

		public int Compare (string x, string y)
		{
			int iX = this.TableNamesOrdered.FindIndex (item => this.TableNameEqualityComparer.Equals (item, x));
			int iY = this.TableNamesOrdered.FindIndex (item => this.TableNameEqualityComparer.Equals (item, y));

			if (iX < 0 || iY < 0)
				return 0;

			// Compare the indexes
			return iX.CompareTo (iY);
		}

		#endregion
	}
}

