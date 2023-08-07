using System;

namespace Dataelus.Extensions
{
	/// <summary>
	/// Extensions to the System.Text.StringBuilder class.
	/// </summary>
	public static class StringBuilderExtensions
	{
		/// <summary>
		/// Appends the formatted string as a line.
		/// </summary>
		/// <param name="builder">Builder.</param>
		/// <param name="format">Format.</param>
		/// <param name="args">Arguments.</param>
		public static void AppendLineFormat (this System.Text.StringBuilder builder, string format, params object[] args)
		{
			builder.AppendLine (String.Format (format, args));
		}

		/// <summary>
		/// Appends the formatted line, with the given indent (in space count).
		/// </summary>
		/// <param name="builder">Builder.</param>
		/// <param name="indent">Indent.</param>
		/// <param name="format">Format.</param>
		/// <param name="args">Arguments.</param>
		public static void AppendLineFormat (this System.Text.StringBuilder builder, int indent, string format, params object[] args)
		{
			builder.AppendLine (indent, String.Format (format, args));
		}

		/// <summary>
		/// Appends the line, with the given indent (in space count).
		/// </summary>
		/// <param name="builder">Builder.</param>
		/// <param name="indent">Indent.</param>
		/// <param name="value">Value.</param>
		public static void AppendLine (this System.Text.StringBuilder builder, int indent, string value)
		{
			builder.AppendLine (String.Format ("{0}{1}", new string (' ', indent), value));
		}
	}
}

