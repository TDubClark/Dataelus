using System;

namespace Dataelus.FilterCascade
{
	public abstract class FilterDataBase : NonTextFilter
	{
		protected FilterDataBase ()
			: base ()
		{
		}

		protected FilterDataBase (Table.TypeClass dataTypeClass)
			: base (dataTypeClass)
		{
		}
	}
}

