using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content.PM;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Widget;
using GalaSoft.MvvmLight.Messaging;
using PhotoHelper.IoC;
using PhotoHelper.Utility;

namespace PhotoHelper.Droid.IoC
{
	public class FileService : IFileService
	{
		public string SaveLocation { get; set; }

		public string GetAppData()
		{
			return System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
		}

		public void ChooseLocation()
		{
			// send a message to load the FileBrowserPage
			Messenger.Default.Send(new NotificationMessage(MessageHelper.LoadFileBrowserPage));
		}

		public List<string> GetDirectories()
		{
			// call GetDirectories with the root directory for Android
			return GetDirectories(Android.OS.Environment.ExternalStorageDirectory.Path);
		}

		public List<string> GetDirectories(string filepath)
		{
			List<string> directoryList = new List<string>();
			var dir = new DirectoryInfo(Android.OS.Environment.ExternalStorageDirectory.Path);
			try
			{
				foreach (var item in dir.GetFileSystemInfos())
				{
					directoryList.Add(item.FullName);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
			return directoryList;
		}

		public async Task<bool> DownloadFile(string downloadURL, string filenameToUse)
		{
			return true;
		}
	}
}