using System;

namespace Dataelus.Table
{
	/// <summary>
	/// Row text collection.
	/// </summary>
	public class RowTextCollection : CollectionBase<RowText>, System.Collections.IEnumerable
	{
		public RowTextCollection ()
			: base ()
		{
		}

		public RowTextCollection (TextTable parent, System.Collections.Generic.IEnumerable<RowText> other)
			: this ()
		{
			foreach (var item in other) {
				Add (new RowText (parent, item));
			}
		}
	}
}

