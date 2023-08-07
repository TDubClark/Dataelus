using System;
using System.Collections.Generic;

namespace Dataelus.Search
{
	public interface ISearchLevel
	{
		IEqualityComparer<string> comparer { get; set; }

		ITextTransformer transformer { get; set; }

		int searchLevel { get; set; }
	}
}

