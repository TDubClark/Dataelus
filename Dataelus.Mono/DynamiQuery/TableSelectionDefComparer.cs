using System;
using System.Collections.Generic;

namespace Dataelus.Mono.DynamiQuery
{

	internal class TableSelectionDefComparer : Comparer<TableSelectionDef>
	{
		#region implemented abstract members of Comparer

		public override int Compare (TableSelectionDef x, TableSelectionDef y)
		{
			return x.WizardDisplayOrder.CompareTo (y.WizardDisplayOrder);
		}

		#endregion
	}
}
