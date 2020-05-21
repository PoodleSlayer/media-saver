using Autofac;
using GalaSoft.MvvmLight.Messaging;
using PhotoHelper.IoC;
using PhotoHelper.Models;
using PhotoHelper.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhotoHelper.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GalleryPage : ContentPage
	{
		public GalleryPage()
		{
			InitializeComponent ();
			BindingContext = AppContainer.Container.Resolve<GalleryViewModel>();
			PageListView.ItemSelected += PageListView_ItemSelected;

			BackBtn.Clicked += BackBtn_Clicked;
		}

		// should try to move this to the ViewModel...
		private void PageListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (((ListView)sender).SelectedItem == null)
			{
				return;
			}

			PageModel selectedPage = (PageModel)e.SelectedItem;

			// set the WebView on the MainPage to the URL and go back
			AppContainer.Container.Resolve<MainViewModel>().CurrentURL = selectedPage.PageURL;
			((ListView)sender).SelectedItem = null;
			Navigation.PopModalAsync();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			ViewModel.DidAppear();

			Messenger.Default.Register<PageModel>(this, PromptToRemoveItem);
		}

		private async void PromptToRemoveItem(PageModel item)
		{
			bool answer = await DisplayAlert("Remove Item?", "Do you want to remove \"" + item.PageName + "\"?", "Sure", "Wait no");
			if (answer)
			{
				// remove the item
				ViewModel.RemoveItemFromDb(item);
			}
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			ViewModel.DidDisappear();

			Messenger.Default.Unregister<PageModel>(this, PromptToRemoveItem);
		}

		private void BackBtn_Clicked(object sender, EventArgs e)
		{
			Navigation.PopModalAsync();
		}

		private GalleryViewModel ViewModel
		{
			get => BindingContext as GalleryViewModel;
		}
	}
}