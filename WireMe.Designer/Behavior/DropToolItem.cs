using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interactivity;
using System.Windows.Media;
using WireMe.Common.Utility;
using WireMe.Designer.Interface;
using WireMe.Designer.Model;

namespace WireMe.Designer.Behavior
{
	public class DropToolItem : Behavior<FrameworkElement>
	{
		#region Members
		private DragDropEffects _effect;
		private UIElement _uiElement;
		private object _data;
		private object _target;
		private bool _canDrop;
		private Point? _last = null;
		#endregion

		#region Properties

		#endregion

		#region Event		
		protected override void OnAttached()
		{
			base.OnAttached();

			this.AssociatedObject.AllowDrop = true;
			this.AssociatedObject.DragEnter += new DragEventHandler(OnDragEnter);
			this.AssociatedObject.DragOver += new DragEventHandler(OnDragOver);
			this.AssociatedObject.DragLeave += new DragEventHandler(OnDragLeave);
			this.AssociatedObject.Drop += new DragEventHandler(OnDrop);
		}

		private void OnDragEnter(object sender, DragEventArgs e)
		{
			e.Effects = _effect = DragDropEffects.None;

			var isToolItem = e.Data.GetDataPresent(typeof (ToolItem));
			if (isToolItem)
			{
				//	 set drag effect
				e.Effects = DragDropEffects.Copy;
			}

			////	set the drop target
			//_target = this.AssociatedObject.DataContext;
			//_uiElement = sender as UIElement;
		}

		private void OnDragOver(object sender, DragEventArgs e)
		{
			//	get mouse position
			Point p = WindowsPosition.CorrectGetPosition(this.AssociatedObject);
			_last = p;

			//e.Effects = _effect;
			e.Handled = true;
		}

		private void OnDragLeave(object sender, DragEventArgs e)
		{
			e.Handled = true;

			//	 set drag effect
			e.Effects = DragDropEffects.None;
		}

		private void OnDrop(object sender, DragEventArgs e)
		{
			//	Drop object
			var drop = this.AssociatedObject.DataContext as IDropToolItem;

			var isToolItem = e.Data.GetDataPresent(typeof(ToolItem));
			var toolItem = e.Data.GetData(typeof (ToolItem)) as ToolItem;


			if (drop != null)
			{
				drop.DropToolItem(toolItem, _last);
			}

			e.Handled = true;
		}




		#endregion

		#region Actions
		#endregion

		#region Helpers
		#endregion
	}
}
