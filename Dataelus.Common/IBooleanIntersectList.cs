using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.Generic
{
	/// <summary>
	/// Interface for a boolean intersect list - where two items are intersected as a boolean value.
	/// </summary>
	public interface IBooleanIntersectList<TRow, TColumn>
	{
		void AddNew (TRow row, TColumn column);

		void Remove (TRow row, TColumn column);

		bool Contains (TRow row, TColumn column);
	}
}

