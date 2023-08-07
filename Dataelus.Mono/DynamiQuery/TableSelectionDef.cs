using System;
using System.Collections.Generic;

namespace Dataelus.Mono.DynamiQuery
{
	/// <summary>
	/// Table selection definition.  This defines a database table from which a selection of records is made.
	/// </summary>
	[Serializable]
	public class TableSelectionDef : IUniqueIdentifier
	{
		/// <summary>
		/// Gets or sets the database table name.
		/// </summary>
		/// <value>The table name.</value>
		public string DBTableName { get; set; }

		/// <summary>
		/// Gets or sets the database field names used for defining a selection (these would be the key fields).
		/// </summary>
		/// <value>The field names.</value>
		public List<string> DBFieldNames { get; set; }

		/// <summary>
		/// Gets or sets the order in which this item is displayed in the wizard.
		/// </summary>
		/// <value>The display order.</value>
		public int WizardDisplayOrder { get; set; }

		#region IUniqueIdentifier implementation

		/// <summary>
		/// Gets or sets the object unique ID.
		/// </summary>
		/// <value>The object unique ID.</value>
		public long ObjectUniqueID { get ; set ; }

		#endregion

		/// <summary>
		/// Clears this instance.
		/// </summary>
		public virtual void Clear ()
		{
			this.DBTableName = null;
			this.DBFieldNames.Clear ();
		}

		public TableSelectionDef ()
		{
			this.DBTableName = null;
			this.DBFieldNames = new List<string> ();
			this.WizardDisplayOrder = -1;
			this.ObjectUniqueID = int.MinValue;
		}

		public TableSelectionDef (string dBTableName, int wizardDisplayOrder)
			: this ()
		{
			this.DBTableName = dBTableName;
			this.WizardDisplayOrder = wizardDisplayOrder;
		}

		public TableSelectionDef (string dBTableName, IEnumerable<string> dBFieldNames, int wizardDisplayOrder)
			: this ()
		{
			this.DBTableName = dBTableName;
			this.DBFieldNames.AddRange (dBFieldNames);
			this.WizardDisplayOrder = wizardDisplayOrder;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.DynamiQuery.TableSelectionDef"/> class.
		/// </summary>
		/// <param name="other">The other instance, from which to copy.</param>
		public TableSelectionDef (TableSelectionDef other)
			: this ()
		{
			this.DBTableName = other.DBTableName;
			this.DBFieldNames.AddRange (other.DBFieldNames);
			this.WizardDisplayOrder = other.WizardDisplayOrder;
			this.ObjectUniqueID = other.ObjectUniqueID;
		}

		/// <summary>
		/// Copies the values from other object into this object.
		/// </summary>
		/// <param name="other">Other.</param>
		public void CopyFrom (TableSelectionDef other)
		{
			this.DBTableName = other.DBTableName;
			this.WizardDisplayOrder = other.WizardDisplayOrder;

			this.DBFieldNames.Clear ();
			this.DBFieldNames.AddRange (other.DBFieldNames);
		}
	}
}

