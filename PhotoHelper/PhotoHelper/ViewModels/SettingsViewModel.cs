using GalaSoft.MvvmLight.Messaging;
using PhotoHelper.Models;
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

		private bool noneSwitch = true;
		public bool NoneSwitch
		{
			get => noneSwitch;
			set
			{
				noneSwitch = value;
				RaisePropertyChanged("NoneSwitch");
				if (noneSwitch)
				{
					ToastSwitch = false;
					NotifSwitch = false;
				}
			}
		}

		private bool toastSwitch;
		public bool ToastSwitch
		{
			get => toastSwitch;
			set
			{
				toastSwitch = value;
				RaisePropertyChanged("ToastSwitch");
				if (toastSwitch)
				{
					NoneSwitch = false;
					NotifSwitch = false;
				}
			}
		}

		private bool notifSwitch;
		public bool NotifSwitch
		{
			get => notifSwitch;
			set
			{
				notifSwitch = value;
				RaisePropertyChanged("NotifSwitch");
				if (notifSwitch)
				{
					NoneSwitch = false;
					ToastSwitch = false;
				}
			}
		}

		public void DidAppear()
		{
			Messenger.Default.Register<SettingsFolderMessage>(this, UpdateFolderLabel);

			FolderText = SaveLocation ?? "Please choose a folder...";
			if (Settings.DownloadFeedback == Settings.DownloadNone)
			{
				NoneSwitch = true;
			}
			else if (Settings.DownloadFeedback == Settings.DownloadToast)
			{
				ToastSwitch = true;
			}
			else
			{   // this better be DownloadNotif
				NotifSwitch = true;
			}
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
