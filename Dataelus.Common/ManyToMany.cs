using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus
{
	/// <summary>
	/// Many to many relationship between keys (of type K) and values (of type V).
	/// </summary>
	public class ManyToMany<K, V>
	{
		protected Dictionary<K, List<V>> _associations;

		/// <summary>
		/// Gets or sets the associations between keys (type K) and values (type V).
		/// </summary>
		/// <value>The associations.</value>
		public Dictionary<K, List<V>> Associations {
			get { return _associations; }
			set { _associations = value; }
		}

		protected IEqualityComparer<K> _keyComparer;

		/// <summary>
		/// Gets or sets the comparer between any two keys of type (K).
		/// </summary>
		/// <value>The comparer.</value>
		public IEqualityComparer<K> KeyComparer {
			get { return _keyComparer; }
			set { _keyComparer = value; }
		}

		protected IEqualityComparer<V> _valueComparer;

		/// <summary>
		/// Gets or sets the comparer between any two values of type (V).
		/// </summary>
		/// <value>The comparer.</value>
		public IEqualityComparer<V> ValueComparer {
			get {
				return _valueComparer; }
			set { _valueComparer = value;
			}
		}

		/// <summary>
		/// Adds the item to the collection.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		public void AddItem (K key, V value)
		{
			List<V> lst;
			if (_associations.TryGetValue (key, out lst)) {
				if (_valueComparer == null) {
					if (!lst.Contains (value)) {
						lst.Add (value);
					}
				} else {
					if (!lst.Contains (value, _valueComparer)) {
						lst.Add (value);
					}
				}
			} else {
				Add (key, value);
			}
		}

		/// <summary>
		/// Add the specified key and value to the dictionary (does not check for existing values).
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		protected void Add (K key, V value)
		{
			_associations.Add (key, new List<V> (new V[]{ value }));
		}

		/// <summary>
		/// Gets the keys with the associated value.
		/// </summary>
		/// <returns>The keys.</returns>
		/// <param name="value">Value.</param>
		public List<K> GetKeys (V value)
		{
			if (_valueComparer == null)
				return _associations.Where (x => x.Value.Contains (value)).Select (x => x.Key).ToList ();
			return _associations.Where (x => x.Value.Contains (value, _valueComparer)).Select (x => x.Key).ToList ();
		}

		/// <summary>
		/// Gets the values for the given key.
		/// </summary>
		/// <returns>The values.</returns>
		/// <param name="key">Key.</param>
		/// <exception cref="ArgumentException">If the key is not found.</exception>
		public List<V> GetValues (K key)
		{
			List<V> values;
			if (_associations.TryGetValue (key, out values)) { return values; } else {
				throw new ArgumentException (String.Format ("The given key '{0}' was not found in the dictionary.", key), "key");
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.ManyToMany`1"/> class.
		/// No default comparer is set.
		/// </summary>
		public ManyToMany ()
		{
			_associations = new Dictionary<K, List<V>> ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.ManyToMany`1"/> class.
		/// </summary>
		/// <param name="comparer">The comparer between any values of type (T).</param>
		public ManyToMany (IEqualityComparer<K> keyComparer)
		{
			_keyComparer = keyComparer;
			_associations = new Dictionary<K, List<V>> (keyComparer);
		}

		public ManyToMany (IEqualityComparer<K> keyComparer, IEqualityComparer<V> valueComparer)
			: this (keyComparer)
		{
			_valueComparer = valueComparer;
		}
	}

	/// <summary>
	/// Data structure for a many-to-many relationship between data of the same type.
	/// </summary>
	public class ManyToMany<T> : ManyToMany<T, T>
	{
		protected IEqualityComparer<T> _comparer;

		/// <summary>
		/// Gets or sets the comparer between any values of type (T).
		/// </summary>
		/// <value>The comparer.</value>
		public IEqualityComparer<T> Comparer {
			get { return _comparer; }
			set { _comparer = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.ManyToMany`1"/> class.
		/// No default comparer is set.
		/// </summary>
		public ManyToMany ()
			: base ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.ManyToMany`1"/> class.
		/// </summary>
		/// <param name="comparer">The comparer between any values of type (T).</param>
		public ManyToMany (IEqualityComparer<T> comparer)
			: base (comparer, comparer)
		{
			_comparer = comparer;
		}
	}
}

