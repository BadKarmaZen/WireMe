using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using WireMe.Designer.Model;

namespace WireMe.Designer.Behavior
{
	public class DragToolItem : Behavior<FrameworkElement>
	{
		#region Members
		private FrameworkElement _el = null;
		private bool _isMouseClicked = false;
		private bool _isDragging = false;
		private Point? _dragPoint = null;
		#endregion

		#region Properties
		#endregion

		#region Event
		protected override void OnAttached()
		{
			base.OnAttached();
			this.AssociatedObject.MouseMove += OnMouseMove;
			this.AssociatedObject.PreviewMouseDown += OnPreviewMouseDown;
		}

		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton != MouseButtonState.Pressed)
			{
				_dragPoint = null;
			}

			if (_dragPoint.HasValue)
			{
				var effect = DragDrop.DoDragDrop((DependencyObject)sender, new ToolItem(), DragDropEffects.Copy);
				Debug.WriteLine($"_dragpoint:{_dragPoint} => {effect}");
				e.Handled = true;
			}
		}

		private void OnPreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			_dragPoint = e.GetPosition((IInputElement) sender);
		}

		#endregion

		#region Actions
		#endregion

		#region Helpers
		#endregion
	}
}
