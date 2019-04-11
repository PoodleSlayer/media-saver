using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using PhotoHelper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PhotoHelper.ViewModels
{
	/// <summary>
	/// This is the ViewModel for the Gallery page which will hold a list
	/// of pages the user has saved previously. The list will be a list
	/// of some model class that has info about the page...
	/// </summary>
    public class GalleryViewModel : PHViewModel
    {
		public RelayCommand<PageModel> RemoveItemCommand { get; set; }


		public GalleryViewModel()
		{
			SetupCommands();
		}

		private void SetupCommands()
		{
			RemoveItemCommand = new RelayCommand<PageModel>((item) =>
			{
				RemoveItem(item);
			});
		}

		private void RemoveItem(PageModel item)
		{
			Messenger.Default.Send(item);
		}

		private ObservableCollection<PageModel> pageList;
		public ObservableCollection<PageModel> PageList
		{
			get => pageList;
			set
			{
				pageList = value;
				RaisePropertyChanged("PageList");
			}
		}

		public void RemoveItemFromDb(PageModel item)
		{
			// delete from db
			var pageCollection = App.Database.GetCollection<PageModel>(PageModel.CollectionName);
			pageCollection.Delete(item.PageURL);

			// update the items source
			PageList.Clear();
			List<PageModel> pages = pageCollection.FindAll().ToList();
			foreach (PageModel page in pages)
			{
				PageList.Add(page);
			}
		}

		public void DidAppear()
		{
			var pageCollection = App.Database.GetCollection<PageModel>(PageModel.CollectionName);
			PageList = new ObservableCollection<PageModel>(pageCollection.FindAll());
		}

		public void DidDisappear()
		{

		}
    }
}
