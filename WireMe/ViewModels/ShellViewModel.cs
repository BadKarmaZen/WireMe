using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using WireMe.Designer.ViewModels;
using WireMe.Interfaces;

namespace WireMe.ViewModels
{
	public class ShellViewModel : Conductor<Screen>
	{
		#region Members
		#endregion


		#region Properties
		#endregion


		#region Actions
		public void NewAction()
		{
			IoC.Get<IProjectManager>().NewProject();
			ActivateItem(new DesignerViewModel());
		}

		public void OpenAction()
		{
			IoC.Get<IProjectManager>().LoadProject();
			//ActivateItem(new DesignerViewModel() { LoadProject = true });
		}

		public void SaveAction()
		{
			IoC.Get<IProjectManager>().SaveProject();
		}

		public void SaveAsAction()
		{
			IoC.Get<IProjectManager>().SaveProjectAs();
		}

		public void CloseAction()
		{
			IoC.Get<IProjectManager>().SaveProject();

			this.TryClose();
		}
		#endregion


		#region Helpers

		protected override void OnViewLoaded(object view)
		{
			IoC.Get<IProjectManager>().NewProject();

			ActivateItem(new DesignerViewModel());

			base.OnViewLoaded(view);
		}

		#endregion


		
	}
}
