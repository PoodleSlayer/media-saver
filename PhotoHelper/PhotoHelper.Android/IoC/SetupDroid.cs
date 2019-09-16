using System;
using Autofac;
using PhotoHelper.IoC;

namespace PhotoHelper.Droid.IoC
{
	public class SetupDroid : AppSetup
	{
		protected override void RegisterDependencies(ContainerBuilder cb)
		{
			base.RegisterDependencies(cb);

			cb.RegisterType<FileService>().As<IFileService>().SingleInstance();
			cb.RegisterType<ToastService>().As<IToastService>().SingleInstance();
			cb.RegisterType<NotifierService>().As<INotifierService>().SingleInstance();
			cb.RegisterType<WebService>().As<IWebService>().SingleInstance();
		}
	}
}