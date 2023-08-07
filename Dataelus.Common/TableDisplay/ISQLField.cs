using System;

namespace Dataelus.TableDisplay
{
	// Note: we should never need this

	/// <summary>
	/// Interface for a sql field.
	/// </summary>
	public interface ISQLField
	{
		/// <summary>
		/// Gets the sql component.
		/// </summary>
		/// <returns>The sql.</returns>
		String getSql();
	}
}

