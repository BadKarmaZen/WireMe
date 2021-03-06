﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using WireMe.Interfaces;

namespace WireMe.ViewModels.Designer
{
	public class CanvasItem : PropertyChangedBase, IDragable
	{
		#region Members
		private double _x;
		private double _y;
		private double _width;
		private double _height;
		private double _selected;
		#endregion


		#region Properties
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
			get { return _width; }
			set { _width = value; NotifyOfPropertyChange(() => Width); }
		}

		public double Height
		{
			get { return _height; }
			set { _height = value; NotifyOfPropertyChange(() => Height); }
		}

		public double Selected
		{
			get { return _selected; }
			set { _selected = value; NotifyOfPropertyChange(() => Selected); }
		}

		#endregion


		#region Actions
		#endregion


		#region Helpers
		#endregion

		public Type DragableDataType { get { return typeof (CanvasItem); } }

		public void Remove(object data)
		{
		}
	}
}
