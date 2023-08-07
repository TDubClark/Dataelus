namespace Dataelus.UI.MatchTableEditor
{
	/// <summary>
	/// View control.
	/// </summary>
	public class ViewWidget
	{
		/// <summary>
		/// Gets or sets the widget ID.
		/// </summary>
		/// <value>The widget ID.</value>
		public string WidgetID { get; set; }

		/// <summary>
		/// Gets or sets the row ID.
		/// </summary>
		/// <value>The row ID.</value>
		public long RowID { get; set; }

		/// <summary>
		/// Gets or sets the column ID.
		/// </summary>
		/// <value>The column ID.</value>
		public long ColumnID { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.UI.MatchTableEditor.ViewControlManager"/> class.
		/// </summary>
		/// <param name="widgetID">Widget ID.</param>
		/// <param name="rowID">Row ID.</param>
		/// <param name="columnID">Column ID.</param>
		public ViewWidget (string widgetID, long rowID, long columnID)
		{
			this.WidgetID = widgetID;
			this.RowID = rowID;
			this.ColumnID = columnID;
		}
	}
}
