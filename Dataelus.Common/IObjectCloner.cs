using System;

namespace Dataelus
{
	public interface IObjectCloner<T>
	{
		T GetClone (T value);
	}

	public class ObjectClonerDefault<T> : IObjectCloner<T>
	{
		#region IObjectCloner implementation

		public T GetClone (T value)
		{
			return value;
		}

		#endregion
	}
}

