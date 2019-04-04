using Autofac;
using GalaSoft.MvvmLight.Messaging;
using PhotoHelper.IoC;
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
	public partial class SettingsPage : ContentPage
	{
		private IFileService fileHelper;

		public SettingsPage ()
		{
			InitializeComponent ();
			BackBtn.Clicked += BackBtn_Clicked;
			SelectBtn.Clicked += SelectBtn_Clicked;

			BindingContext = AppContainer.Container.Resolve<SettingsViewModel>();

			fileHelper = AppContainer.Container.Resolve<IFileService>();
			if (String.IsNullOrEmpty(fileHelper.SaveLocation))
			{
				DestFolderLbl.Text = "Please choose a folder...";
			}
			else
			{
				DestFolderLbl.Text = fileHelper.SaveLocation;
			}
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			// subscribe for messages
			Messenger.Default.Register<NotificationMessage>(this, UpdateFolderLabel);
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			// unsubscribe from messages
			Messenger.Default.Unregister<NotificationMessage>(this, UpdateFolderLabel);
		}

		private void UpdateFolderLabel(NotificationMessage msg)
		{
			DestFolderLbl.Text = fileHelper.SaveLocation;
		}

		private void SelectBtn_Clicked(object sender, EventArgs e)
		{
			fileHelper.ChooseLocation();
		}

		private void BackBtn_Clicked(object sender, EventArgs e)
		{
			Navigation.PopModalAsync();
		}

		private SettingsViewModel ViewModel
		{
			get => BindingContext as SettingsViewModel;
		}
	}
}