using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using WireMe.Interfaces;
using WireMe.Behaviors;

namespace WireMe.ViewModels.Designer
{
	public class ToolItem : IDragable
	{
		public string Id { get; set; }
		public Type DragableDataType { get { return typeof(ToolItem); } }
		public void Remove(object data) {}
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
		public List<ToolItem> ToolItems
		{
			get
			{
				return new List<ToolItem> {
					new ToolItem { Id = "red" },
					new ToolItem { Id = "red"} };
			}
		}

		private List<CanvasItem> _items = new List<CanvasItem>();


		public List<Type> DropableDataTypes { get; }

		public List<CanvasItem> Items
		{
			get { return new List<CanvasItem>(_items); }
		//	set { _items = value; }
		}


		public DesignerViewModel()
		{
			DropableDataTypes = new List<Type> { typeof(ToolItem), typeof(DragWrapper)};
		}


		public void Drop(object data, object target, Point point)
		{
			_items.Add(new CanvasItem()
			           {
				           X = point.X,
				           Y = point.Y,
									 Width = 50,
									 Height = 22
			           });

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
	}
}
