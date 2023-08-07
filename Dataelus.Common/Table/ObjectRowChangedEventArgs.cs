using System;

namespace Dataelus.Table
{
	/// <summary>
	/// Row changed event handler.
	/// </summary>
	public delegate void RowChangedEventHandler (object sender, ObjectRowChangedEventArgs args);

	/// <summary>
	/// ObjectRow changed event arguments.
	/// </summary>
	public class ObjectRowChangedEventArgs
	{
		/// <summary>
		/// Gets or sets the row changed.
		/// </summary>
		/// <value>The row changed.</value>
		public ObjectRow RowChanged { get; set; }

		/// <summary>
		/// Gets or sets the column changed.
		/// </summary>
		/// <value>The column changed.</value>
		public ObjectColumn ColumnChanged { get; set; }

		/// <summary>
		/// Gets or sets the prior value.
		/// </summary>
		/// <value>The prior value.</value>
		public object PriorValue { get; set; }

		/// <summary>
		/// Gets or sets the new value.
		/// </summary>
		/// <value>The new value.</value>
		public object NewValue { get; set; }

		public ObjectRowChangedEventArgs ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Table.ObjectRowChangedEventArgs"/> class.
		/// </summary>
		/// <param name="rowChanged">Row changed.</param>
		/// <param name="columnChanged">Column changed.</param>
		public ObjectRowChangedEventArgs (ObjectRow rowChanged, ObjectColumn columnChanged)
		{
			this.RowChanged = rowChanged;
			this.ColumnChanged = columnChanged;
		}

		public ObjectRowChangedEventArgs (ObjectRow rowChanged, ObjectColumn columnChanged, object priorValue, object newValue)
		{
			this.RowChanged = rowChanged;
			this.ColumnChanged = columnChanged;
			this.PriorValue = priorValue;
			this.NewValue = newValue;
		}
		

		public override string ToString ()
		{
			return string.Format ("[ObjectRowChangedEventArgs: RowChanged={0}, ColumnChanged={1}]", RowChanged, ColumnChanged);
		}
	}
}

