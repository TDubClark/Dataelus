using System;
using System.Collections.Generic;

namespace Dataelus
{
	/// <summary>
	/// Interface for a tracked collection; where the minimal number of changes are made
	/// </summary>
	public interface ICollectionTracked<T> : ICollection<T>
	{
		/// <summary>
		/// Updates this collection to match the given list.
		/// </summary>
		/// <param name="newCollection">The new collection of items.</param>
		void Update (IEnumerable<T> newCollection);
	}

	/// <summary>
	/// Interface for a collection where changes can be tracked.
	/// </summary>
	public interface ICollectionTrackable
	{
		bool IsTracking { get; set; }

		void ResetTracking ();

		void StartTracking ();

		void StopTracking ();
	}
}

