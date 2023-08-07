using System;

namespace Dataelus.Table.Comparison.Generic
{
	public interface IRecordEqualityComparer<R>
	{
		bool RecordEquals (R table1Record, R table2Record);
	}
}

