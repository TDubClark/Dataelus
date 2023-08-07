using System;
using System.Collections.Generic;

namespace Dataelus.Table
{
	/// <summary>
	/// Row text object (second implementation, with more flexibility about the parent table).
	/// </summary>
	public class RowText2 : RowText
	{
		protected ITable _parent;

		protected override int FindColumnIndex (string columnName)
		{
			return _parent.FindColumnIndex (columnName);
		}

		public RowText2 (ITable parent, IList<object> values)
			: base (values.Count)
		{
			_parent = parent;

			for (int i = 0; i < values.Count; i++) {
				_values [i] = values [i].ToString ();
			}
		}
	}
}

