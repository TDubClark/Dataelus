using System;
using System.Collections.Generic;

namespace Dataelus.UI.KeyValueEditor
{
	/// <summary>
	/// Controller for a key-value editor.
	/// </summary>
	public class Controller : RecordEditor.RecordEditorController, IController
	{
		protected Dictionary<string, object> _values;

		/// <summary>
		/// Gets or sets the values.
		/// </summary>
		/// <value>The values.</value>
		public Dictionary<string, object> Values {
			get { return _values; }
			set { _values = value; }
		}

		public Controller (Dictionary<string, object> values)
			: base (GetTable (values), 0)
		{
			if (values == null)
				throw new ArgumentNullException ("values");
			
			_values = values;
		}

		/// <summary>
		/// Saves the values to the dictionary.
		/// </summary>
		protected override void SaveToDatabase ()
		{
			foreach (var item in _values) {
				_values [item.Key] = _data [_rowIndex, item.Key];
			}
		}

		/// <summary>
		/// Gets the table.
		/// </summary>
		/// <returns>The table.</returns>
		/// <param name="values">Values.</param>
		protected static Dataelus.Table.ObjectTable GetTable (Dictionary<string, object> values)
		{
			if (values == null)
				throw new ArgumentNullException ("values");
			
			var table = new Dataelus.Table.ObjectTable ();
			foreach (var item in values) {
				table.AddColumn (item.Key, item.Value.GetType ());
			}

			var row = table.CreateRow ();
			foreach (var item in values) {
				row [item.Key] = item.Value;
			}
			table.AddRow (row);

			return table;
		}
	}
}

