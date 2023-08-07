using System;

namespace Dataelus.UI
{
	/// <summary>
	/// Defines how a field of an object is viewed.
	/// </summary>
	public class ObjectFieldView
	{
		public string FieldViewTitle { get; set; }

		public int FieldOrder { get; set; }

		public Func<object, string> FieldText { get; set; }

		public ObjectFieldView (string fieldViewTitle, int fieldOrder, Func<object, string> fieldText)
		{
			this.FieldViewTitle = fieldViewTitle;
			this.FieldOrder = fieldOrder;
			this.FieldText = fieldText;
		}
	}
}

