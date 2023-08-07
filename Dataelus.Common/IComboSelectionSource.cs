using System;
using System.Linq;

namespace Dataelus
{
	/// <summary>
	/// Interface for a combobox selection source.
	/// </summary>
	public interface IComboSelectionSource : System.Collections.IEnumerable
	{
		/// <summary>
		/// Gets or sets the item equality comparer.
		/// </summary>
		/// <value>The item equality comparer.</value>
		System.Collections.IEqualityComparer ItemComparer { get; set; }
	}

	/// <summary>
	/// Interface for a combobox selection source, in which objects can be referenced by unique codes.
	/// </summary>
	public interface IComboSelectionSourceCoded : IComboSelectionSource
	{
		/// <summary>
		/// Function for getting the display text of an object.
		/// </summary>
		/// <value>The displayer.</value>
		Func<object, string> Displayer { get; set; }

		/// <summary>
		/// Function for getting the unique code of an object
		/// </summary>
		/// <value>The get unique code.</value>
		Func<object, string> GetUniqueCode { get; set; }

		/// <summary>
		/// Gets or sets the equality comparer for the unique codes.
		/// </summary>
		/// <value>The equality comparer for the unique codes.</value>
		System.Collections.Generic.IEqualityComparer<string> UniqueCodeComparer { get; set; }
	}

	/// <summary>
	/// Base list, which can serve as a ComboBox selection source, with coded objects.
	/// </summary>
	public class ComboSelectionSourceCodedBase<T> : ListBase<T>, IComboSelectionSourceCoded where T : class
	{
		/// <summary>
		/// Function for getting the display text of an object.
		/// </summary>
		/// <value>The displayer.</value>
		public Func<object, string> Displayer { get; set; }

		/// <summary>
		/// Function for getting the unique code of an object
		/// </summary>
		/// <value>The get unique code.</value>
		public Func<object, string> GetUniqueCode { get; set; }

		/// <summary>
		/// Gets or sets the equality comparer for the unique codes.
		/// </summary>
		/// <value>The equality comparer for the unique codes.</value>
		public System.Collections.Generic.IEqualityComparer<string> UniqueCodeComparer { get; set; }

		/// <summary>
		/// Gets or sets the item equality comparer.
		/// </summary>
		/// <value>The item equality comparer.</value>
		public System.Collections.IEqualityComparer ItemComparer { get; set; }

		/// <summary>
		/// Gets or sets the item equality comparer (generic).
		/// </summary>
		/// <value>The item comparer generic.</value>
		public System.Collections.Generic.IEqualityComparer<T> ItemComparerGeneric { get; set; }

		/// <summary>
		/// Gets the item by the given code.
		/// </summary>
		/// <returns>The item by code.</returns>
		/// <param name="uniqueCode">Unique code.</param>
		public virtual T GetItemByCode (string uniqueCode)
		{
			return _items.Find (x => this.UniqueCodeComparer.Equals (uniqueCode, this.GetUniqueCode (x)));
		}

