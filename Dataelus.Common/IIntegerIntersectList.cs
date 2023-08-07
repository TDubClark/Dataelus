using System;

namespace Dataelus.Generic
{
	public interface IIntegerIntersectList<TRow, TColumn>
	{
		int GetInteger (TRow row, TColumn column);

		void SetInteger (TRow row, TColumn column, int count);
	}
}

