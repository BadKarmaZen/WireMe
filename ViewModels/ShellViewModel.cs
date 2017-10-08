using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace WireMe.ViewModels
{
	public class ShellViewModel : Conductor<Screen>
	{
		protected override void OnViewLoaded(object view)
		{
			ActivateItem(new Designer.DesignerViewModel());
			base.OnViewLoaded(view);
		}
	}
}
