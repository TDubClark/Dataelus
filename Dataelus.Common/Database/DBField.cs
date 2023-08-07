using System;
using System.Text.RegularExpressions;

namespace Dataelus.Database
{
	/// <summary>
	/// Represents a Database field (Table name, Field name, data type information).
	/// </summary>
	public class DBField : DBFieldSimple, IDBField, IPrioritized
	{
		protected int _orderIndex;

		#region IPrioritized implementation

		public int PriorityIndex {
			get { return _orderIndex; }
			set { _orderIndex = value; }
		}

		#endregion

		public int Order {
			get { return _orderIndex; }
			set { _orderIndex = value; }
		}

		public DBField ()
			: base ()
		{
		}

		public DBField (IDBField other)
			: this ()
		{
			CopyFrom (other);
		}

		public void CopyFrom (IDBField other)
		{
			base.CopyFrom (other);
			this.DataType = other.DataType;
			this.MaxLength = other.MaxLength;
			this.Nullable = other.Nullable;
		}

		public DBField (string tableName, string fieldName, string dataType, int maxLength, bool nullable, int order)
			: base (tableName, fieldName)
		{
			Assign (dataType, maxLength, nullable, order);
		}

		public DBField (string schemaName, string tableName, string fieldName, string dataType, int maxLength, bool nullable, int order)
			: base (schemaName, tableName, fieldName)
		{
			Assign (dataType, maxLength, nullable, order);
		}

		protected void Assign (string dataType, int maxLength, bool nullable, int order)
		{
			_dataType = dataType;
			_maxLength = maxLength;
			_nullable = nullable;
			_orderIndex = order;
		}

		#region IEquatable implementation

		public bool Equals (IDBField other, System.Collections.Generic.IEqualityComparer<string> comparer)
		{
			return (
			    Equals ((IDBFieldSimple)other, comparer)
			    && (comparer.Equals (this.DataType, other.DataType))
			    && (this.MaxLength == other.MaxLength)
			    && (this.Nullable == other.Nullable)
			);
		}

		#endregion

		#region IEquatable implementation

		public bool Equals (IDBField other)
		{
			return Equals (other, new StringEqualityComparer ());
		}

		#endregion

		#region IDbField implementation

		protected string _dataType;

		public string DataType {
			get { return _dataType; }
			set { _dataType = value; }
		}

		protected int _maxLength;

		public int MaxLength {
			get { return _maxLength; }
			set { _maxLength = value; }
		}

		protected bool _nullable;

		public bool Nullable {
			get { return _nullable; }
			set { _nullable = value; }
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// Removes the brackets from the given field (if applicable), and returns.
		/// </summary>
		/// <returns>The field without the brackets.</returns>
		/// <param name="field">Field.</param>
		public static string RemoveBrackets (string field)
		{
			if (field == null)
				return field;

			var mtc = Regex.Match (field.Trim (), @"^\[(?<val>.*)\]$", RegexOptions.IgnoreCase);
			if (mtc.Success) {
				field = mtc.Groups ["val"].Value;
			}
			return field;
		}

		//		/// <summary>
		//		/// Gets whether a bracket is required for the given field.
		//		/// </summary>
		//		/// <returns><c>true</c>, if bracket required, <c>false</c> otherwise.</returns>
		//		/// <param name="field">Field.</param>
		//		public static bool BracketRequired (string field)
		//		{
		//			return (Regex.IsMatch (field, "[^A-Za-z0-9_]") || SQLLanguage.SQLLang.IsReservedWordSQLServer (field, new StringEqualityComparer ()));
		//		}
		//
		//		/// <summary>
		//		/// Enbracket the specified field.
		//		/// </summary>
		//		/// <param name="field">Field.</param>
		//		public static string Enbracket (string field)
		//		{
		//			// Replace and square brackets
		//			if (BracketRequired (field)) {
		//				field = String.Format ("[{0}]", Regex.Replace (Regex.Replace (field, @"\]", ""), @"\[", ""));
		//			}
		//			return field;
		//		}

		#endregion

		public override string ToString ()
		{
			return string.Format ("[{5} DBField: PriorityIndex={0}, Order={1}, DataType={2}, MaxLength={3}, Nullable={4}]", PriorityIndex, Order, DataType, MaxLength, Nullable, base.ToString ());
		}

		public override string ToString (string format)
		{
			// Source: https://msdn.microsoft.com/en-us/library/system.iformattable(v=vs.90).aspx

			switch (format.ToUpperInvariant ()) {
				case "T":
					// Tree View
					return String.Format ("{0} ({1}{2}, {3})", _fieldName, _dataType, (_maxLength > 0) ? String.Format ("({0:d})", _maxLength) : "", _nullable ? "null" : "not null");
				default:
					break;
			}
			return base.ToString (format);
		}
	}
}

