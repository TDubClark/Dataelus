using System;

namespace Dataelus.Table
{
	public delegate void RowAddedEventHandler (object sender, ObjectRowAddedEventArgs args);

	/// <summary>
	/// ObjectRow added event arguments.
	/// </summary>
	public class ObjectRowAddedEventArgs : EventArgs
	{
		/// <summary>
		/// Gets or sets the row added.
		/// </summary>
		/// <value>The row added.</value>
		public ObjectRow RowAdded { get; set; }

		/// <summary>
		/// Gets or sets the index of the row.
		/// </summary>
		/// <value>The index of the row.</value>
		public int RowIndex { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Table.ObjectRowAddedEventArgs"/> class.
		/// </summary>
		/// <param name="rowAdded">Row added.</param>
		public ObjectRowAddedEventArgs (ObjectRow rowAdded)
		{
			this.RowAdded = rowAdded;
		}

		public ObjectRowAddedEventArgs (ObjectRow rowAdded, int rowIndex)
		{
			this.RowAdded = rowAdded;
			this.RowIndex = rowIndex;
		}

		public override string ToString ()
		{
			return string.Format ("[ObjectRowAddedEventArgs]");
		}
	}
}
