using System;

namespace Dataelus.Table
{
	/// <summary>
	/// Column for an Object Table
	/// </summary>
	public class ObjectColumn : Column, IEquatable<ObjectColumn>, IEquatable<ObjectColumn, String>
	{
		protected Type _dataType;

		/// <summary>
		/// Gets or sets the type of the data.
		/// </summary>
		/// <value>The type of the data.</value>
		public Type DataType {
			get { return _dataType; }
			set { _dataType = value; }
		}

		protected TypeClass _dataTypeClass;

		/// <summary>
		/// Gets or sets the classification of the data type.
		/// </summary>
		/// <value>The data type class.</value>
		public TypeClass DataTypeClass {
			get { return _dataTypeClass; }
			set { _dataTypeClass = value; }
		}

		protected ITextInputInterpreter _textInterpreter;

		/// <summary>
		/// Gets or sets the text interpreter.
		/// </summary>
		/// <value>The text interpreter.</value>
		public ITextInputInterpreter TextInterpreter {
			get { return _textInterpreter; }
			set { _textInterpreter = value; }
		}

		protected ITextInputParser _textParser;

		/// <summary>
		/// Gets or sets the text parser.
		/// </summary>
		/// <value>The text parser.</value>
		public ITextInputParser TextParser {
			get { return _textParser; }
			set { _textParser = value; }
		}

		protected System.Collections.Generic.IEqualityComparer<object> _equalityComparer = null;

		/// <summary>
		/// Gets or sets the equality comparer.
		/// </summary>
		/// <value>The equality comparer.</value>
		public System.Collections.Generic.IEqualityComparer<object> EqualityComparer {
			get { return _equalityComparer; }
			set { _equalityComparer = value; }
		}

		/// <summary>
		/// Gets a value indicating whether this column allows null values.
		/// </summary>
		/// <value><c>true</c> if this instance is allow null; otherwise, <c>false</c>.</value>
		public bool IsAllowNull{ get { return _textInterpreter.IsAllowNull; } }

		public ObjectColumn ()
			: base ()
		{
		}

		public ObjectColumn (string columnName)
			: base (columnName)
		{
		}

		public ObjectColumn (string columnName, int index)
			: base (columnName, index)
		{
		}

		public ObjectColumn (Column other)
			: base (other)
		{
		}

		public ObjectColumn (ObjectColumn other)
			: base (other)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Table.ObjectColumn"/> class.
		/// </summary>
		/// <param name="columnName">Column name.</param>
		/// <param name="dataType">Data type (Type Class defaults to Object).</param>
		public ObjectColumn (string columnName, Type dataType)
			: this (columnName, GetClass (dataType), dataType)
		{
		}

		public ObjectColumn (string columnName, TypeClass dataTypeClass)
			: this (columnName, dataTypeClass, GetType (dataTypeClass))
		{
		}

		public ObjectColumn (string columnName, TypeClass dataTypeClass, Type dataType)
			: this (columnName, dataTypeClass, dataType
				, TextInputParser.GetParserByType (dataType)
				, TextInputInterpreter.GetInterpreterByType (dataType))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Table.ObjectColumn"/> class.
		/// </summary>
		/// <param name="columnName">Column name.</param>
		/// <param name="dataType">Data type.</param>
		/// <param name="parser">The Text Input Parser: parses the text into a new object type.</param>
		/// <param name="interpreter">Interpreter of text input: fixes any formatting issues with the text input, prior to parsing.</param>
		public ObjectColumn (string columnName, Type dataType, ITextInputParser parser, ITextInputInterpreter interpreter)
			: this (columnName, GetClass (dataType), dataType, parser, interpreter)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Table.ObjectColumn"/> class.
		/// </summary>
		/// <param name="columnName">Column name.</param>
		/// <param name="dataTypeClass">Data type class.</param>
		/// <param name="dataType">Data type.</param>
		/// <param name="parser">The Text Input Parser: parses the text into a new object type.</param>
		/// <param name="interpreter">Interpreter of text input: fixes any formatting issues with the text input, prior to parsing.</param>
		public ObjectColumn (string columnName, TypeClass dataTypeClass, Type dataType, ITextInputParser parser, ITextInputInterpreter interpreter)
			: this (columnName)
		{
			_dataTypeClass = dataTypeClass;
			_dataType = dataType;

			_textParser = parser;
			_textInterpreter = interpreter;
		}

