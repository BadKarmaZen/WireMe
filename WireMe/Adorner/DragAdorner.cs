using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace WireMe.Adorner
{
	public class DragAdorner : System.Windows.Documents.Adorner
	{
		#region Member

		private ContentPresenter _contentPresenter;
		private AdornerLayer _adornerLayer;
		private double _leftOffset;
		private double _topOffset;

		#endregion

		#region Properties

		public double LeftOffset
		{
			get { return _leftOffset; }
		}

		public double TopOffset
		{
			get { return _topOffset; }
		}

		#endregion

		public DragAdorner(object data, DataTemplate dataTemplate, UIElement adornedElement, AdornerLayer adornerLayer)
			: base(adornedElement)
		{
			_adornerLayer = adornerLayer;

			_contentPresenter = new ContentPresenter()
			{
				Content = data,
				ContentTemplate = dataTemplate,
				Opacity = 0.75
			};

			if (_adornerLayer != null)
				_adornerLayer.Add(this);

			this.IsHitTestVisible = false;
		}

		protected override Size MeasureOverride(Size constraint)
		{
			_contentPresenter.Measure(constraint);
			return _contentPresenter.DesiredSize;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			_contentPresenter.Arrange(new Rect(finalSize));
			return finalSize;
		}

		protected override Visual GetVisualChild(int index)
		{
			return _contentPresenter;
		}

		protected override int VisualChildrenCount
		{
			get { return 1; }
		}

		public void UpdatePosition(double left, double top)
		{
			_leftOffset = left;
			_topOffset = top;
			if (_adornerLayer != null)
			{
				_adornerLayer.Update(this.AdornedElement);
			}
		}

		public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
		{
			GeneralTransformGroup result = new GeneralTransformGroup();
			result.Children.Add(base.GetDesiredTransform(transform));
			result.Children.Add(new TranslateTransform(_leftOffset, _topOffset));
			return result;
		}

		public void Destroy()
		{
			if (_adornerLayer != null)
				_adornerLayer.Remove(this);
		}
	}
}
