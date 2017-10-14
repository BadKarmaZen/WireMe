using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using WireMe.ViewModels;
using WireMe.Model.Project;
using WireMe.Interfaces;

namespace WireMe.Core
{
	public class Bootstrapper : BootstrapperBase
	{
		#region Consts

		private readonly List<Assembly> _assemblies;

		#endregion

		#region Members

		private SimpleContainer _simpleContainer;

		#endregion

		public Bootstrapper()
		{
			_assemblies = base.SelectAssemblies().ToList();

			Initialize();
		}

		protected override void Configure()
		{
			_simpleContainer = new SimpleContainer();

			try
			{
				_simpleContainer.RegisterInstance(typeof(IEventAggregator), "IEventAggregator", new EventAggregator());
				_simpleContainer.RegisterInstance(typeof(IWindowManager), "IWindowManager", new WindowManager());
				_simpleContainer.RegisterSingleton(typeof(ShellViewModel), "ShellViewModel", typeof(ShellViewModel));


				_simpleContainer.RegisterInstance(typeof(IProjectManager), "ProjectManager", new ProjectManager());
			}
			catch (Exception)
			{

				throw;
			}

			//_simpleContainer.RegisterInstance(typeof(ThemeManager), "ThemeManager", new ThemeManager());

			//ILogonService logonService = new LogOnService();
			//_simpleContainer.RegisterInstance(typeof(ILogonService), "ILogonService", logonService);
			//_simpleContainer.RegisterInstance(typeof(ILicenseService), "ILicenseService", new LicenseService(logonService));

			//ISystemLicenses systemLicensesService = new SystemLicenseService();
			//Task.Run(() => systemLicensesService.RefreshData());
			//_simpleContainer.RegisterInstance(typeof(ISystemLicenses), "ISystemLicenses", systemLicensesService);
			//_simpleContainer.RegisterInstance(typeof(IVerifyLicense), "IVerifyLicense", new VerifyLicenseService());

			base.Configure();
		}



		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			DisplayRootViewFor<ShellViewModel>();
		}

		protected override IEnumerable<Assembly> SelectAssemblies()
		{
			return GetAssemblies();
		}

		protected override object GetInstance(Type service, string key)
		{
			return _simpleContainer.GetInstance(service, key);
		}

		protected override IEnumerable<object> GetAllInstances(Type service)
		{
			return _simpleContainer.GetAllInstances(service);
		}

		protected override void BuildUp(object instance)
		{
			_simpleContainer.BuildUp(instance);
		}

		#region Helpers
		private IEnumerable<Assembly> GetAssemblies()
		{
			var filenames = Directory.EnumerateFiles(Environment.CurrentDirectory, "WireMe.*.dll");

			foreach (var filename in filenames)
			{
				var assembly = Assembly.LoadFile(filename);

				_assemblies.Add(assembly);
			}
			return _assemblies;
		}

		//private void LoadModules()
		//{
		//	var filenames = Directory.EnumerateFiles(Environment.CurrentDirectory, "WireMe.*.dll");

		//	foreach (var filename in filenames)
		//	{
		//		var assembly = Assembly.LoadFile(filename);
		//		var modules = from t in assembly.GetTypes()
		//									where t.GetInterfaces().Contains(typeof(IModule))
		//									select t;

		//		foreach (var module in modules)
		//		{
		//			IModule foo = Activator.CreateInstance(module) as IModule;
		//			if (foo != null)
		//			{
		//				foo.Load();
		//			}
		//		}
		//	}
		//}

		#endregion
	}
}