		#region IEquatable implementation

		/// <summary>
		/// Determines whether the specified <see cref="Dataelus.Table.ObjectColumn"/> is equal to the current <see cref="Dataelus.Table.ObjectColumn"/>.
		/// </summary>
		/// <param name="other">The <see cref="Dataelus.Table.ObjectColumn"/> to compare with the current <see cref="Dataelus.Table.ObjectColumn"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="Dataelus.Table.ObjectColumn"/> is equal to the current
		/// <see cref="Dataelus.Table.ObjectColumn"/>; otherwise, <c>false</c>.</returns>
		public virtual bool Equals (ObjectColumn other)
		{
			return Equals (other, new StringEqualityComparer ());
		}

		#endregion

		/// <summary>
		/// Determines whether the specified <see cref="Dataelus.Table.ObjectColumn"/> is equal to the current <see cref="Dataelus.Table.ObjectColumn"/>.
		/// </summary>
		/// <param name="other">Other.</param>
		/// <param name="comparer">The column name comparer.</param>
		public virtual bool Equals (ObjectColumn other, System.Collections.Generic.IEqualityComparer<string> comparer)
		{
			return Equals (other, true, true, comparer);
		}

		/// <summary>
		/// Determines whether the specified <see cref="Dataelus.Table.ObjectColumn"/> is equal to the current <see cref="Dataelus.Table.ObjectColumn"/>.
		/// </summary>
		/// <param name="other">Other.</param>
		/// <param name="matchIndex">If set to <c>true</c> match the index.</param>
		/// <param name="matchID">If set to <c>true</c> match the ID.</param>
		/// <param name="comparer">Comparer.</param>
		public virtual bool Equals (ObjectColumn other, bool matchIndex, bool matchID, System.Collections.Generic.IEqualityComparer<string> comparer)
		{
			return (base.Equals (other, matchIndex, matchID, comparer)
			&& this.DataType.Equals (other.DataType));
		}


		/// <summary>
		/// Gets the Type for the given classification.
		/// </summary>
		/// <returns>The default type.</returns>
		/// <param name="dataTypeClass">Data type class.</param>
		public static Type GetType (TypeClass dataTypeClass)
		{
			switch (dataTypeClass) {
			case TypeClass.Boolean:
				return typeof(System.Boolean);
			case TypeClass.DateTime:
				return typeof(System.DateTime);
			case TypeClass.Double:
				return typeof(System.Double);
			case TypeClass.Integer:
				return typeof(System.Int32);
			case TypeClass.Object:
				return typeof(System.Object);
			case TypeClass.String:
				return typeof(System.String);
			default:
				break;
			}
			return typeof(System.Object);
		}

		/// <summary>
		/// Gets the Type Classification.
		/// </summary>
		/// <returns>The class.</returns>
		/// <param name="dataType">Data type (System.String, System.DateTime, System.Boolean; else System.Object).</param>
		public static TypeClass GetClass (Type dataType)
		{
			if (dataType.Equals (typeof(System.String)))
				return TypeClass.String;
			if (dataType.Equals (typeof(System.DateTime)))
				return TypeClass.DateTime;
			if (dataType.Equals (typeof(System.Boolean)))
				return TypeClass.Boolean;
			if (
				dataType.Equals (typeof(System.Int32)) ||
				dataType.Equals (typeof(System.Int64)) ||
				dataType.Equals (typeof(System.Int16)) ||
				dataType.Equals (typeof(System.Byte)))
				return TypeClass.Integer;
			if (dataType.Equals (typeof(System.Double)) || dataType.Equals (typeof(System.Single)))
				return TypeClass.Double;

			return TypeClass.Object;
		}
	}

	/// <summary>
	/// Data Type classifications.
	/// </summary>
	public enum TypeClass
	{
		/// <summary>
		/// The type is System.String
		/// </summary>
		String,

		/// <summary>
		/// The type is System.Boolean
		/// </summary>
		Boolean,

		/// <summary>
		/// The type is System.DateTime
		/// </summary>
		DateTime,

		/// <summary>
		/// The type is System.Double
		/// </summary>
		Double,

		/// <summary>
		/// The type is System.Int32
		/// </summary>
		Integer,

		/// <summary>
		/// The type is System.Object (generic)
		/// </summary>
		Object
	}
}

