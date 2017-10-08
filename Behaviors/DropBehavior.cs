using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interactivity;
using System.Windows.Media;
using WireMe.Adorner;
using WireMe.Helpers;
using WireMe.Interfaces;

namespace WireMe.Behaviors
{
	public class DropBehavior : Behavior<FrameworkElement>
	{
		#region Members

		private System.Windows.Documents.Adorner _adorner = null;
		private AdornerLayer _adornerLayer = null;
		private DragDropEffects _effect;
		private bool _isOpening;

		private UIElement _uiElement;
		private object _data;
		private object _target;
		private bool _canDrop;
		private string _message;
		private Point _last;

		#endregion

		#region Timers

		// this timer trigger opening of the target
		System.Timers.Timer _openTimer = new System.Timers.Timer() { Enabled = false, Interval = 1000, AutoReset = false };

		// this one makes the adorner blink this fast...
		System.Timers.Timer _blinkTimer = new System.Timers.Timer() { Enabled = false, Interval = 50, AutoReset = true };

		// ...for this much time
		System.Timers.Timer _blinkStopTimer = new System.Timers.Timer() { Enabled = false, Interval = 500, AutoReset = false };

		#endregion

		#region Properties

		public static DependencyProperty AdornerTypeProperty = DependencyProperty.Register("AdornerType", typeof(Type), typeof(DragBehavior), new FrameworkPropertyMetadata(null));
		public Type AdornerType
		{
			get { return (Type)base.GetValue(AdornerTypeProperty); }
			set { base.SetValue(AdornerTypeProperty, value); }
		}

		#endregion

		#region Events

		protected override void OnAttached()
		{
			base.OnAttached();

			this.AssociatedObject.AllowDrop = true;
			this.AssociatedObject.DragEnter += new DragEventHandler(AssociatedObject_DragEnter);
			this.AssociatedObject.DragOver += new DragEventHandler(AssociatedObject_DragOver);
			this.AssociatedObject.DragLeave += new DragEventHandler(AssociatedObject_DragLeave);
			this.AssociatedObject.Drop += new DragEventHandler(AssociatedObject_Drop);
		}


		#endregion

		/// <summary>
		/// Gets the IDropable which will handle the dropping.
		/// </summary>
		private IDropable GetDropable()
		{
			IDropable dropable = null;
			// get dropable parent
			FrameworkElement el = this.AssociatedObject;
			while (el != null && !(el.DataContext is IDropable))
			{
				el = (FrameworkElement)VisualTreeHelper.GetParent(el);
			}

			if (el != null && el.DataContext is IDropable)
			{
				dropable = (IDropable)el.DataContext;
			}
			return dropable;
		}

		/// <summary>
		/// Gets the first acceptable Type for dropping.
		/// </summary>
		private Type GetDataType(DragEventArgs e)
		{
			IDropable dropable = GetDropable();
			if (dropable != null && dropable.DropableDataTypes != null)
			{
				if (e.Data.GetDataPresent(DataFormats.FileDrop) && dropable.DropableDataTypes.Contains(typeof(FileInfo)))
				{
					return typeof(FileInfo);
				}
				else
				{
					return dropable.DropableDataTypes.FirstOrDefault(t => e.Data.GetDataPresent(t));
				}
			}
			else
			{
				return null;
			}
		}

		private object GetData(DragEventArgs e)
		{
			if (GetDataType(e) == typeof(FileInfo))
			{
				return ((string[])e.Data.GetData(DataFormats.FileDrop)).Select(f => new FileInfo(f)).ToList();
			}
			else
			{
				//return e.Data.GetData(GetDataType(e));
				return e.Data.GetData(typeof(DragWrapper));
			}
		}

		public DropBehavior()
		{
			_blinkStopTimer.Elapsed += (s, e) =>
			{
				// do open
				Application.Current.Dispatcher.Invoke(() =>
				{
					IDropable dropable = GetDropable();
					if (dropable != null)
					{
						//if (dropable.CanOpen(_data, _target))
						dropable.Open(_data, _target);
					}
					_blinkTimer.Stop();
				});
			};

			_blinkTimer.Elapsed += (s, e) =>
			{
				// flash
				Application.Current.Dispatcher.Invoke(() =>
				{
					if (_uiElement != null)
					{
						_isOpening = !_isOpening;
						RemoveAdorner();
						MakeAdorner(_uiElement, _data, _target, _isOpening, _canDrop, _message);
					}
				});
			};

			_openTimer.Elapsed += (s, e) =>
			{
				// user waited for open
				Application.Current.Dispatcher.Invoke(() =>
				{
					IDropable dropable = GetDropable();
					if (dropable != null)
					{
						if (dropable.CanOpen(_data, _target))
						{
							_blinkTimer.Start();
							_blinkStopTimer.Start();
						}
					}
				});
			};
		}


