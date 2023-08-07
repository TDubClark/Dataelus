using System;
using System.Collections.Generic;

namespace Dataelus.Mono.DynamiQuery
{
	/// <summary>
	/// Table selection definition, including user interface definition.
	/// </summary>
	[Serializable]
	public class TableSelectionUIDef : TableSelectionDef
	{
		/// <summary>
		/// Gets or sets the name of the database field on which search can be conducted (if not exclusively, then by default).
		/// </summary>
		/// <value>The name of the search DB field.</value>
		public string SearchDBFieldName { get; set; }

		/// <summary>
		/// Gets or sets the display field names for each Database field name (key=Database Field; value=Display Name).
		/// </summary>
		/// <value>The display field names.</value>
		public Dictionary<string, string> DisplayFieldNames { get; set; }

		public override void Clear ()
		{
			base.Clear ();
			this.SearchDBFieldName = null;
			this.DisplayFieldNames.Clear ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.DynamiQuery.TableSelectionUIDef"/> class.
		/// </summary>
		public TableSelectionUIDef ()
			: base ()
		{
			this.SearchDBFieldName = null;
			this.DisplayFieldNames = new Dictionary<string, string> ();
		}

		public TableSelectionUIDef (TableSelectionDef other) : base (other)
		{
			this.SearchDBFieldName = null;
			this.DisplayFieldNames = new Dictionary<string, string> ();
		}

		public TableSelectionUIDef (TableSelectionUIDef other) : base (other)
		{
			this.SearchDBFieldName = other.SearchDBFieldName;
			this.DisplayFieldNames = new Dictionary<string, string> (other.DisplayFieldNames);
		}
	}
}

