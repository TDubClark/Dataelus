using System;
using System.Linq;

namespace Dataelus.Extensions
{
	/// <summary>
	/// Extensions for the Type class.
	/// </summary>
	public static class TypeExtensions
	{
		/// <summary>
		/// Determines if this type is a primative, including common non-primative types (String, DateTime, Decimal).
		/// </summary>
		/// <returns><c>true</c> if is primative expanded the specified type; otherwise, <c>false</c>.</returns>
		/// <param name="type">Type.</param>
		public static bool IsPrimativeExpanded (this Type type)
		{
			if (type == null)
				throw new NullReferenceException ("Cannot reference an undefined type");

			Type[] nonPrimativeTypes = {
				typeof(System.String),
				typeof(System.Decimal),
				typeof(System.DateTime)
			};

			return (type.IsPrimitive || nonPrimativeTypes.Contains (type));
		}

		/// <summary>
		/// Determines if this is a generic collection; if so, outputs the collectionType.
		/// </summary>
		/// <returns><c>true</c> if a generic collection; otherwise, <c>false</c>.</returns>
		/// <param name="collType">The collection type.</param>
		/// <param name="collectionItemType">The collection item type.</param>
		public static bool IsGenericCollection (this Type collType, out Type collectionItemType)
		{
			if (collType.IsGenericType) {
				// This is a generic type, like List<> or IEnumerable<>
				Type[] interfaces = collType.GetInterfaces ();

				if (interfaces.Contains (typeof(System.Collections.Generic.ICollection<>))) {
					// This is a generic collection

					Type[] typeArgs = collType.GetGenericArguments ();

					collectionItemType = typeArgs [0];
					return true;
				}
			}
			collectionItemType = null;
			return false;
		}

		/// <summary>
		/// Determines if this is a generic collection of the given itemType; ex: ICollection([itemType]).
		/// </summary>
		/// <returns><c>true</c> if this is generic collection of the specified itemType; otherwise, <c>false</c>.</returns>
		/// <param name="collType">The collection type.</param>
		/// <param name="collectionItemType">The collection item type.</param>
		public static bool IsGenericCollectionOfType (this Type collType, Type collectionItemType)
		{
			if (collType == null)
				throw new NullReferenceException ("Cannot reference an undefined type");
			if (collectionItemType == null)
				throw new ArgumentNullException ("collectionItemType");
			
			Type collectionType;
			if (collType.IsGenericCollection (out collectionType)) {
				return collectionType.Equals (collectionItemType);
			}
			return false;
		}

		/// <summary>
		/// Determines if is this type is a generic of System.Nullable(T); if so, outputs the base type.
		/// </summary>
		/// <returns><c>true</c> if this type is a generic of System.Nullable(T); otherwise, <c>false</c>.</returns>
		/// <param name="type">Type.</param>
		/// <param name="genericType">The generic type argument (T).</param>
		public static bool IsNullableType (this Type type, out Type genericType)
		{
			if (type.IsGenericType) {
				// Any generic of System.Nullable<> will have a Type.Name of "Nullable`1"
				if (type.Name.Equals (typeof(System.Nullable<>).Name)) {
					genericType = type.GetGenericArguments ().FirstOrDefault ();
					return true;
				}
			}
			genericType = null;
			return false;
		}
	}
}
