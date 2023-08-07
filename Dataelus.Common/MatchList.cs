using System;
using System.Collections.Generic;

namespace Dataelus
{
	/// <summary>
	/// Stores a match list between types X and Y.
	/// </summary>
	public class MatchList<X, Y>
	{
		protected Dictionary<X, Y> _XtoY;

		/// <summary>
		/// Gets or sets the x-to-y dictionary.
		/// </summary>
		/// <value>The xto y.</value>
		public Dictionary<X, Y> XtoY {
			get { return _XtoY; }
			set { _XtoY = value; }
		}

		protected Dictionary<Y, X> _YtoX;

		/// <summary>
		/// Gets or sets the y-to-x dictionary.
		/// </summary>
		/// <value>The yto x.</value>
		public Dictionary<Y, X> YtoX {
			get { return _YtoX; }
			set { _YtoX = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.MatchList`2"/> class.
		/// </summary>
		/// <param name="xComparer">X comparer.</param>
		/// <param name="yComparer">Y comparer.</param>
		public MatchList (IEqualityComparer<X> xComparer, IEqualityComparer<Y> yComparer)
		{
			if (xComparer == null)
				_XtoY = new Dictionary<X, Y> ();
			else
				_XtoY = new Dictionary<X, Y> (xComparer);

			if (yComparer == null)
				_YtoX = new Dictionary<Y, X> ();
			else
				_YtoX = new Dictionary<Y, X> (yComparer);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.MatchList`2"/> class.
		/// </summary>
		public MatchList ()
			: this (null, null)
		{
		}

		/// <summary>
		/// Add the specified xvalue and yvalue.
		/// </summary>
		/// <param name="xvalue">Xvalue.</param>
		/// <param name="yvalue">Yvalue.</param>
		public void Add (X xvalue, Y yvalue)
		{
			_XtoY.Add (xvalue, yvalue);
			_YtoX.Add (yvalue, xvalue);
		}

		public X GetX (Y value)
		{
			return _YtoX [value];
		}

		public Y GetY (X value)
		{
			return _XtoY [value];
		}

		public bool TryGetX (Y value, out X result)
		{
			return _YtoX.TryGetValue (value, out result);
		}

		public bool TryGetY (X value, out Y result)
		{
			return _XtoY.TryGetValue (value, out result);
		}
	}
}

