using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus
{
	/// <summary>
	/// Interface for an item in a hierarchy according to uniquely-indentifying codes.
	/// </summary>
	public interface ICodedHierarchyNode
	{
		/// <summary>
		/// Gets or sets the item code (Unique Identifier).
		/// </summary>
		/// <value>The item code.</value>
		string ItemCode{ get; set; }

		/// <summary>
		/// Gets or sets the codes of parent items.
		/// </summary>
		/// <value>The parent item codes.</value>
		List<string> ParentItemCodes{ get; set; }
	}

	/// <summary>
	/// Coded hierarchy class; has methods for working with a Code-based Hierarchy (with items which implement <see cref="Dataelus.ICodedHierarchyNode"/>).
	/// </summary>
	public class CodedHierarchy
	{
		protected IEqualityComparer<string> _comparer;

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.CodedHierarchy"/> class.
		/// </summary>
		public CodedHierarchy ()
		{
			
		}

		public CodedHierarchy (IEqualityComparer<string> comparer)
			: this ()
		{
			_comparer = comparer;
		}

		/// <summary>
		/// Gets the item codes for all parents of the item identified by [itemCode].
		/// </summary>
		/// <returns>The parent codes.</returns>
		/// <param name="nodes">Nodes.</param>
		/// <param name="itemCode">Item code.</param>
		public string[] GetParentCodes (IEnumerable<ICodedHierarchyNode> nodes, string itemCode)
		{
			return GetParentCodes (nodes, itemCode, _comparer);
		}

		/// <summary>
		/// Gets the item codes for all parents of the item identified by [itemCode].
		/// </summary>
		/// <returns>The parent codes.</returns>
		/// <param name="nodes">Nodes.</param>
		/// <param name="itemCode">Item code.</param>
		/// <param name="comparer">Comparer.</param>
		public static string[] GetParentCodes (IEnumerable<ICodedHierarchyNode> nodes, string itemCode, IEqualityComparer<string> comparer)
		{
			var scoll = new StringCollection ();
			GetParentCodesRecurse (scoll, new List<ICodedHierarchyNode> (nodes), itemCode, comparer, true);
			return scoll.ItemsNonNull.ToArray ();
		}

		/// <summary>
		/// Gets the parent codes recurse.
		/// </summary>
		/// <param name="nodes">Nodes.</param>
		/// <param name="itemCode">Item code.</param>
		/// <param name="comparer">Comparer.</param>
		/// <param name="isInitial">If set to <c>true</c>, then this is the initial (set to False for all calls from this function).</param>
		private static void GetParentCodesRecurse (StringCollection scoll, List<ICodedHierarchyNode> lstNodes, string itemCode, IEqualityComparer<string> comparer, bool isInitial)
		{
			// If item is already added, then stop this branch of the the recursion
			if (scoll.Contains (itemCode, comparer))
				return;

			int index = lstNodes.FindIndex (x => comparer.Equals (x.ItemCode, itemCode));
			if (index < 0)
				return;

			if (!isInitial)
				scoll.Add (itemCode);
			if (lstNodes [index].ParentItemCodes != null) {
				foreach (var parentCode in lstNodes [index].ParentItemCodes) {
					GetParentCodesRecurse (scoll, lstNodes, parentCode, comparer, false);
				}
			}
		}

		/// <summary>
		/// Gets the item codes for all children of the item identified by [itemCode].
		/// </summary>
		/// <returns>The child codes.</returns>
		/// <param name="nodes">Nodes.</param>
		/// <param name="itemCode">Item code.</param>
		public string[] GetChildCodes (IEnumerable<ICodedHierarchyNode> nodes, string itemCode)
		{
			return GetChildCodes (nodes, itemCode, _comparer);
		}

		/// <summary>
		/// Gets the item codes for all children of the item identified by [itemCode].
		/// </summary>
		/// <returns>The child codes.</returns>
		/// <param name="nodes">Nodes.</param>
		/// <param name="itemCode">Item code.</param>
		/// <param name="comparer">Comparer.</param>
		public static string[] GetChildCodes (IEnumerable<ICodedHierarchyNode> nodes, string itemCode, IEqualityComparer<string> comparer)
		{
			var scoll = new StringCollection ();
			GetChildCodesRecurse (scoll, new List<ICodedHierarchyNode> (nodes), itemCode, comparer, true);
			return scoll.ItemsNonNull.ToArray ();
		}

		private static void GetChildCodesRecurse (StringCollection scoll, List<ICodedHierarchyNode> lstNodes, string itemCode, IEqualityComparer<string> comparer, bool isInitial)
		{
			// If item is already added, then stop this branch of the recursion
			if (scoll.Contains (itemCode, comparer))
				return;

			if (!isInitial)
				scoll.Add (itemCode);
			foreach (var node in lstNodes) {
				if (node.ParentItemCodes != null) {
					// Find all items to which this is the parent
					if (node.ParentItemCodes.Contains (itemCode)) {
						GetChildCodesRecurse (scoll, lstNodes, node.ItemCode, comparer, false);
					}
				}
			}
		}
	}
}

