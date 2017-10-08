using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WireMe.Model.Project;

namespace WireMe.Interfaces
{
	public interface IProjectManager
	{
		Project CurrentProject { get; }

		void LoadProject();
		void NewProject();
		void SaveProject();
		void SaveProjectAs();
	}
}
