using System;

namespace Dataelus.Database
{
	/// <summary>
	/// Association types between two database fields.
	/// </summary>
	public enum FieldAssociationType
	{
		/// <summary>
		/// The associated field holds descriptions for the primary field.
		/// </summary>
		DescriptionField,

		/// <summary>
		/// The associated field holds the combobox display value for the primary field.
		/// </summary>
		ComboDisplayField,

		/// <summary>
		/// The associated field holds the unit for values in the primary field.
		/// </summary>
		UnitField,

		/// <summary>
		/// The associated field holds the date/time stamp for values in the primary field.
		/// </summary>
		DateStampField
	}

	/// <summary>
	/// Stores an association between two Database fields.
	/// </summary>
	public class DBFieldAssociation
	{
		/// <summary>
		/// Gets or sets the primary field.
		/// </summary>
		/// <value>The field.</value>
		public DBFieldSimple Field {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the field associated with the primary field.
		/// </summary>
		/// <value>The associated field.</value>
		public DBFieldSimple AssociatedField {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the type of the association - what relation does the AssociatedField have to the primary Field.
		/// </summary>
		/// <value>The type of the association.</value>
		public FieldAssociationType AssociationType {
			get;
			set;
		}

		public DBFieldAssociation ()
		{
		}

		public DBFieldAssociation (DBFieldSimple field, DBFieldSimple associatedField, FieldAssociationType assocType)
		{
			this.Field = field;
			this.AssociatedField = associatedField;
			this.AssociationType = assocType;
		}
	}

	/// <summary>
	/// Queries a pair of database fields
	/// </summary>
	public class DBFieldPairQuery
	{
		/// <summary>
		/// Gets or sets the value field.
		/// </summary>
		/// <value>The value field.</value>
		public IFieldQuery ValueField {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the assoc field.
		/// </summary>
		/// <value>The assoc field.</value>
		public IFieldQuery AssocField {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the SQL FROM-clause.
		/// </summary>
		/// <value>The SQL from.</value>
		public string SQLFrom { get; set; }

		public DBFieldPairQuery ()
		{
		}
	}


	public interface IFieldQuery
	{
		/// <summary>
		/// Gets the sql field.
		/// </summary>
		/// <returns>The sql field.</returns>
		string GetSqlField ();

		/// <summary>
		/// Gets or sets the Mono/.NET Type string.
		/// </summary>
		/// <value>The mono type string.</value>
		string MonoTypeString { get; set; }
	}
}

