using System;

namespace Dataelus.Mono
{
	/// <summary>
	/// Application Load/Environment Seed object.
	/// </summary>
	[Serializable]
	public class AppLESeed
	{
		public string LogDirectory { get; set; }

		public string AppSettingsPath { get; set; }

		public AppLESeed ()
		{
		}
	}

	public class GtkSettings
	{
		public string GtkDefaultTheme { get; set; }

		public string GtkRcParse { get; set; }
	}
}

