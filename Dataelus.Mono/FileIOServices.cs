using System;

namespace Dataelus.Mono
{
	/// <summary>
	/// File IO services (Get Input and Output Streams).
	/// </summary>
	public class FileIOServices : IFileIOServices
	{
		/// <summary>
		/// Creates a new DATAELUS logging manager.
		/// Uses the Mono/.NET Path Concatenator and a new instance of this class for the IO Services.
		/// </summary>
		/// <returns>The log mgr.</returns>
		/// <param name="directory">Directory.</param>
		public static Dataelus.Log.LogManager CreateLogMgr (string directory)
		{
			return new Dataelus.Log.LogManager (directory, new FileIOServices ());
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.FileIOServices"/> class.
		/// </summary>
		public FileIOServices ()
		{
		}

		#region IFileIOServices implementation

		/// <summary>
		/// Gets a stream to a given file, intended for output.
		/// </summary>
		/// <returns>The writer.</returns>
		/// <param name="filepath">Filepath.</param>
		/// <param name="isAppend">If set to <c>true</c> is append.</param>
		public System.IO.Stream GetStreamOut (string filepath, bool isAppend)
		{
			var fmode = System.IO.FileMode.Create;
			if (isAppend)
				fmode = System.IO.FileMode.Append;
			return new System.IO.FileStream (filepath, fmode, System.IO.FileAccess.Write);
		}

		/// <summary>
		/// Gets a stream to the given file, intended for input.
		/// </summary>
		/// <returns>The stream in.</returns>
		/// <param name="filepath">Filepath.</param>
		public System.IO.Stream GetStreamIn (string filepath)
		{
			return new System.IO.FileStream (filepath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
		}

		/// <summary>
		/// Concatenates the file paths
		/// </summary>
		/// <returns>The path concat.</returns>
		/// <param name="paths">Paths.</param>
		public string FilePathConcat (params string[] paths)
		{
			return System.IO.Path.Combine (paths);
		}

		#endregion
	}

	public static class FileServices
	{
		/// <summary>
		/// Gets the path concatenator.
		/// </summary>
		/// <returns>The path concatenator method.</returns>
		public static FilePathConcat GetPathConcat ()
		{
			return new FilePathConcat (System.IO.Path.Combine);
		}
	}
}

