using Autofac;
using GalaSoft.MvvmLight.Command;
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
		public RelayCommand SaveCommand { get; set; }

		public SaveViewModel()
		{
			SetupCommands();
		}

		private void SetupCommands()
		{
			SaveCommand = new RelayCommand(() =>
			{
				SavePage();
			});
		}

		public void DidAppear()
		{
			PageURL = AppContainer.Container.Resolve<MainViewModel>().CurrentURL;

			var pageCollection = App.Database.GetCollection<PageModel>(PageModel.CollectionName);
			var pageItem = pageCollection.FindOne(x => x.PageURL == PageURL);
			if (pageItem == null)
			{
				newPage = new PageModel()
				{
					PageURL = PageURL,
					PageName = "",
					PageFileName = "",
					DefaultPage = false,
				};
			}
			else
			{
				newPage = pageItem;
			}

			PageName = newPage.PageName;
			PageFileName = newPage.PageFileName;
			DefaultPage = newPage.DefaultPage;
			RaisePropertyChanged(""); // updates all bindings

			// maybe try loading from database if user already has this page saved? so they
			// can possibly edit their settings for it
		}

		public void DidDisappear()
		{
			// in case we need this
		}

		private void SavePage()
		{
			// save the new page details, or update if it exists
			newPage = new PageModel
			{
				PageURL = PageURL,
				PageName = PageName,
				PageFileName = PageFileName,
				DefaultPage = DefaultPage
			};

			var pageCollection = App.Database.GetCollection<PageModel>(PageModel.CollectionName);
			pageCollection.Upsert(PageURL, newPage);
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
