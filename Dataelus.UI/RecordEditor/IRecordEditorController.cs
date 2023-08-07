namespace Dataelus.UI.RecordEditor
{
	public interface IRecordEditorController
	{
		object RecordData { get; set; }

		void LoadView ();

		void Save ();

		bool ValidateValue (string fieldName, object value, out string message);

		bool ValidateValue (int fieldID, object value, out string message);
	}

	public interface IValueCollection
	{
		/// <summary>
		/// Gets or sets a value indicating whether the value collection is a suggestion or a constraint.
		/// </summary>
		/// <value><c>true</c> if this instance is suggestion; otherwise, <c>false</c>.</value>
		bool IsSuggestion { get; set; }

		/// <summary>
		/// Gets or sets the fieldname of the ID column.
		/// </summary>
		/// <value>The fieldname ID.</value>
		string FieldnameID { get; set; }

		/// <summary>
		/// Gets or sets the fieldname of the Value column.
		/// </summary>
		/// <value>The fieldname value.</value>
		string FieldnameValue { get; set; }

		/// <summary>
		/// Gets or sets the fieldname of the Display column.
		/// </summary>
		/// <value>The fieldname display.</value>
		string FieldnameDisplay { get; set; }
	}
}

