using System;

namespace Dataelus.HMVC
{
	/// <summary>
	/// Interface for the controller master.
	/// </summary>
	public interface IControllerMaster : IControllerBase
	{
		bool IsMaster{ get; }
	}
}

