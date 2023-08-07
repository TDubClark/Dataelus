using System;

namespace Dataelus.Database
{
	/// <summary>
	/// Represents a Database field (simple - just names of Schema, Table, Field).
	/// </summary>
	public class DBFieldSimple : IDBFieldSimple //, IFormattable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.DBFieldSimple"/> class.
		/// </summary>
		public DBFieldSimple ()
			: this (null, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.DBFieldSimple"/> class.
		/// </summary>
		/// <param name="tableName">Table name.</param>
		/// <param name="fieldName">Field name.</param>
		public DBFieldSimple (string tableName, string fieldName)
			: this (null, tableName, fieldName)
		{
		}

		public DBFieldSimple (string schemaName, string tableName, string fieldName)
		{
			_schemaName = schemaName;
			_tableName = tableName;
			_fieldName = fieldName;
		}

		public DBFieldSimple (IDBFieldSimple other)
		{
			CopyFrom (other);
		}

		public void CopyFrom (IDBFieldSimple other)
		{
			this.SchemaName = other.SchemaName;
			this.TableName = other.TableName;
			this.FieldName = other.FieldName;
		}

		#region IEquatable implementation

		public bool Equals (IDBFieldSimple other, System.Collections.Generic.IEqualityComparer<string> comparer)
		{
			return (
			    comparer.Equals (((IDBFieldSimple)this).SchemaName, other.SchemaName)
			    && comparer.Equals (((IDBFieldSimple)this).TableName, other.TableName)
			    && comparer.Equals (((IDBFieldSimple)this).FieldName, other.FieldName)
			);
		}

		#endregion

		#region IEquatable implementation

		public bool Equals (IDBFieldSimple other)
		{
			return Equals (other, new StringEqualityComparer ());
		}

		#endregion

		#region IDbFieldSimple implementation

		protected string _schemaName;

		public string SchemaName {
			get { return _schemaName; }
			set { _schemaName = value; }
		}

		protected string _tableName;

		public string TableName {
			get { return _tableName; }
			set { _tableName = value; }
		}

		protected string _fieldName;

		public string FieldName {
			get {
				return _fieldName; }
			set { _fieldName = value;
			}
		}

		#endregion

		public bool EqualsOrException (IDBFieldSimple other, System.Collections.Generic.IEqualityComparer<string> comparer)
		{
			if (!comparer.Equals (((IDBFieldSimple)this).SchemaName, other.SchemaName)) {
				throw new Exception (String.Format ("This Schema != Other Schema ('{0}' (this) != '{1}' (other))", ((IDBFieldSimple)this).SchemaName, other.SchemaName));
			}
			if (!comparer.Equals (((IDBFieldSimple)this).TableName, other.TableName)) {
				throw new Exception (String.Format ("This Table != Other Table ('{0}' (this) != '{1}' (other))", ((IDBFieldSimple)this).TableName, other.TableName));
			}
			if (!comparer.Equals (((IDBFieldSimple)this).FieldName, other.FieldName)) {
				throw new Exception (String.Format ("This Field != Other Field ('{0}' (this) != '{1}' (other))", ((IDBFieldSimple)this).FieldName, other.FieldName));
			}
			return true;
		}

		public override string ToString ()
		{
			return ToString ("L");
		}

		public virtual string ToString (string format)
		{
			// Source: https://msdn.microsoft.com/en-us/library/system.iformattable(v=vs.90).aspx

			switch (format.ToUpperInvariant ()) {
				case "L":
					return String.Format ("[DBFieldSimple: SchemaName={0}, TableName={1}, FieldName={2}]", SchemaName, TableName, FieldName);
				case "S":
					// Standard: Table.Field
					return String.Format ("{0}.{1}", TableName, FieldName);
				case "T":
					// Tree View
					return String.Format ("{0}", FieldName);
					break;
				default:
					throw new FormatException (String.Format ("Format string '{0}' is not supported.", format));
			}
		}

		//		#region IFormattable implementation
		//
		//		public string ToString (string format, IFormatProvider formatProvider)
		//		{
		//		}
		//
		//		#endregion
	}

	/// <summary>
	/// DB field simple json converter.  (NOT TESTED)
	/// </summary>
	public class DBFieldSimpleJsonConverter : Newtonsoft.Json.JsonConverter
	{
		#region implemented abstract members of JsonConverter

		public override void WriteJson (Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
		{
			// Source: http://stackoverflow.com/questions/17856580/newtonsoft-json-custom-serialization-behavior-for-datetime

			// NOTE: this code is not tested
			var result = new Newtonsoft.Json.Linq.JObject ();

			DBFieldSimple item = (DBFieldSimple)value;

			result.Add ("FieldName", Newtonsoft.Json.Linq.JToken.FromObject (item.FieldName));
			result.Add ("SchemaName", Newtonsoft.Json.Linq.JToken.FromObject (item.SchemaName));
			result.Add ("TableName", Newtonsoft.Json.Linq.JToken.FromObject (item.TableName));

			result.WriteTo (writer);
		}

		public override object ReadJson (Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
		{
			// http://michaelcummings.net/mathoms/using-a-custom-jsonconverter-to-fix-bad-json-results/
			return serializer.Deserialize (reader, objectType);
		}

		public override bool CanConvert (Type objectType)
		{
			if (objectType.GetType ().Equals (typeof(DBFieldSimple)))
				return true;
			return false;
		}

		#endregion
	}
}

