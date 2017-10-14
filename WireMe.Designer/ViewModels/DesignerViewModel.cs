using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using WireMe.Designer.Interface;
using WireMe.Designer.Model;

namespace WireMe.Designer.ViewModels
{
	public class DesignerViewModel : Screen, IDropToolItem
	{
		#region Members

		private List<ToolItem> toolItems;
		private ObservableCollection<DesignerItem> _items = new ObservableCollection<DesignerItem>();

		#endregion


		#region Properties
		public List<ToolItem> ToolItems
		{
			get
			{
				return toolItems;
			}

			set
			{
				toolItems = value; NotifyOfPropertyChange(() => ToolItems);
			}
		}

		public ObservableCollection<DesignerItem> Items
		{
			get { return _items; }
			set { _items = value; NotifyOfPropertyChange(() => Items); }
		}

		#endregion


		#region Actions

		#endregion


		#region Helpers

		protected override void OnViewLoaded(object view)
		{
			ToolItems = new List<ToolItem> { new ToolItem() };
			base.OnViewLoaded(view);
		}
		public void DropToolItem(ToolItem item, Point? point)
		{
			if (point.HasValue)
			{
				Items.Add(new DesignerItem {
					Height = 20.0,
					Width = 50.0,
					X = point.Value.X - 25.0,
					Y = point.Value.Y - 10.0
				});
			}
		}

		#endregion

	}
}
