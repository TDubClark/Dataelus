using System;

namespace Dataelus.Mono.Extensions
{
	/// <summary>
	/// Extensions to System.Object and any collections thereof.
	/// </summary>
	public static class ObjectDataExtensions
	{
		// Methods: DataTable Creators :: getting the name, value, and type of public properties in an object
		// Source: http://stackoverflow.com/questions/957783/loop-through-an-objects-properties-in-c-sharp

		public static Dataelus.Table.ObjectTable ToObjectTable (this System.Data.DataTable dt)
		{
			return DataServices.GetObjectTable (dt);
		}

		public static Dataelus.Table.ObjectTable ToObjectTable (this System.Data.DataSet ds)
		{
			return DataServices.GetObjectTable (ds);
		}


		/// <summary>
		/// Determines if this Type is irreducible - it cannot be reduced to other types.
		/// Irreducible types include System.String and System.DateTime
		/// </summary>
		/// <returns><c>true</c> if is irreducable type the specified objectType; otherwise, <c>false</c>.</returns>
		/// <param name="objectType">Object type.</param>
		public static bool IsIrreducibleType (this Type objectType)
		{
			if (objectType.IsPrimitive
			    || objectType.Equals (typeof(String))
			    || objectType.Equals (typeof(DateTime))) {
				return true;
			}
			return false;
		}

		public const string ValueColumnName = "Value";

		#region Methods: DataTable Creators

		/// <summary>
		/// Gets a blank data table from the properties of this object.
		/// </summary>
		/// <returns>The data table blank.</returns>
		/// <param name="value">Value.</param>
		public static System.Data.DataTable ToDataTableBlank (this object value)
		{
			var valueType = value.GetType ();
			if (valueType.IsIrreducibleType ()) {
				return GetDataTableBlank (valueType);
			} else {
				var propInfo = valueType.GetProperties ();
				return GetDataTableBlank (propInfo);
			}
		}

		/// <summary>
		/// Gets a blank data table from the properties of this object.
		/// </summary>
		/// <returns>The data table blank.</returns>
		/// <param name="value">Value.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static System.Data.DataTable ToDataTableBlank<T> (this T value)
		{
			if (typeof(T).IsIrreducibleType ()) {
				return GetDataTableBlank (typeof(T));
			} else {
				var propInfo = typeof(T).GetProperties ();
				return GetDataTableBlank (propInfo);
			}
		}

		/// <summary>
		/// Gets the data table for the given irreducible type.
		/// </summary>
		/// <returns>The data table blank.</returns>
		/// <param name="irreducibleType">Irreducible type.</param>
		static System.Data.DataTable GetDataTableBlank (Type irreducibleType)
		{
			var dt = new System.Data.DataTable ();
			dt.Columns.Add (new System.Data.DataColumn (ValueColumnName, irreducibleType));
			return dt;
		}

		/// <summary>
		/// Gets the data table for the given property info.
		/// </summary>
		/// <returns>The data table.</returns>
		/// <param name="propInfo">Property info.</param>
		public static System.Data.DataTable GetDataTableBlank (System.Reflection.PropertyInfo[] propInfo)
		{
			var dt = new System.Data.DataTable ();
			for (int i = 0; i < propInfo.Length; i++) {
				var prop = propInfo [i];
				string name = prop.Name;
				Type propertyType = prop.PropertyType;
				//object propertyValue = prop.GetValue (value, null);
				dt.Columns.Add (new System.Data.DataColumn (name, propertyType));
			}
			return dt;
		}

		/// <summary>
		/// Creates a one-record data table from the properties of this object.
		/// </summary>
		/// <returns>The data table.</returns>
		/// <param name="value">Value.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static System.Data.DataTable ToDataTable<T> (this T value)
		{
			var dt = value.ToDataTableBlank ();

			value.AddToDataTable (dt);

			return dt;
		}

		/// <summary>
		/// Adds the properties of the given object to data table.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="dt">Dt.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void AddToDataTable<T> (this T value, System.Data.DataTable dt)
		{
			if (typeof(T).IsIrreducibleType ()) {
				AddValueToDataTable (value, dt);
			} else {
				System.Reflection.PropertyInfo[] propInfo = typeof(T).GetProperties ();

				AddValueToDataTable (value, dt, propInfo);
			}
		}

		/// <summary>
		/// Adds the properties of the given object to data table.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="dt">Dt.</param>
		public static void AddToDataTable (this object value, System.Data.DataTable dt)
		{
			if (value.GetType ().IsIrreducibleType ()) {
				AddValueToDataTable (value, dt);
			} else {
				System.Reflection.PropertyInfo[] propInfo = value.GetType ().GetProperties ();

				AddValueToDataTable (value, dt, propInfo);
			}
		}

