using System;

namespace WireMe.Interfaces
{
	public interface IDragable
	{
		/// <summary>
		/// Type of the data item
		/// </summary>
		Type DragableDataType { get; }

		/// <summary>
		/// Remove the object from the collection
		/// </summary>
		void Remove(object data);
	}
}