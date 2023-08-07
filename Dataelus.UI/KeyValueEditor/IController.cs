using System;
using System.Collections.Generic;

namespace Dataelus.UI.KeyValueEditor
{
	public interface IController : RecordEditor.IRecordEditorController
	{
		/// <summary>
		/// Gets or sets the dictionary of key/value pairs.
		/// </summary>
		/// <value>The values.</value>
		Dictionary<string, object> Values { get; set; }
	}
}