		/// <summary>
		/// Adds the value to data table, to the column "Value".
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="dt">Dt.</param>
		static void AddValueToDataTable (object value, System.Data.DataTable dt)
		{
			var dr = dt.NewRow ();
			dr [ValueColumnName] = value;
			dt.Rows.Add (dr);
		}

		static void AddValueToDataTable<T> (T value, System.Data.DataTable dt, System.Reflection.PropertyInfo[] propInfo)
		{
			var dr = dt.NewRow ();
			for (int i = 0; i < propInfo.Length; i++) {
				var prop = propInfo [i];
				string name = prop.Name;

				object propertyValue = null;

				try {
					propertyValue = prop.GetValue (value, null).ToNullable ();
				} catch (Exception ex) {
					throw new Exception (String.Format ("Error when getting the value of property '{0}'.", name), ex);
				}

				try {
					dr [name] = propertyValue;
				} catch (Exception ex) {
					throw new Exception (String.Format ("Error when assigning the value {{ {1} }} to DataColumn '{0}'.", name, propertyValue), ex);
				}
			}
			dt.Rows.Add (dr);
		}

		/// <summary>
		/// Creates a Data Table from the properties of the given object
		/// </summary>
		/// <returns>The data table.</returns>
		/// <param name="collection">Collection.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static System.Data.DataTable ToDataTable<T> (this System.Collections.Generic.IEnumerable<T> collection)
		{
			System.Data.DataTable dt = null;
			foreach (var item in collection) {
				if (dt == null)
					dt = item.ToDataTableBlank ();
				item.AddToDataTable (dt);
			}
			return dt;
		}

		/// <summary>
		/// Creates a Data Table from the properties of the given object
		/// </summary>
		/// <returns>The data table.</returns>
		/// <param name="collection">Collection.</param>
		public static System.Data.DataTable ToDataTable (this System.Collections.IEnumerable collection)
		{
			System.Data.DataTable dt = null;
			foreach (var item in collection) {
				if (dt == null)
					dt = item.ToDataTableBlank ();
				item.AddToDataTable (dt);
			}
			return dt;
		}

		#endregion

		#region Methods: Nullable Type Handlers

		/// <summary>
		/// Gets the nullable value (if DBNull, returns Null; else returns [this]).
		/// </summary>
		/// <returns>The nullable value.</returns>
		/// <param name="value">Value.</param>
		public static object ToNullable (this object value)
		{
			if (value == null)
				return null;
			if (System.DBNull.Value.Equals (value))
				return null;
			return value;
		}

		/// <summary>
		/// Gets the DB nullable value (if Null, returns DBNull; else returns [this]).
		/// </summary>
		/// <returns>The DB nullable value.</returns>
		/// <param name="value">Value.</param>
		public static object ToDBNullable (this object value)
		{
			if (value == null)
				return System.DBNull.Value;
			return value;
		}

		/// <summary>
		/// Gets the nullable string (returns NULL if this is Null or DBNull).
		/// </summary>
		/// <returns>The nullable string.</returns>
		/// <param name="value">Value.</param>
		public static string ToNullableString (this object value)
		{
			if (value == null)
				return null;
			if (System.DBNull.Value.Equals (value))
				return null;
			return value.ToString ();
		}

		/// <summary>
		/// Gets the nullable string (returns [defaultValue] if this is Null or DBNull).
		/// </summary>
		/// <returns>The nullable string.</returns>
		/// <param name="value">Value.</param>
		/// <param name="defaultValue">The default value (if value is Null or DBNull)</param>
		public static string ToNullableString (this object value, string defaultValue)
		{
			return value.ToNullableString () ?? defaultValue;
		}

		/// <summary>
		/// Gets the nullable value of the given type; if Null or DBNull, returns the default value.
		/// </summary>
		/// <returns>The nullable.</returns>
		/// <param name="value">Value.</param>
		/// <param name="defaultValue">Default value.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T ToNullable<T> (this object value, T defaultValue)
		{
			return ToNullable (value) == null ? defaultValue : (T)value;
		}

		/// <summary>
		/// Gets this string as either DBNull or the output of the given parser function.
		/// </summary>
		/// <returns>The DB nullable parsable.</returns>
		/// <param name="value">Value.</param>
		/// <param name="parser">Parser.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static object ToDBNullableParsable<T> (this string value, TryParseFunc<T> parser)
		{
			if (String.IsNullOrWhiteSpace (value))
				return DBNull.Value;
			
			T result;
			return parser (value, out result) ? (object)result : DBNull.Value;
		}

		#endregion
	}

	public delegate bool TryParseFunc<T> (string value, out T result);
}

