using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using WireMe.Adorner;
using WireMe.Interfaces;

namespace WireMe.Behaviors
{
	public class DragBehavior : Behavior<FrameworkElement>
	{
		#region Members

		private bool _isMouseClicked = false;
		private bool _isDragging = false;
		private Point _original;
		private DragAdorner _itemAdorner;
		private FrameworkElement _el = null;

		#endregion

		#region Properties

		public static DependencyProperty DataTemplateProperty = DependencyProperty.Register("DataTemplate", typeof(DataTemplate), typeof(DragBehavior), new FrameworkPropertyMetadata(null));

		public DataTemplate DataTemplate
		{
			get { return (DataTemplate)base.GetValue(DataTemplateProperty); }
			set { base.SetValue(DataTemplateProperty, value); }
		}

		#endregion

		#region Events

		protected override void OnAttached()
		{
			base.OnAttached();
			this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
			this.AssociatedObject.GiveFeedback += AssociatedObject_GiveFeedback;
			this.AssociatedObject.MouseDown += AssociatedObject_MouseDown;
			this.AssociatedObject.MouseUp += AssociatedObject_MouseUp;
		}

		private void AssociatedObject_MouseUp(object sender, MouseButtonEventArgs e)
		{
			Mouse.Capture(null);
			_isMouseClicked = false;
			_isDragging = false;
			DestroyAdorner();
		}

		private void AssociatedObject_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Debug.Write("mouse is clicked => captured");
			//Mouse.Capture(this.AssociatedObject);
			_isMouseClicked = true;
			_original = e.GetPosition(this.AssociatedObject);
		}

		private void AssociatedObject_GiveFeedback(object sender, GiveFeedbackEventArgs e)
		{
			//try
			{
				// get dragable parent
				//FrameworkElement el = this.AssociatedObject;
				//while (!(el.DataContext is IDragable))
				//    el = (FrameworkElement)VisualTreeHelper.GetParent(el);

				Point p = CorrectGetPosition(_el);
				//Debug.WriteLine("dragging " + p.X + "," + p.Y);
				if (_isDragging && p.X != _itemAdorner.LeftOffset && p.Y != _itemAdorner.TopOffset)
				{
					UpdateDragAdorner(new Point(p.X - _original.X, p.Y - _original.Y));
				}
			}
			//catch { }
		}

		private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
		{
			if (_isMouseClicked)
			{
				Point p = e.GetPosition(this.AssociatedObject);
				Debug.WriteLine("moving " + p.X + "," + p.Y);
				if (_isDragging)
				{
					// update adorner
					UpdateDragAdorner(p);
				}
				else
				{
					if (Math.Abs(p.X - _original.X) > 10 || Math.Abs(p.Y - _original.Y) > 10)
					{
						_isDragging = true;

						FrameworkElement el = this.AssociatedObject;
						FrameworkElement dragEl = null;
						while ((FrameworkElement)VisualTreeHelper.GetParent(el) != null)
						{
							el = (FrameworkElement)VisualTreeHelper.GetParent(el);
							if (AdornerLayer.GetAdornerLayer(el) != null)
							{
								_el = el;
							}
							if (el.DataContext is IDragable)
							{
								dragEl = el;
							}
						}

						// get dragable parent
						//while (!(el.DataContext is IDragable))
						//    el = (FrameworkElement)VisualTreeHelper.GetParent(el);

						IDragable dragObject = dragEl.DataContext as IDragable;
						if (dragObject != null)
						{
							Mouse.Capture(null);

							// init adorner
							Point pp = e.GetPosition(_el);
							Point newpoint = new Point(pp.X - _original.X, pp.Y - _original.Y);
							InitializeDragAdorner(this.AssociatedObject.DataContext, newpoint );

							// start dragging
							DataObject data = new DataObject();
							data.SetData(dragObject.DragableDataType, this.AssociatedObject.DataContext);
							//DragDropEffects effect = System.Windows.DragDrop.DoDragDrop(this.AssociatedObject, data, DragDropEffects.Move);
							DragDropEffects effect = System.Windows.DragDrop.DoDragDrop(this.AssociatedObject,
								new DragWrapper
								{
									Item = data,
									DragInfo = _itemAdorner
								}, 
								DragDropEffects.Move);

							// done dragging
							Mouse.Capture(null);
							_isMouseClicked = false;
							_isDragging = false;
							DestroyAdorner();
							if (effect == DragDropEffects.Move)
							{
								dragObject.Remove(this.AssociatedObject.DataContext);
							}
						}
					}
				}
			}
		}

		#endregion

		#region Helpers

		void InitializeDragAdorner(object dragData, Point startPosition)
		{
			if (this.DataTemplate != null)
			{
				if (_itemAdorner == null)
				{
					//FrameworkElement el = this.AssociatedObject;
					//while (!(el is UserControl))
					//    el = (FrameworkElement)VisualTreeHelper.GetParent(el);

					var adornerLayer = AdornerLayer.GetAdornerLayer(_el);
					_itemAdorner = new DragAdorner(dragData, DataTemplate, _el, adornerLayer);
					_itemAdorner.UpdatePosition(startPosition.X, startPosition.Y);
					// set adorner z-order
					var setZOrderMethodInfo = adornerLayer.GetType().GetMethod("SetAdornerZOrder", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
					setZOrderMethodInfo.Invoke(adornerLayer, new object[] { _itemAdorner, 1 });
				}
			}
		}

		void UpdateDragAdorner(Point currentPosition)
		{
			if (_itemAdorner != null)
			{
				Debug.WriteLine("UpdateDragAdorner " + currentPosition.X + "," + currentPosition.Y);
				_itemAdorner.UpdatePosition(currentPosition.X, currentPosition.Y);
			}
		}

		void DestroyAdorner()
		{
			if (_itemAdorner != null)
			{
				_itemAdorner.Destroy();
				_itemAdorner = null;
			}
		}

		#endregion

		#region Win32 Helpers

		[StructLayout(LayoutKind.Sequential)]
		internal struct Win32Point
		{
			public Int32 X;
			public Int32 Y;
		};

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetCursorPos(ref Win32Point pt);

		public static Point CorrectGetPosition(Visual relativeTo)
		{
			Win32Point w32Mouse = new Win32Point();
			GetCursorPos(ref w32Mouse);
			return relativeTo.PointFromScreen(new Point(w32Mouse.X, w32Mouse.Y));
		}

		#endregion




	}
}
