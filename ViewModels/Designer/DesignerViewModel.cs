using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using WireMe.Interfaces;
using WireMe.Behaviors;
using WireMe.Model.Project;

namespace WireMe.ViewModels.Designer
{
	public class ToolItem : IDragable
	{
		public string Id { get; set; }
		public Type DragableDataType { get { return typeof(ToolItem); } }
		public void Remove(object data) { }
	}

	public class CanvasItem : PropertyChangedBase
	{
		private double _x;
		private double _y;
		private double _width;
		private double _height;

		public double X
		{
			get { return _x; }
			set { _x = value; NotifyOfPropertyChange(() => X); }
		}

		public double Y
		{
			get { return _y; }
			set { _y = value; NotifyOfPropertyChange(() => Y); }
		}

		public double Width
		{
			get
			{
				return _width;
			}

			set
			{
				_width = value; NotifyOfPropertyChange(() => Width);
			}
		}

		public double Height
		{
			get { return _height; }
			set { _height = value; NotifyOfPropertyChange(() => Height); }
		}
	}

	public class DesignerViewModel : Screen, IDropable
	{
		#region Members
		private List<CanvasItem> _items = new List<CanvasItem>();

		private Page _currentPage;
		#endregion


		#region Properties
		public List<ToolItem> ToolItems
		{
			get
			{
				return new List<ToolItem> {
					new ToolItem { Id = "red" },
					new ToolItem { Id = "red"} };
			}
		}

		public List<Type> DropableDataTypes { get; }

		public List<CanvasItem> Items
		{
			get
			{
				var items = from i in _currentPage.Items
										select new CanvasItem
										{
											X = i.X,
											Y = i.Y,
											Width = i.Width,
											Height = i.Height
										};
				return items.ToList();
			}
		}

		public bool LoadProject { get; internal set; }
		#endregion

		public DesignerViewModel()
		{
			DropableDataTypes = new List<Type> { typeof(ToolItem), typeof(DragWrapper) };
			_currentPage = new Page();
		}

		#region Actions
		#endregion


		#region Helpers

		protected override void OnViewLoaded(object view)
		{
			if (LoadProject)
			{
				_currentPage = IoC.Get<IProjectManager>().CurrentProject.Pages.First();
			}

			NotifyOfPropertyChange((() => Items));
			base.OnViewLoaded(view);
		}

		public void Drop(object data, object target, Point point)
		{
			var item = new PageItem
			{
				Name = Guid.NewGuid().ToString(),
				X = point.X,
				Y = point.Y,
				Width = 50,
				Height = 22
			};

			_currentPage.Add(item);

			IoC.Get<IProjectManager>().CurrentProject.Add(_currentPage);

			NotifyOfPropertyChange((() => Items));
		}

		public bool CanDrop(object data, object target, out string message)
		{
			message = string.Empty;
			return true;
		}

		public void Open(object data, object target)
		{
		}

		public bool CanOpen(object data, object target)
		{
			return true;
		}

		#endregion
		
	}
}
