using System.Collections.Generic;

namespace Dataelus.Database
{
	/// <summary>
	/// DB constraint column comparer.
	/// </summary>
	public class DBConstraintColumnComparer : Comparer<DBConstraintColumn>
	{
		IDBQuerier _querier;

		/// <summary>
		/// Gets or sets the querier.
		/// </summary>
		/// <value>The querier.</value>
		public IDBQuerier Querier {
			get{ return _querier; }
			set{ _querier = value; }
		}

		Dictionary<DBFieldSimple, int> _fieldCounts;

		/// <summary>
		/// Gets or sets the field counts.
		/// </summary>
		/// <value>The field counts.</value>
		public Dictionary<DBFieldSimple, int> FieldCounts {
			get { return _fieldCounts; }
			set { _fieldCounts = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.DBConstraintColumnComparer"/> class.
		/// </summary>
		public DBConstraintColumnComparer ()
		{
			_fieldCounts = new Dictionary<DBFieldSimple, int> (new DBFieldSimpleEqualityComparer ());
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.DBConstraintColumnComparer"/> class.
		/// </summary>
		/// <param name="querier">Querier.</param>
		public DBConstraintColumnComparer (IDBQuerier querier)
		{
			_querier = querier;
			_fieldCounts = new Dictionary<DBFieldSimple, int> (new DBFieldSimpleComparer ());
		}

		#region implemented abstract members of Comparer

		/// <summary>
		/// Compare the specified x and y.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public override int Compare (DBConstraintColumn x, DBConstraintColumn y)
		{
			var fieldX = x.Column;
			var fieldY = y.Column;

			int fieldXCount;
			if (!_fieldCounts.TryGetValue (fieldX, out fieldXCount)) {
				fieldXCount = _querier.GetRecordCount (fieldX);
				_fieldCounts.Add (fieldX, fieldXCount);
			}

			int fieldYCount;
			if (!_fieldCounts.TryGetValue (fieldY, out fieldYCount)) {
				fieldYCount = _querier.GetRecordCount (fieldY);
				_fieldCounts.Add (fieldY, fieldYCount);
			}

			return fieldXCount.CompareTo (fieldYCount);
		}

		#endregion
	}
}