		void AssociatedObject_DragEnter(object sender, DragEventArgs e)
		{
			e.Effects = _effect = DragDropEffects.None;
			Type dataType = GetDataType(e);
			if (dataType != null)
			{
				//Debug.WriteLine("Timer START");
				_openTimer.Start();

				// can drop?
				IDropable dropable = GetDropable();
				_canDrop = dropable.CanDrop(GetData(e), this.AssociatedObject.DataContext, out _message);
				_data = GetData(e);
				_target = this.AssociatedObject.DataContext;
				_uiElement = sender as UIElement;

				if (_adorner == null)
				{
					MakeAdorner(_uiElement, _data, _target, false, _canDrop, _message);
				}

				// give mouse effect
				if (_canDrop)
				{
					e.Effects = _effect = DragDropEffects.Move;
				}
			}
			e.Handled = true;
		}


		void AssociatedObject_DragOver(object sender, DragEventArgs e)
		{
			//Debug.WriteLine("+++++" + _dataType.ToString());
			Point p = WindowsPosition.CorrectGetPosition(this.AssociatedObject);
			if (_last.X != p.X || _last.Y != p.Y)
			{
				//Debug.WriteLine("Timer RESET " + DateTime.Now);
				_openTimer.Stop();
				_openTimer.Start();
				_last = p;
			}
			e.Effects = _effect;
			e.Handled = true;
		}

		void AssociatedObject_DragLeave(object sender, DragEventArgs e)
		{
			//Debug.WriteLine("Timer STOP");
			_openTimer.Stop();
			_blinkTimer.Stop();
			_blinkStopTimer.Stop();
			RemoveAdorner();
			e.Handled = true;
		}

		void AssociatedObject_Drop(object sender, DragEventArgs e)
		{
			_openTimer.Stop();
			_blinkTimer.Stop();
			_blinkStopTimer.Stop();

			// drop the data
			Type dataType = GetDataType(e);
			if (dataType != null)
			{
				DragWrapper wrapper = _data as DragWrapper;

				if (wrapper != null)
				{
					Debug.WriteLine($"Drop {wrapper.DragInfo.LeftOffset},{wrapper.DragInfo.TopOffset}");
					var pw = wrapper.DragInfo.PointToScreen(new Point(wrapper.DragInfo.LeftOffset, wrapper.DragInfo.TopOffset));

					Debug.WriteLine($"-> screen :  {pw}");
					var pc = _uiElement.PointToScreen(new Point(0,0));
					Debug.WriteLine($"-> canvas :  {pc}");

					GetDropable().Drop(wrapper.Item, _target, new Point(wrapper.DragInfo.LeftOffset-100, wrapper.DragInfo.TopOffset));
				}
				else
				{
					GetDropable().Drop(_data, _target, _last);
				}
			}

			// remove adorner
			RemoveAdorner();

			e.Handled = true;
			return;
		}

		void MakeAdorner(UIElement uiElement, object data, object target, bool isOpening, bool canDrop, string message)
		{
			// get adorner layer
			FrameworkElement el = this.AssociatedObject;
			while (el != null && (!(el is UserControl) || AdornerLayer.GetAdornerLayer(el) == null))
			{
				el = (FrameworkElement)VisualTreeHelper.GetParent(el);
			}

			if (el != null)
			{
				_adornerLayer = AdornerLayer.GetAdornerLayer(el);

				// make adorner
				if (AdornerType == null)
				{
					_adorner = new DropAdorner(uiElement, data, target, isOpening, canDrop, message);
				}
				else
				{
					_adorner = (System.Windows.Documents.Adorner)Activator.CreateInstance(AdornerType, new object[] { uiElement, data, target, isOpening, canDrop, message });
				}

				_adornerLayer.Add(this._adorner);

				// set adorner z-order
				var setZOrderMethodInfo = _adornerLayer.GetType().GetMethod("SetAdornerZOrder", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
				setZOrderMethodInfo.Invoke(_adornerLayer, new object[] { _adorner, 1 });
			}
		}
		void RemoveAdorner()
		{
			if (_adornerLayer != null && _adorner != null)
			{
				_adornerLayer.Remove(_adorner);
			}
			_adorner = null;
			_adornerLayer = null;
		}
	}
}
