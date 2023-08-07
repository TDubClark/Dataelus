using System;

namespace Dataelus.EDD
{
	/// <summary>
	/// Edd record validation rule (abstract).
	/// </summary>
	public abstract class EDDRecordValidationRule : IEDDRecordValidationRule
	{
		protected EDDRecordValidationRule ()
		{
		}

		#region IEddRecordValidationRule implementation

		public abstract bool IsValid (Dataelus.Table.IRecordValueCollection values);

		public abstract ITableValidationResult Validate (Dataelus.Table.IRecordValueCollection values);

		protected string[] _columnNames;

		public virtual string[] ColumnNames {
			get { return _columnNames; }
			set { _columnNames = value; }
		}

		protected int[] _columnIDs;

		public virtual int[] ColumnIDs {
			get { return _columnIDs; }
			set { _columnIDs = value; }
		}

		#endregion
	}
}

