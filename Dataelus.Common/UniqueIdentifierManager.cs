using System;

namespace Dataelus
{
	/// <summary>
	/// Unique identifier manager.
	/// </summary>
	public class UniqueIdentifierManager
	{
		protected long _priorUniqueID;

		/// <summary>
		/// Gets or sets the prior unique ID.
		/// </summary>
		/// <value>The prior unique ID.</value>
		public long PriorUniqueID {
			get { return _priorUniqueID; }
			set { _priorUniqueID = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.UniqueIdentifierManager"/> class; 
		/// </summary>
		/// <param name="seed">Seed.</param>
		public UniqueIdentifierManager (long seed)
		{
			_priorUniqueID = seed - 1;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.UniqueIdentifierManager"/> class; starts prior index at -1.
		/// </summary>
		public UniqueIdentifierManager ()
			: this (0)
		{
		}

		/// <summary>
		/// Gets a new unique ID.
		/// </summary>
		/// <returns>The unique ID.</returns>
		public long GetUniqueID ()
		{
			_priorUniqueID++;
			return _priorUniqueID;
		}

		/// <summary>
		/// Gets a new unique ID as an integer (32-bit).
		/// </summary>
		/// <returns>The unique identifier as an integer.</returns>
		public int GetUniqueIDAsInt ()
		{
			return (int)GetUniqueID ();
		}
	}
}

