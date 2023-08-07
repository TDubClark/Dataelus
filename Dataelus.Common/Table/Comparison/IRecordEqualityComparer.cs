using System;

namespace Dataelus.Table.Comparison
{
	public interface IRecordEqualityComparer
	{
		bool RecordEquals (object table1Record, object table2Record);
	}
}

