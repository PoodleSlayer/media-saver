using Autofac;
using PhotoHelper.IoC;
using PhotoHelper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoHelper.ViewModels
{
    public class SaveViewModel : PHViewModel
    {
		private PageModel newPage;

		public void DidAppear()
		{
			PageURL = AppContainer.Container.Resolve<MainViewModel>().CurrentURL;
		}

		public string PageName
		{
			get; set;
		}

		public string PageURL
		{
			get; set;
		}

		public string PageFileName
		{
			get; set;
		}

		public bool DefaultPage
		{
			get; set;
		}

    }
}
