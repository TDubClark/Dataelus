using System;

namespace Dataelus
{
	/// <summary>
	/// Interface for a text transformer.
	/// </summary>
	public interface ITextTransformer
	{
		string Transform (string value);
	}
}

