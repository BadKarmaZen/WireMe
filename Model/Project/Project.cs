using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireMe.Model.Project
{
	public class Project
	{
		#region Members
		private List<Page> _pages;
		#endregion


		#region Properties
		public string Name { get; set; }
		public List<Page> Pages { get => _pages; set => _pages = value; }
		#endregion

		public Project()
		{
			_pages = new List<Page>();
		}

		#region Actions
		public void Clear()
		{
			_pages = new List<Page>();
		}

		public void Add(Page page)
		{
			Debug.Assert(page != null);

			_pages.Add(page);
		}

		public void RemoveItem(Page page)
		{
			Debug.Assert(page != null);

			_pages.Remove(page);
		}
		#endregion


		#region Helpers
		#endregion
	}
}
