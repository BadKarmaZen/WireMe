using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireMe.Model.Project
{
	public class Page
	{
		#region Members
		private List<PageItem> _items;
		#endregion


		#region Properties
		public string Name { get; set; }
		public List<PageItem> Items { get => _items; set => _items = value; }
		#endregion

		public Page()
		{
			_items = new List<PageItem>();
		}

		#region Actions
		public void Clear()
		{
			_items = new List<PageItem>();
		}

		public void Add(PageItem item)
		{
			Debug.Assert(item != null);

			_items.Add(item);
		}

		public void RemoveItem(PageItem item)
		{
			Debug.Assert(item != null);

			_items.Remove(item);
		}
		#endregion


		#region Helpers
		#endregion
	}
}
