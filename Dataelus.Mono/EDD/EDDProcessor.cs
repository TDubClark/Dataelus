using System;

namespace Dataelus.Mono.EDD
{
	/// <summary>
	/// This is the main EDD processor class.
	/// </summary>
	public class EDDProcessor
	{
		public Dataelus.Database.DBSchematic DatabaseSchematic { get; set; }

		public Dataelus.Mono.IDBQuerier2 DatabaseQuerier { get; set; }

	}
}

