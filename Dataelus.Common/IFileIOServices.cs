using System;

namespace Dataelus
{
	/// <summary>
	/// Interface for File IO services.
	/// </summary>
	public interface IFileIOServices
	{
		/// <summary>
		/// Gets a stream to a given file, intended for output.
		/// </summary>
		/// <returns>The writer.</returns>
		/// <param name="filepath">Filepath.</param>
		/// <param name="isAppend">If set to <c>true</c> appending to the file.</param>
		System.IO.Stream GetStreamOut (string filepath, bool isAppend);

		/// <summary>
		/// Gets a stream to the given file, intended for input.
		/// </summary>
		/// <returns>The stream in.</returns>
		/// <param name="filepath">Filepath.</param>
		System.IO.Stream GetStreamIn (string filepath);

		/// <summary>
		/// Concatenates the file paths
		/// </summary>
		/// <returns>The path concat.</returns>
		/// <param name="paths">Paths.</param>
		string FilePathConcat (params string[] paths);
	}
}

