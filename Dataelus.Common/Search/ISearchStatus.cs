using System;

namespace Dataelus.Search
{
	public interface ISearchStatus
	{
		int lastRowMatch { get; set; }

		int lastColumnMatch { get; set; }

		int lastMatchSearchLevel { get; set; }

		string searchText { get; set; }

		string searchTextPrior { get; set; }

		/// <summary>
		/// Whether the current Search text is a superset of the prior search text (ex: the user deleted something from the front)
		/// </summary>
		/// <returns><c>true</c>, if subset of prior was ised, <c>false</c> otherwise.</returns>
		bool isSubsetOfPrior ();

		/// <summary>
		/// Whether the current Search text is a superset of the prior search text (ex: the user added something to the end)
		/// </summary>
		/// <returns><c>true</c>, if superset of prior was ised, <c>false</c> otherwise.</returns>
		bool isSupersetOfPrior ();
	}
}

