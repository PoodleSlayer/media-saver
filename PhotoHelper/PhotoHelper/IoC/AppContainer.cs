using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoHelper.IoC
{
	/// <summary>
	/// This class acts as the global static Autofac container for the app.
	/// </summary>
    public static class AppContainer
    {
		/// <summary>
		/// The current Autofac container for the app
		/// </summary>
		public static IContainer Container { get; set; }
    }
}
