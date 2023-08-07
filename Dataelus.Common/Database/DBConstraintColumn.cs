using System;

namespace Dataelus.Database
{
	public class DBConstraintColumn : IPrioritized
	{
		/// <summary>
		/// Gets or sets the column which is a FK.
		/// </summary>
		/// <value>The column.</value>
		public DBFieldSimple Column { get; set; }

		/// <summary>
		/// Gets or sets the column referenced by the FK.
		/// </summary>
		/// <value>The referenced column.</value>
		public DBFieldSimple ReferencedColumn { get; set; }

		/// <summary>
		/// Gets or sets the filter order.
		/// </summary>
		/// <value>The filter order.</value>
		public int FilterOrder { get; set; }

		public int UniqueID { get; set; }

		public DBConstraintColumn ()
			: this (null, null, 0)
		{
		}

		public DBConstraintColumn (DBFieldSimple field, DBFieldSimple referencedField)
			: this (field, referencedField, 0)
		{
		}

		public DBConstraintColumn (DBFieldSimple field, DBFieldSimple referencedField, int filterOrder)
		{
			this.Column = field;
			this.ReferencedColumn = referencedField;
			this.FilterOrder = filterOrder;
		}


		#region IPrioritized implementation

		public int PriorityIndex {
			get { return this.FilterOrder; }
			set { this.FilterOrder = value; }
		}

		#endregion

		public override string ToString ()
		{
			return string.Format ("[DBConstraintColumn: Column={0:s}, ReferencedColumn={1:s}, FilterOrder={2}]", Column, ReferencedColumn, FilterOrder, PriorityIndex);
		}

		public virtual string ToString (string format)
		{
			switch (format.ToUpperInvariant ()) {
				case "F":
					return String.Format ("{0} => {1}", Column.ToString("S"), ReferencedColumn.ToString("S"));
				default:
					throw new ArgumentOutOfRangeException ("format", String.Format ("Unrecognized format '{0}'", format));
			}
		}
	}
}

