using Autofac;
using PhotoHelper.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoHelper.IoC
{
	/// <summary>
	/// This class allows each platform to construct its own Autofac container
	/// for any necessary dependencies.
	/// </summary>
    public class AppSetup
    {
		/// <summary>
		/// This method initializes the Autofac container for the app by calling
		/// the necessary RegisterDependencies method and returning the built
		/// container.
		/// </summary>
		/// <returns></returns>
		public IContainer CreateContainer()
		{
			var containerBuilder = new ContainerBuilder();
			RegisterDependencies(containerBuilder);
			return containerBuilder.Build();
		}

		/// <summary>
		/// This method is used to add dependencies to the specified ContainerBuilder.
		/// Each platform will need to override this method and call 
		/// base.RegisterDependencies first.
		/// </summary>
		/// <param name="cb">The ContainerBuilder to add dependencies to</param>
		protected virtual void RegisterDependencies(ContainerBuilder cb)
		{
			cb.RegisterType<MainViewModel>().SingleInstance();
			cb.RegisterType<GalleryViewModel>().SingleInstance();
			cb.RegisterType<SettingsViewModel>().SingleInstance();
			cb.RegisterType<SaveViewModel>().SingleInstance();
		}
    }
}
