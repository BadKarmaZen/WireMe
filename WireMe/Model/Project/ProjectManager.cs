using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WireMe.Interfaces;

namespace WireMe.Model.Project
{
	public class ProjectManager : IProjectManager
	{
		#region Members
		private Project _fakeProject;
		#endregion


		#region Properties

		public Project CurrentProject { get; set; }
		#endregion


		#region Actions
		public void LoadProject()
		{
			CurrentProject = _fakeProject;
		}

		public void NewProject()
		{
			CurrentProject = new Project();
		}

		public void SaveProject()
		{
			//	Todo
			_fakeProject = CurrentProject;
		}

		public void SaveProjectAs()
		{
			//	Todo
		}

		#endregion


		#region Helpers
		#endregion
	}
}
