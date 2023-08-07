using System;

namespace Dataelus
{
	/// <summary>
	/// Interface for a type converter.
	/// </summary>
	public interface ITypeConverter
	{
		Type GetDataType (string dbDataType);
	}
}

