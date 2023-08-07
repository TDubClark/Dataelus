using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Dataelus
{
	/// <summary>
	/// Application Access class.
	/// </summary>
	public class AppAccess
	{
		/// <summary>
		/// Gets or sets a value indicating whether Debug access is allowed.
		/// </summary>
		/// <value><c>true</c> if debug access; otherwise, <c>false</c>.</value>
		public bool DebugAccess{ get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether Global access is allowed.
		/// </summary>
		/// <value><c>true</c> if global access; otherwise, <c>false</c>.</value>
		public bool GlobalAccess{ get; set; }

		/// <summary>
		/// Gets or sets the access types.
		/// </summary>
		/// <value>The access types.</value>
		public List<string> AccessTypes{ get; set; }

		/// <summary>
		/// Loads this object from the given File Lines and the given Access code.
		/// </summary>
		/// <param name="accessFileLines">Access file lines.</param>
		/// <param name="accessPasscode">Access passcode.</param>
		public void Load (List<string> accessFileLines, string accessPasscode)
		{
			if (accessFileLines == null)
				throw new ArgumentNullException ("accessFileLines");

			if (accessFileLines.Count == 0)
				return;
			if (accessFileLines [0].Equals (accessPasscode)) {
				var lineColl = new StringCollection (accessFileLines);
				this.DebugAccess = lineColl.MatchPattern ("^debug[=](1|y|true)", RegexOptions.IgnoreCase);
				this.GlobalAccess = lineColl.MatchPattern ("^global[=](1|y|true)", RegexOptions.IgnoreCase);

				for (int i = 1; i < accessFileLines.Count; i++) {
					if (!Regex.IsMatch (accessFileLines [i], "^(debug|global)[=]", RegexOptions.IgnoreCase)) {
						this.AccessTypes.Add (accessFileLines [i]);
					}
				}
			}
		}

		/// <summary>
		/// Gets the permissions of this object as a list of file lines.
		/// </summary>
		/// <returns>The file lines.</returns>
		/// <param name="accessPasscode">Access passcode.</param>
		public List<string> GetFileLines (string accessPasscode)
		{
			var lines = new List<string> ();
			lines.Add (accessPasscode);
			lines.Add (String.Format ("debug={0}", this.DebugAccess.ToString ()));
			lines.Add (String.Format ("global={0}", this.GlobalAccess.ToString ()));
			lines.AddRange (AccessTypes);

			return lines;
		}

		public AppAccess ()
		{
			this.DebugAccess = false;
			this.GlobalAccess = false;
			this.AccessTypes = new List<string> ();
		}
	}
}

