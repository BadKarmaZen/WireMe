using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using WireMe.ViewModels;

namespace WireMe.Core
{
	public class Bootstrapper : BootstrapperBase
	{
		#region Members

		private SimpleContainer _simpleContainer;
		#endregion

		public Bootstrapper()
		{
			Initialize();
		}

		protected override void Configure()
		{
			_simpleContainer = new SimpleContainer();

			_simpleContainer.RegisterInstance(typeof(IEventAggregator), "IEventAggregator", new EventAggregator());
			_simpleContainer.RegisterInstance(typeof(IWindowManager), "IWindowManager", new WindowManager());
			_simpleContainer.RegisterSingleton(typeof(ShellViewModel), "ShellViewModel", typeof(ShellViewModel));

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
	}
}
