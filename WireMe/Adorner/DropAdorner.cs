using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WireMe.Adorner
{
	public class DropAdorner : System.Windows.Documents.Adorner
	{
		#region Consts

		private const int RADIUS = 4;
		private const int BORDER = 1;

		#endregion

		#region Members

		private object _data;
		private object _target;
		private bool _isOpening;
		private bool _canDrop;
		private string _message;

		#endregion

		public DropAdorner(UIElement element, object data, object target, bool isOpening, bool canDrop, string message) : base(element)
		{
			_data = data;
			_target = target;
			_isOpening = isOpening;
			_canDrop = canDrop;
			_message = message;
			this.IsHitTestVisible = false;
		}

		protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
		{
			//if (!(_target is IScreen))
			{
				if (_isOpening)
				{
					SolidColorBrush fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2200FF00"));
					drawingContext.DrawRectangle(fill, null, new Rect(0, 0, ActualWidth, ActualHeight));
				}
				else
				{
					SolidColorBrush fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#050000FF"));
					drawingContext.DrawRectangle(fill, null, new Rect(0, 0, ActualWidth, ActualHeight));
				}

				if (_canDrop)
				{
					SolidColorBrush border = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CCFFFFFF"));
					SolidColorBrush dot = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CC0000FF"));

					RenderOptions.SetEdgeMode(this, EdgeMode.Unspecified);
					drawingContext.DrawEllipse(border, null, new Point(0, 0), RADIUS + BORDER, RADIUS + BORDER);
					drawingContext.DrawEllipse(border, null, new Point(0, ActualHeight), RADIUS + BORDER, RADIUS + BORDER);
					drawingContext.DrawEllipse(border, null, new Point(ActualWidth, 0), RADIUS + BORDER, RADIUS + BORDER);
					drawingContext.DrawEllipse(border, null, new Point(ActualWidth, ActualHeight), RADIUS + BORDER, RADIUS + BORDER);

					drawingContext.DrawEllipse(dot, null, new Point(0, 0), RADIUS, RADIUS);
					drawingContext.DrawEllipse(dot, null, new Point(0, ActualHeight), RADIUS, RADIUS);
					drawingContext.DrawEllipse(dot, null, new Point(ActualWidth, 0), RADIUS, RADIUS);
					drawingContext.DrawEllipse(dot, null, new Point(ActualWidth, ActualHeight), RADIUS, RADIUS);
				}
			}
		}
	}
}
