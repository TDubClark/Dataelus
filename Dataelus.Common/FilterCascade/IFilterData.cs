using System;

namespace Dataelus.FilterCascade
{
	/// <summary>
	/// Interface for filter data (Implementation is intended to be serialized).
	/// </summary>
	public interface IFilterData : Dataelus.IEquatable<IFilterData, string>
	{
		/// <summary>
		/// Gets or sets the filter code.
		/// </summary>
		/// <value>The filter code.</value>
		string FilterCode{ get; set; }

		/// <summary>
		/// Gets or sets the selected values.
		/// </summary>
		/// <value>The selected values.</value>
		System.Collections.Generic.List<string> SelectedValues{ get; set; }

		/// <summary>
		/// Gets or sets the mode.
		/// </summary>
		/// <value>The mode.</value>
		SelectionMode Mode{ get; set; }

		/// <summary>
		/// Gets or sets the selection count minimum.
		/// </summary>
		/// <value>The selection count minimum.</value>
		int SelectionCountMin{ get; set; }

		/// <summary>
		/// Gets or sets the selection count maximum.
		/// </summary>
		/// <value>The selection count max.</value>
		int SelectionCountMax{ get; set; }

		void CopyFrom (IFilterData other);
	}

	/// <summary>
	/// The mode of selection.
	/// </summary>
	public enum SelectionMode
	{
		/// <summary>
		/// A single item may be selected.
		/// </summary>
		Single,

		/// <summary>
		/// Multiple items may be selected.
		/// </summary>
		Multiple,

		/// <summary>
		/// The number of selectable items is bounded by a minimum and maximum selection count.
		/// </summary>
		Bounded,
	}
}