		/// <summary>
		/// Gets the code-based dictionary.
		/// </summary>
		/// <returns>The code dictionary.</returns>
		public System.Collections.Generic.Dictionary<string, T> GetCodeDictionary ()
		{
			return _items.ToDictionary (x => this.GetUniqueCode (x), this.UniqueCodeComparer);
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.ComboSelectionSourceCodedBase`1"/> class.
		/// </summary>
		protected ComboSelectionSourceCodedBase ()
		{
			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.ComboSelectionSourceCodedBase`1"/> class.
		/// </summary>
		/// <param name="displayer">Displayer for object T - gets the display text.</param>
		/// <param name="getUniqueCode">Gets a unique code for object T.</param>
		public ComboSelectionSourceCodedBase (Func<T, string> displayer, Func<T, string> getUniqueCode)
		{
			InitComboSourceGeneric (displayer, getUniqueCode);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.ComboSelectionSourceCodedBase`1"/> class.
		/// </summary>
		/// <param name="displayer">Displayer for object T - gets the display text.</param>
		/// <param name="getUniqueCode">Gets a unique code for object T.</param>
		/// <param name="uniqueCodeComparer">Unique code comparer.</param>
		public ComboSelectionSourceCodedBase (Func<T, string> displayer, Func<T, string> getUniqueCode
			, System.Collections.Generic.IEqualityComparer<string> uniqueCodeComparer)
		{
			InitComboSourceGeneric (displayer, getUniqueCode, uniqueCodeComparer);
		}

		/// <summary>
		/// Initializes the combo source variables, using generic functions and the default String Equality Comparer.
		/// </summary>
		/// <param name="displayer">Displayer for object T - gets the display text.</param>
		/// <param name="getUniqueCode">Gets a unique code for object T.</param>
		public virtual void InitComboSourceGeneric (Func<T, string> displayer, Func<T, string> getUniqueCode)
		{
			InitComboSourceGeneric (displayer, getUniqueCode, new StringEqualityComparer ());
		}

		/// <summary>
		/// Initializes the combo source variables, using generic functions.
		/// </summary>
		/// <param name="displayer">Displayer for object T - gets the display text.</param>
		/// <param name="getUniqueCode">Gets a unique code for object T.</param>
		/// <param name="uniqueCodeComparer">Unique code comparer.</param>
		public virtual void InitComboSourceGeneric (
			Func<T, string> displayer, Func<T, string> getUniqueCode
			, System.Collections.Generic.IEqualityComparer<string> uniqueCodeComparer)
		{
			var comparer = new NullableStringFieldEqualityComparer<T> (uniqueCodeComparer, getUniqueCode);
			InitComboSourceGeneric (comparer, comparer, displayer, getUniqueCode, uniqueCodeComparer);
		}

		/// <summary>
		/// Initializes the combo source variables, using generic functions.
		/// </summary>
		/// <param name="itemComparer">Item comparer.</param>
		/// <param name="itemComparerGeneric">Item comparer generic.</param>
		/// <param name="displayer">Displayer.</param>
		/// <param name="getUniqueCode">Get unique code.</param>
		/// <param name="uniqueCodeComparer">Unique code comparer.</param>
		public virtual void InitComboSourceGeneric (System.Collections.IEqualityComparer itemComparer
			, System.Collections.Generic.IEqualityComparer<T> itemComparerGeneric
			, Func<T, string> displayer, Func<T, string> getUniqueCode
			, System.Collections.Generic.IEqualityComparer<string> uniqueCodeComparer)
		{
			this.ItemComparer = itemComparer;
			this.ItemComparerGeneric = itemComparerGeneric;
			this.Displayer = x => displayer ((T)x);
			this.GetUniqueCode = x => getUniqueCode ((T)x);
			this.UniqueCodeComparer = uniqueCodeComparer;
		}

		/// <summary>
		/// Initializes the combo source variables, using generic functions.
		/// </summary>
		/// <param name="itemComparer">Item comparer - compares two objects for equality.</param>
		/// <param name="displayer">Displayer for object T - gets the display text.</param>
		/// <param name="getUniqueCode">Gets a unique code for object T.</param>
		/// <param name="uniqueCodeComparer">Unique code comparer.</param>
		public virtual void InitComboSourceGeneric (System.Collections.IEqualityComparer itemComparer
			, Func<T, string> displayer, Func<T, string> getUniqueCode
			, System.Collections.Generic.IEqualityComparer<string> uniqueCodeComparer)
		{
			this.ItemComparer = itemComparer;
			this.ItemComparerGeneric = new GenericEqualityComparer<T> (itemComparer);
			this.Displayer = x => displayer ((T)x);
			this.GetUniqueCode = x => getUniqueCode ((T)x);
			this.UniqueCodeComparer = uniqueCodeComparer;
		}

		/// <summary>
		/// Initializes the combo source variables.
		/// </summary>
		/// <param name="itemComparer">Item comparer.</param>
		/// <param name="displayer">Displayer.</param>
		/// <param name="getUniqueCode">Get unique code.</param>
		/// <param name="uniqueCodeComparer">Unique code comparer.</param>
		public virtual void InitComboSource (System.Collections.IEqualityComparer itemComparer
			, Func<object, string> displayer, Func<object, string> getUniqueCode
			, System.Collections.Generic.IEqualityComparer<string> uniqueCodeComparer)
		{
			this.ItemComparer = itemComparer;
			this.ItemComparerGeneric = new GenericEqualityComparer<T> (itemComparer);
			this.Displayer = displayer;
			this.GetUniqueCode = getUniqueCode;
			this.UniqueCodeComparer = uniqueCodeComparer;
		}
	}

	class GenericEqualityComparer<T> : System.Collections.Generic.EqualityComparer<T>
	{
		public System.Collections.IEqualityComparer Comparer { get; set; }

		public GenericEqualityComparer (System.Collections.IEqualityComparer comparer)
		{
			this.Comparer = comparer;
		}

		#region implemented abstract members of EqualityComparer

		public override bool Equals (T x, T y)
		{
			return this.Comparer.Equals (x, y);
		}

		public override int GetHashCode (T obj)
		{
			return this.Comparer.GetHashCode (obj);
		}

		#endregion
	}
}

