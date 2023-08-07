using System;

namespace Dataelus.EDD
{
	/// <summary>
	/// Edd field.
	/// </summary>
	public class EDDField : IEDDField
	{
		/// <summary>
		/// Gets or sets the text interpretter.
		/// Interprets the text to other (properly-formatted) text.
		/// Ex: If the user enters "1/2" for a date, this would convert to "01/02/2014".
		/// </summary>
		/// <value>The text interpretter.</value>
		public ITextInputInterpreter TextInterpretter { get; set; }

		/// <summary>
		/// Gets or sets the text parser.
		/// Parses the text to an object.
		/// </summary>
		/// <value>The text parser.</value>
		public ITextInputParser TextParser { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.EDD.EDDField"/> class.
		/// </summary>
		public EDDField ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.EDD.EDDField"/> class.
		/// </summary>
		/// <param name="eddColumnName">Edd column name.</param>
		public EDDField (string eddColumnName)
			: this ()
		{
			_eddColumnName = eddColumnName;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.EDD.EddField"/> class.
		/// </summary>
		/// <param name="eddColumnName">Edd column name.</param>
		/// <param name="dbDataField">Database data field.</param>
		public EDDField (string eddColumnName, Dataelus.Database.IDBField dbDataField)
			: this (eddColumnName)
		{
			_dbDataField = dbDataField;
		}

		/// <summary>
		/// (Copy Constructor) Initializes a new instance of the <see cref="Dataelus.EDD.EDDField"/> class, copied from the given object.
		/// </summary>
		/// <param name="other">The object from which to copy.</param>
		public EDDField (IEDDField other)
			: this (other.EDDColumnName)
		{
			if (other == null)
				throw new ArgumentNullException ("other");
			
			_dbDataField = new Database.DBField (other.DBDataField);
			_dbUploadField = new Database.DBField (other.DBUploadField);
			_dbReferenceField = new Database.DBField (other.DBReferenceField);
		}

		/// <summary>
		/// (Copy Constructor) Initializes a new instance of the <see cref="Dataelus.EDD.EDDField"/> class, copied from the given object.
		/// </summary>
		/// <param name="other">The object from which to copy.</param>
		public EDDField (EDDField other)
			: this ((IEDDField)other)
		{
			this.TextInterpretter = other.TextInterpretter;
			this.TextParser = other.TextParser;
		}

		#region IEddField implementation

		protected string _eddColumnName;

		/// <summary>
		/// Gets or sets the name of the edd column.
		/// </summary>
		/// <value>The name of the edd column.</value>
		public string EDDColumnName {
			get { return _eddColumnName; }
			set { _eddColumnName = value; }
		}

		protected Dataelus.Database.IDBField _dbDataField;

		/// <summary>
		/// Gets or sets the database data field.
		/// </summary>
		/// <value>The db data field.</value>
		public Dataelus.Database.IDBField DBDataField {
			get { return _dbDataField; }
			set { _dbDataField = value; }
		}

		protected Dataelus.Database.IDBField _dbUploadField;

		/// <summary>
		/// Gets or sets the database upload field.
		/// </summary>
		/// <value>The db upload field.</value>
		public Dataelus.Database.IDBField DBUploadField {
			get { return _dbUploadField; }
			set { _dbUploadField = value; }
		}

		protected Dataelus.Database.IDBField _dbReferenceField;

		/// <summary>
		/// Gets or sets the database reference field.
		/// </summary>
		/// <value>The db reference field.</value>
		public Dataelus.Database.IDBField DBReferenceField {
			get { return _dbReferenceField; }
			set { _dbReferenceField = value; }
		}

		#endregion

		public override string ToString ()
		{
			return string.Format ("[EDDField: TextInterpretter={0}, TextParser={1}, EDDColumnName={2}, DBDataField={3}, DBUploadField={4}, DBReferenceField={5}]", TextInterpretter, TextParser, EDDColumnName, DBDataField, DBUploadField, DBReferenceField);
		}
	}
}

