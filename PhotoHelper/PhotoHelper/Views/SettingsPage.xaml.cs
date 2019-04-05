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
	public partial class SettingsPage : ContentPage
	{
		private IFileService fileHelper;

		public SettingsPage ()
		{
			InitializeComponent ();
			BackBtn.Clicked += BackBtn_Clicked;
			SelectBtn.Clicked += SelectBtn_Clicked;
			SaveBtn.Clicked += SaveBtn_Clicked;

			BindingContext = AppContainer.Container.Resolve<SettingsViewModel>();

			fileHelper = AppContainer.Container.Resolve<IFileService>();
		}

		private void SaveBtn_Clicked(object sender, EventArgs e)
		{
			Settings.SaveLocation = fileHelper.SaveLocation;
			Settings.SaveSettings();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			ViewModel.DidAppear();
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			ViewModel.DidDisappear();
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