using Autofac;
using GalaSoft.MvvmLight.Messaging;
using PhotoHelper.IoC;
using PhotoHelper.Utility;
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
	public partial class SavePage : ContentPage
	{
		public SavePage ()
		{
			InitializeComponent ();
			BindingContext = AppContainer.Container.Resolve<SaveViewModel>();

			BackBtn.Clicked += BackBtn_Clicked;
			ViewModel.PageSaved += ViewModel_PageSaved;
		}

		private void ViewModel_PageSaved(object sender, EventArgs e)
		{
			Navigation.PopModalAsync();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			ViewModel.DidAppear();

			Messenger.Default.Register<NotificationMessage>(this, PromptForPageName);
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			ViewModel.DidDisappear();

			Messenger.Default.Unregister<NotificationMessage>(this, PromptForPageName);
		}

		private async void PromptForPageName(NotificationMessage msg)
		{
			if (msg.Notification == MessageHelper.PromptForPageName)
			{
				// user needs to enter a Page Name at least
				await DisplayAlert("No Page Name", "Please enter a Page Name to save this page (it will help you find it later!)", "Thanks dude");
			}
		}

		// move all of these to a command in the base view model since basically all pages will have a back button
		private void BackBtn_Clicked(object sender, EventArgs e)
		{
			Navigation.PopModalAsync();
		}

		private SaveViewModel ViewModel
		{
			get => BindingContext as SaveViewModel;
		}
	}
}