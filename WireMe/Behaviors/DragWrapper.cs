using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WireMe.Adorner;

namespace WireMe.Behaviors
{
	public class DragWrapper
	{
		public object Item { get; set; }
		public DragAdorner DragInfo { get; set; }
	}
}
