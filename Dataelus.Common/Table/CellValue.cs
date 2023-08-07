using System;

namespace Dataelus.Table
{
	public class CellValue : Grid.GridCellBase, ICellValue
	{
		public CellValue ()
			: base ()
		{
		}

		public CellValue (int row, int column, object value)
			: base (row, column)
		{
			_value = value;
		}

		protected object _value;

		public object Value {
			get { return _value; }
			set { _value = value; }
		}
	}
}

