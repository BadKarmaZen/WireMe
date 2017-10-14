using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WireMe.Interfaces
{
	public interface IDropable
	{
		/// <summary>
		/// Type of the data item
		/// </summary>
		List<Type> DropableDataTypes { get; }

		/// <summary>
		/// Drop data into the collection.
		/// </summary>
		void Drop(object data, object target, Point point);

		bool CanDrop(object data, object target, out string message);

		void Open(object data, object target);

		bool CanOpen(object data, object target);
	}
}
