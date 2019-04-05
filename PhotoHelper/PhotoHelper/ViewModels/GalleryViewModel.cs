using PhotoHelper.Models;
using System;
using System.Collections.ObjectModel;

namespace PhotoHelper.ViewModels
{
	/// <summary>
	/// This is the ViewModel for the Gallery page which will hold a list
	/// of pages the user has saved previously. The list will be a list
	/// of some model class that has info about the page...
	/// </summary>
    public class GalleryViewModel : PHViewModel
    {
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
