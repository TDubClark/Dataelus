using System;
using System.Collections;
using System.Collections.Generic;

namespace Dataelus
{
	/// <summary>
	/// Abstract Base class for a equality comparer of Nullable objects.
	/// </summary>
	public abstract class NullableEqualityComparer<T> : IEqualityComparer<T>, IEqualityComparer where T : class
	{
		protected bool _equalsIfBothNull = true;

		/// <summary>
		/// What to return from the Equals function if both objects are Null (default = true)
		/// </summary>
		public bool EqualsIfBothNull {
			get { return _equalsIfBothNull; }
			set { _equalsIfBothNull = value; }
		}

		protected int _hashCodeIfNonNull = 0;

		/// <summary>
		/// What to return from the GetHashCode function if the object is Null (default = 0)
		/// </summary>
		public int HashCodeIfNonNull {
			get { return _hashCodeIfNonNull; }
			set { _hashCodeIfNonNull = value; }
		}

		/// <summary>
		/// Determines whether the two non-null objects are equal.
		/// </summary>
		/// <returns><c>true</c>, if equals, <c>false</c> otherwise.</returns>
		/// <param name="x">The x item.</param>
		/// <param name="y">The y item.</param>
		protected abstract bool EqualsNonNull (T x, T y);

		/// <summary>
		/// Gets the hash code for the given non-null object.
		/// </summary>
		/// <returns>The hash code non null.</returns>
		/// <param name="obj">Object.</param>
		protected abstract int GetHashCodeNonNull (T obj);

		#region IEqualityComparer implementation

		/// <summary>
		/// Gets whether the given objects are equal.
		/// </summary>
		/// <param name="x">The x object.</param>
		/// <param name="y">The y object.</param>
		public bool Equals (T x, T y)
		{
			// If both objects are Null ...
			if (x == null && y == null)
				return _equalsIfBothNull;

			// If only one object is Null ...
			if (x == null || y == null)
				return false;
			
			return EqualsNonNull (x, y);
		}

		/// <Docs>The object for which the hash code is to be returned.</Docs>
		/// <para>Returns a hash code for the specified object.</para>
		/// <returns>A hash code for the specified object.</returns>
		/// <summary>
		/// Gets the hash code.
		/// </summary>
		/// <param name="obj">Object.</param>
		public int GetHashCode (T obj)
		{
			if (obj == null)
				return _hashCodeIfNonNull;
			return GetHashCodeNonNull (obj);
		}

		#endregion

		bool IEqualityComparer.Equals (object x, object y)
		{
			return this.Equals (x as T, y as T);
		}

		int IEqualityComparer.GetHashCode (object obj)
		{
			return this.GetHashCode (obj as T);
		}
	}

	/// <summary>
	/// Compares two nullable objects for equality on a single String field.
	/// </summary>
	public class NullableStringFieldEqualityComparer<T> : NullableEqualityComparer<T> where T : class
	{
		IEqualityComparer<string> _fieldComparer;

		public IEqualityComparer<string> FieldComparer {
			get { return _fieldComparer; }
			set { _fieldComparer = value; }
		}

		Func<T, string> _transform;

		public Func<T, string> Transform {
			get { return _transform; }
			set { _transform = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.NullableStringFieldEqualityComparer`1"/> class.
		/// </summary>
		/// <param name="fieldComparer">Field comparer.</param>
		/// <param name="transform">Transform from the object to the field.</param>
		public NullableStringFieldEqualityComparer (IEqualityComparer<string> fieldComparer, Func<T, string> transform)
		{
			if (fieldComparer == null)
				throw new ArgumentNullException ("fieldComparer");
			if (transform == null)
				throw new ArgumentNullException ("transform");
			this._fieldComparer = fieldComparer;
			this._transform = transform;
		}

		#region implemented abstract members of NullableEqualityComparer

		protected override bool EqualsNonNull (T x, T y)
		{
			return _fieldComparer.Equals (_transform (x), _transform (y));
		}

		protected override int GetHashCodeNonNull (T obj)
		{
			return _transform (obj).GetHashCode ();
		}

		#endregion
	}
}

