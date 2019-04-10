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

		private string folderText;
		public string FolderText
		{
			get => folderText;
			set
			{
				folderText = value;
				RaisePropertyChanged("FolderText");
			}
		}

		public void DidAppear()
		{
			Messenger.Default.Register<NotificationMessage>(this, UpdateFolderLabel);

			FolderText = SaveLocation ?? "Please choose a folder...";
		}

		public void DidDisappear()
		{
			Messenger.Default.Unregister<NotificationMessage>(this, UpdateFolderLabel);

			if (String.IsNullOrEmpty(Settings.SaveLocation))
			{
				SaveLocation = null;
			}
		}

		private void UpdateFolderLabel(NotificationMessage msg)
		{
			FolderText = SaveLocation = msg.Notification;
		}
	}
}
