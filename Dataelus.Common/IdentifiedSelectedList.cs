using System.Linq;

namespace Dataelus
{
	/// <summary>
	/// List of selected objects.
	/// </summary>
	public class IdentifiedSelectedList : IdentifiedList<SelectedListItem>
	{
		public void Add (object item, bool selected)
		{
			Add (new SelectedListItem (item, selected));
		}

		/// <summary>
		/// Gets the selected items.
		/// </summary>
		/// <returns>The selected.</returns>
		public object[] GetSelected ()
		{
			return _identifiedValues.Select (x => x.Value)
				.ToList ()
				.Where (x => x.Selected)
				.Select (x => x.Item)
				.ToArray ();
		}

		public void Select (long id, bool selected)
		{
			SelectedListItem item;
			if (TryGetValue (id, out item)) {
				item.Selected = selected;
			}
		}

		public IdentifiedSelectedList ()
		{
		}
	}

	public class SelectedListItem
	{
		public object Item { get; set; }

		public bool Selected { get; set; }

		public SelectedListItem ()
		{
			
		}

		public SelectedListItem (object item, bool selected)
		{
			this.Item = item;
			this.Selected = selected;
		}
	}

	namespace Generic
	{
		/// <summary>
		/// List of selected objects.
		/// </summary>
		public class IdentifiedSelectedList<T> : IdentifiedList<SelectedListItem<T>>
		{
			public void Add (T item, bool selected)
			{
				Add (new SelectedListItem<T> (item, selected));
			}

			/// <summary>
			/// Gets the selected items.
			/// </summary>
			/// <returns>The selected.</returns>
			public T[] GetSelected ()
			{
				return _identifiedValues.Select (x => x.Value)
				.ToList ()
				.Where (x => x.Selected)
				.Select (x => x.Item)
				.ToArray ();
			}

			public void Select (long id, bool selected)
			{
				SelectedListItem<T> item;
				if (TryGetValue (id, out item)) {
					item.Selected = selected;
				}
			}

			public IdentifiedSelectedList ()
			{
			}
		}

		/// <summary>
		/// Selected list item - stores an item, and a switch indicating whether it is selected.
		/// </summary>
		public class SelectedListItem<T>
		{
			public T Item { get; set; }

			public bool Selected { get; set; }

			public SelectedListItem ()
			{

			}

			public SelectedListItem (T item, bool selected)
			{
				this.Item = item;
				this.Selected = selected;
			}
		}

		/// <summary>
		/// Selected list item - stores an item, and a switch indicating whether it is selected.
		/// </summary>
		public class IdentifiedSelectedListItem<T> : SelectedListItem<T>
		{
			public long UniqueID { get; set; }

			public IdentifiedSelectedListItem ()
			{
				
			}

			public IdentifiedSelectedListItem (T item, bool selected, long uniqueID)
				: base (item, selected)
			{
				this.UniqueID = uniqueID;
			}
		}
	}
}

