using System;
using System.Collections.Generic;

namespace Dataelus
{
	/// <summary>
	/// Graph of value/display pairs.
	/// </summary>
	public class GraphValueDisplay
	{
		/// <summary>
		/// Gets or sets the list of root nodes in the graph.
		/// </summary>
		/// <value>The root node.</value>
		public List<GraphValueDisplayRootNode> RootNodes{ get; set; }

		public GraphValueDisplay ()
		{
			RootNodes = new List<GraphValueDisplayRootNode> ();
		}

		protected IEqualityComparer<object> NodeClassComparer{ get; set; }

		/// <summary>
		/// Loads the graph.
		/// </summary>
		/// <param name="textTable">Text table.</param>
		/// <param name="textFilters">Text filters.</param>
		public void LoadGraph (Table.TextTable textTable, FilterCascade.FilterTextItemCollection textFilters)
		{
			var filterTable = new FilterCascade.FilterTextTable (textTable);
			var comparer = new StringEqualityComparer ();

			foreach (var item in textFilters) {
				var newRoot = new GraphValueDisplayRootNode () {
					NodeClass = item.ColumnName,
					ValueType = item.DataType
				};

				// Get the values for this filter
				var valueDict = filterTable.GetValueDisplay (textFilters, item.FilterCode, comparer);
				foreach (var valuePair in valueDict) {
					var newNode = newRoot.AddNode (valuePair.Key, valuePair.Value);

					// TODO: Reference this node from any existing parent nodes.
					if (newNode != null) {

					}
					//var parents = textFilters.GetParentFilters (item.ColumnName);
				}

				RootNodes.Add (newRoot);
			}

			// Connect the whole graph together
			ConnectGraphNodes (filterTable, textFilters, comparer);
		}

		/// <summary>
		/// Connects the graph nodes.
		/// </summary>
		/// <param name="filterTable">Filter table.</param>
		/// <param name="textFilters">Text filters.</param>
		/// <param name="comparer">Comparer.</param>
		private void ConnectGraphNodes (FilterCascade.FilterTextTable filterTable, FilterCascade.FilterTextItemCollection textFilters, IEqualityComparer<string> comparer)
		{
			// The idea is to go through each row of the table, find all the nodes, and make sure they are all connected.
			for (int i = 0; i < filterTable.RowCount; i++) {

			}
			throw new NotImplementedException ();
		}
	}

	public class NodeClassComparer : IEqualityComparer<object>, System.Collections.IEqualityComparer
	{
		public IEqualityComparer<string> Comparer{ get; set; }

		public NodeClassComparer ()
		{
			this.Comparer = new StringEqualityComparer ();
		}

		///<Docs><param name="x">First <see langword="T" /> to compare.</param><param name="y">Second <see langword="T" /> to compare.</param><summary><para>Determines whether the specified objects are equal.</para></summary><returns><para><see langword="true" /> if the specified objects are equal; otherwise, <see langword="false" />.</para></returns><remarks><para>An implementation of Equals(T,T) shall satisfy the following: The equality function shall be be reflexive, so x.Equals(x) is true; symmetric, so x.Equals(y) and y.Equals(x); and transitive, so x.Equals(y)  and
		///y.Equals(z) implies x.Equals(z); for any values x, y and z for  which these expressions are defined.</para></remarks><since version=".NET 2.0" /></Docs>
		public new bool Equals (object obj1, object obj2)
		{
			return Comparer.Equals ((string)obj1, (string)obj2);
		}

		///<Docs><param name="obj">The object for which the hash code is to be returned.</param><summary><para>Returns a hash code for the specified object.</para></summary><returns><para>A hash code for the specified object.</para></returns><remarks><para>To produce a hash function for the given object. <block subset="none" type="note"> A hash function is used to
		///   quickly generate a number (a hash code) corresponding to the value of an object.
		///   Hash functions are used with <see langword="hashtables" />. A good hash function
		///   algorithm rarely generates hash codes that collide. For more information about
		///   hash functions, see <paramref name="The Art of Computer Programming" />
		///
		///   , Vol. 3, by Donald E. Knuth.</block></para><block subset="none" type="behaviors"><para>All implementations are required to ensure that if x.Equals(y) ==  true, then x.GetHashCode() equals y.GetHashCode(), for any x and y values for which these expressions are defined.</para></block></remarks><since version=".NET 2.0" /><exception cref="T:System.ArgumentNullException">The type of <paramref name="obj" /> is a reference type and <paramref name="obj" /> is <see langword="null" />.</exception></Docs>
		public int GetHashCode (object obj)
		{
			return Comparer.GetHashCode ((string)obj);
		}
	}
}

