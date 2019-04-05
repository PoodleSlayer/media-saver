using GalaSoft.MvvmLight.Messaging;
using PhotoHelper.Utility;
using System;

namespace PhotoHelper.ViewModels
{
	/// <summary>
	/// This is the ViewModel for the user's Settings page. So far
	/// the only settings are specifying the save location and the
	/// default page to open on load, but that might actually move
	/// to the gallery page or somewhere.
	/// </summary>
    public class SettingsViewModel : PHViewModel
    {
		private string saveLocation;
		public string SaveLocation
		{
			get => saveLocation;
			set
			{
				saveLocation = value;
				RaisePropertyChanged("SaveLocation");
			}
		}

		public void DidAppear()
		{
			Messenger.Default.Register<NotificationMessage>(this, UpdateFolderLabel);

			SaveLocation = Settings.SaveLocation ?? "Please choose a folder...";
		}

		public void DidDisappear()
		{
			Messenger.Default.Unregister<NotificationMessage>(this, UpdateFolderLabel);
		}

		private void UpdateFolderLabel(NotificationMessage msg)
		{
			SaveLocation = msg.Notification;
		}
	}
}
