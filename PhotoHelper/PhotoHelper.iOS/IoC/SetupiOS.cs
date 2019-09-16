using System;
using Autofac;
using PhotoHelper.IoC;

namespace PhotoHelper.iOS.IoC
{
	public class SetupiOS : AppSetup
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