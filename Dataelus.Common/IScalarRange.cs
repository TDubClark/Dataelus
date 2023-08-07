using System;
using System.Collections.Generic;

namespace Dataelus
{
	/// <summary>
	/// Interface for a range of scalar values.
	/// </summary>
	public interface IScalarRange<T>
	{
		T LowerBound{ get; set; }

		T UpperBound{ get; set; }

		bool LowerBoundInclusive{ get; set; }

		bool UpperBoundInclusive{ get; set; }

		IComparer<T> BoundsComparer{ get; set; }

		bool WithinRange (T scalarValue);
	}

	/// <summary>
	/// Scalar range base implementation.
	/// </summary>
	public class ScalarRangeBase<T> : IScalarRange<T>
	{
		public ScalarRangeBase ()
		{
			_lowerBoundInclusive = false;
			_upperBoundInclusive = false;
		}

		public ScalarRangeBase (IComparer<T> comparer)
			: this ()
		{
			if (comparer == null)
				throw new ArgumentNullException ("comparer");
			_boundsComparer = comparer;
		}

		#region IScalarRange implementation

		protected T _lowerBound;

		public T LowerBound {
			get { return _lowerBound; }
			set { _lowerBound = value; }
		}

		protected T _upperBound;

		public T UpperBound {
			get { return _upperBound; }
			set { _upperBound = value; }
		}

		protected bool _lowerBoundInclusive;

		public bool LowerBoundInclusive {
			get { return _lowerBoundInclusive; }
			set { _lowerBoundInclusive = value; }
		}

		protected bool _upperBoundInclusive;

		public bool UpperBoundInclusive {
			get { return _upperBoundInclusive; }
			set { _upperBoundInclusive = value; }
		}

		protected IComparer<T> _boundsComparer;

		public IComparer<T> BoundsComparer {
			get { return _boundsComparer; }
			set { _boundsComparer = value; }
		}

		public virtual bool WithinRange (T scalarValue)
		{
			int comparedLower = _boundsComparer.Compare (_lowerBound, scalarValue);
			int comparedUpper = _boundsComparer.Compare (_upperBound, scalarValue);

			if (comparedLower > 0) // _lowerBound is greater than scalarValue
				return false;
			if (comparedUpper < 0) // _upperBound is less than scalarValue
				return false;

			if (comparedLower == 0)
				return _lowerBoundInclusive;
			if (comparedUpper == 0)
				return _upperBoundInclusive;

			return true;
		}

		#endregion
	}

	/// <summary>
	/// Value comparer (any class which implements IComparable(T).
	/// </summary>
	public class ValueComparer<T> : Comparer<T> where T : IComparable<T>
	{
		#region implemented abstract members of Comparer

		public override int Compare (T x, T y)
		{
			return x.CompareTo (y);
		}

		#endregion
	}

	/// <summary>
	/// Value range (any class which implements IComparable(T).
	/// </summary>
	public class ValueRange<T> : ScalarRangeBase<T> where T : IComparable<T>
	{
		public ValueRange ()
			: base (new ValueComparer<T> ())
		{
		}

		public ValueRange (IComparer<T> comparer)
			: base (comparer)
		{
		}
	}
}

