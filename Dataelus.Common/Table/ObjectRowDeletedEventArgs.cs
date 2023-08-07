using System;

namespace Dataelus.Table
{
	public delegate void RowDeletedEventHandler (object sender, ObjectRowDeletedEventArgs args);

	/// <summary>
	/// ObjectRow deleted event arguments.
	/// </summary>
	public class ObjectRowDeletedEventArgs : EventArgs
	{
		/// <summary>
		/// Gets or sets the row deleted.
		/// </summary>
		/// <value>The row deleted.</value>
		public ObjectRow RowDeleted { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Table.ObjectRowDeletedEventArgs"/> class.
		/// </summary>
		/// <param name="rowDeleted">Row deleted.</param>
		public ObjectRowDeletedEventArgs (ObjectRow rowDeleted)
		{
			this.RowDeleted = rowDeleted;
		}

		public override string ToString ()
		{
			return string.Format ("[ObjectRowDeletedEventArgs]");
		}
	}
}

