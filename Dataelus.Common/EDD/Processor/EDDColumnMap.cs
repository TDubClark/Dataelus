using System;

namespace Dataelus.EDD.Processor
{
	public class EDDColumnMap
	{
		public string SourceColumnName { get; set; }

		public string TargetColumnName { get; set; }
	}
	
	public class EDDColumnMapCollection : ListBase<EDDColumnMap>
	{

	}
}
