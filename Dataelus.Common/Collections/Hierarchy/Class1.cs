using System;
using System.Linq;
using Dataelus.Extensions;

namespace Dataelus.Collections.Hierarchy
{
	public class Class1
	{
		public Class1 ()
		{
		}

		/// <summary>
		/// Function the specified collectionGroup.
		/// </summary>
		/// <param name="collections">A set of collections.</param>
		void Function (params object[] collections)
		{
			foreach (var collection in collections) {
				Type collType = collection.GetType ();

				Type itemType;
				if (collType.IsGenericCollection (out itemType)) {
					// Check the generic type
					if (!itemType.IsPrimativeExpanded ()) {
						// The first type argument is not primative

						var properties = itemType.GetProperties ();
						foreach (var prop in properties) {
							var propType = prop.PropertyType;
							foreach (var coll2 in collections) {
								if (coll2.GetType ().IsGenericCollectionOfType (propType)) {
									// This is the parent collection for that property.
								}
							}
						}
					}
				}
			}
		}


	}
}

