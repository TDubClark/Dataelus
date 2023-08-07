using System;
using System.Collections.Generic;

namespace Dataelus.Search
{
	public class SearchLevel : ISearchLevel
	{
		public SearchLevel ()
		{
		}

		#region ISearchLevel implementation

		protected IEqualityComparer<string> _comparer;

		public IEqualityComparer<string> comparer {
			get { return _comparer; }
			set { _comparer = value; }
		}

		protected ITextTransformer _transformer;

		public ITextTransformer transformer {
			get { return _transformer; }
			set { _transformer = value; }
		}

		protected int _searchLevel;

		public int searchLevel {
			get {
				return _searchLevel; }
			set { _searchLevel = value;
			}
		}

		#endregion
	}
}

