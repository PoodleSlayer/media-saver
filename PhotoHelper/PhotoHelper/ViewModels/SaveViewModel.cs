using Autofac;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using PhotoHelper.IoC;
using PhotoHelper.Models;
using PhotoHelper.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoHelper.ViewModels
{
    public class SaveViewModel : PHViewModel
    {
		private PageModel newPage;
		public RelayCommand SaveCommand { get; set; }
		public event EventHandler PageSaved;

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

			if (String.IsNullOrEmpty(PageName))
			{
				// need to set this so the Gallery view has labels, also it just makes
				// sense for the user to at least set this. If they don't specify a 
				// filename then the app just grabs the page's URL
				Messenger.Default.Send(new NotificationMessage(MessageHelper.PromptForPageName));
				return;
			}

			var pageCollection = App.Database.GetCollection<PageModel>(PageModel.CollectionName);
			if (DefaultPage == true)
			{
				// only allow one DefaultPage so clear any that are currently set in the db
				var defaultPages = pageCollection.Find(x => x.DefaultPage == true);
				foreach (PageModel dPage in defaultPages)
				{
					dPage.DefaultPage = false;
					// ideally i would bulk upsert but in theory there should only ever be one
					// of these so this is okay for now
					pageCollection.Upsert(dPage.PageURL, dPage);
				}
			}
			pageCollection.Upsert(PageURL, newPage);

			// fire event to let the page know this is done
			PageSaved?.Invoke(this, null);
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
