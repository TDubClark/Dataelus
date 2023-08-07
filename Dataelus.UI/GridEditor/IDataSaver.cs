using System;

namespace Dataelus.UI.GridEditor
{
	/// <summary>
	/// Interface for a class which can save a Data object of the given type.
	/// </summary>
	public interface IDataSaver<D>
	{
		void SaveData (D data);
	}
}

