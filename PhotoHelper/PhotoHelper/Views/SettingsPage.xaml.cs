﻿using Autofac;
using GalaSoft.MvvmLight.Messaging;
using PhotoHelper.IoC;
using PhotoHelper.Models;
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
		private FileBrowserPage fileBrowserPage;
		private IFileService fileHelper;

		public SettingsPage ()
		{
			InitializeComponent ();
			BackBtn.Clicked += BackBtn_Clicked;
			SelectBtn.Clicked += SelectBtn_Clicked;
			SaveBtn.Clicked += SaveBtn_Clicked;
			CacheBtn.Clicked += CacheBtn_Clicked;

			BindingContext = AppContainer.Container.Resolve<SettingsViewModel>();

			fileHelper = AppContainer.Container.Resolve<IFileService>();
			fileBrowserPage = new FileBrowserPage();

			if (!String.IsNullOrEmpty(Settings.SaveLocation))
			{
				ViewModel.SaveLocation = Settings.SaveLocation;
			}
		}

		private void CacheBtn_Clicked(object sender, EventArgs e)
		{
			AppContainer.Container.Resolve<IWebService>().ClearCache();
		}

		private void SaveBtn_Clicked(object sender, EventArgs e)
		{
			// update the user's Save Location
			Settings.SaveLocation = fileHelper.SaveLocation;

			// update the user's Download Feedback
			if (ViewModel.NoneSwitch)
			{
				Settings.DownloadFeedback = Settings.DownloadNone;
			}
			else if (ViewModel.ToastSwitch)
			{
				Settings.DownloadFeedback = Settings.DownloadToast;
			}
			else
			{
				Settings.DownloadFeedback = Settings.DownloadNotif;
			}

			Settings.SaveSettings();

			Navigation.PopModalAsync();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			Messenger.Default.Register<NotificationMessage>(this, LoadFileBrowserPage);
			ViewModel.DidAppear();
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			Messenger.Default.Unregister<NotificationMessage>(this, LoadFileBrowserPage);
			ViewModel.DidDisappear();
		}

		private void LoadFileBrowserPage(NotificationMessage msg)
		{
			// this should ONLY happen on Android and iOS
			if (Device.RuntimePlatform != Device.iOS && Device.RuntimePlatform != Device.Android)
			{
				return;
			}

			if (msg.Notification == MessageHelper.LoadFileBrowserPage)
			{
				Navigation.PushModalAsync(fileBrowserPage);
			}
		}

		private void SelectBtn_Clicked(object sender, EventArgs e)
		{
			fileHelper.ChooseLocation();
		}

		private void BackBtn_Clicked(object sender, EventArgs e)
		{
			ViewModel.SaveLocation = Settings.SaveLocation;
			Navigation.PopModalAsync();
		}

		private SettingsViewModel ViewModel
		{
			get => BindingContext as SettingsViewModel;
		}
	}
}