namespace Dataelus.Generic
{
	/// <summary>
	/// Interface for an item in a hierarchy according to uniquely-indentifying code objects (type C).
	/// </summary>
	public interface IHierarchyNode<C, V> : IHierarchyNode<C>
	{
		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		V Value { get; set; }
	}
	/// <summary>
	/// Interface for an item in a hierarchy according to uniquely-indentifying code objects (type C).
	/// </summary>
	public interface IHierarchyNode<C>
	{
		/// <summary>
		/// Gets or sets the item code (Unique Identifier).
		/// </summary>
		/// <value>The item code.</value>
		C ItemCode{ get; set; }

		/// <summary>
		/// Gets or sets the codes of parent items.
		/// </summary>
		/// <value>The parent item codes.</value>
		List<C> ParentItemCodes{ get; set; }
	}

	/// <summary>
	/// Interface for an item in a hierarchy according to uniquely-indentifying codes.
	/// </summary>
	public interface ICodedHierarchyNode<T> : ICodedHierarchyNode
	{
		T Item{ get; set; }
	}

	/// <summary>
	/// An item in a hierarchy according to uniquely-indentifying codes.
	/// </summary>
	public class CodedHierarchyNode<T> : ICodedHierarchyNode<T>
	{
		protected T _item;

		/// <summary>
		/// Gets or sets the item.
		/// </summary>
		/// <value>The item.</value>
		public T Item {
			get { return _item; }
			set { _item = value; }
		}

		protected string _itemCode;

		/// <summary>
		/// Gets or sets the item code (Unique Identifier).
		/// </summary>
		/// <value>The item code.</value>
		public string ItemCode {
			get { return _itemCode; }
			set { _itemCode = value; }
		}

		protected List<string> _parentItemCodes;

		/// <summary>
		/// Gets or sets the codes of parent items.
		/// </summary>
		/// <value>The parent item codes.</value>
		public List<string> ParentItemCodes {
			get { return _parentItemCodes; }
			set { _parentItemCodes = value; }
		}

		public CodedHierarchyNode ()
			: this (default(T), null)
		{
		}

		public CodedHierarchyNode (T item, string code)
			: this (item, code, new string[]{ })
		{
		}

		public CodedHierarchyNode (T item, string code, params string[] parentCodes)
			: this (item, code, new List<string> (parentCodes))
		{
		}

		public CodedHierarchyNode (T item, string code, IEnumerable<string> parentCodes)
		{
			_item = item;
			_itemCode = code;
			_parentItemCodes = new List<string> (parentCodes);
		}
	}

	/// <summary>
	/// Coded hierarchy.
	/// Gets parents and children for a generic type which implements ICodedHierarchyNode.
	/// </summary>
	public class CodedHierarchy<T> : CodedHierarchy where T: Dataelus.ICodedHierarchyNode
	{
		public CodedHierarchy ()
			: base ()
		{
			
		}

		public CodedHierarchy (IEqualityComparer<string> comparer)
			: base (comparer)
		{
			
		}

		/// <summary>
		/// Gets the parents of the given itemCode, from the list of items.
		/// </summary>
		/// <returns>The parents.</returns>
		/// <param name="items">Items.</param>
		/// <param name="itemCode">Item code.</param>
		public List<T> GetParents (List<T> items, string itemCode)
		{
			var parentCodes = GetParentCodes (items.Cast<ICodedHierarchyNode> (), itemCode);
			var parents = new List<T> ();

			foreach (var code in parentCodes) {
				parents.Add (items.Find (x => _comparer.Equals (x.ItemCode, code)));
			}

			return parents;
		}

		/// <summary>
		/// Gets the children of the given itemCode, from the list of items.
		/// </summary>
		/// <returns>The children.</returns>
		/// <param name="items">Items.</param>
		/// <param name="itemCode">Item code.</param>
		public List<T> GetChildren (List<T> items, string itemCode)
		{
			var childCodes = GetChildCodes (items.Cast<ICodedHierarchyNode> (), itemCode);
			var children = new List<T> ();

			foreach (var code in childCodes) {
				children.Add (items.Find (x => _comparer.Equals (x.ItemCode, code)));
			}

			return children;
		}
	}
}

