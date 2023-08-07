using System;

namespace Dataelus.EDD.Processor
{
	public interface IValidator
	{
		EDDValidationResult Validate ();
	}

	public interface IValidatorSimple
	{
		EDDValidationResult Validate (Table.ObjectTable table);
	}

	public class ValidatorDefault : IValidator
	{


		#region IValidator implementation

		public EDDValidationResult Validate ()
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

