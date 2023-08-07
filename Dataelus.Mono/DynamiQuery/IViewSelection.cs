using System;
using System.Collections.Generic;

namespace Dataelus.Mono.DynamiQuery
{
	/// <summary>
	/// Interface for a view which allows item/record selection.
	/// </summary>
	public interface IViewSelection
	{
		IControllerSelection ControllerObject { get; set; }

		int MinSelectionCount { get; set; }

		int MaxSelectionCount { get; set; }

		void SetSelectedObjectIDs (IEnumerable<long> ids);

		void LoadForm (System.Data.DataSet dataSource);

		long[] GetSelectedObjectIDs ();

		void CloseWindow();
	}
}

