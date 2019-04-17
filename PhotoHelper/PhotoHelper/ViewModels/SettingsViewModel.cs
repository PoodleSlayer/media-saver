using GalaSoft.MvvmLight.Messaging;
using PhotoHelper.Utility;
using PhotoHelper.Utility.Messages;
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
			Messenger.Default.Register<SettingsFolderMessage>(this, UpdateFolderLabel);

			FolderText = SaveLocation ?? "Please choose a folder...";
		}

		public void DidDisappear()
		{
			Messenger.Default.Unregister<SettingsFolderMessage>(this, UpdateFolderLabel);

			if (String.IsNullOrEmpty(Settings.SaveLocation))
			{
				SaveLocation = null;
			}
		}

		private void UpdateFolderLabel(SettingsFolderMessage msg)
		{
			FolderText = SaveLocation = msg.SelectedFolder;
		}
	}
}
