using System;

namespace Dataelus.EDD.Processor
{
	/// <summary>
	/// EDD file upload record - information about an EDD file.
	/// </summary>
	public class EDDFileUploadRecord
	{
		public string Filename { get; set; }

		public DateTime UploadDate { get; set; }

		public object EDDType { get; set; }
	}
}
