using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
			var dir = new DirectoryInfo(filepath);
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
			var webClient = new WebClient();
			webClient.DownloadDataCompleted += (s, e) =>
			{
				var bytes = e.Result; // gets downloaded data

				int count = 1;
				// need to figure out extension from the downloadURL for cases where the file is
				// and mp4 or something other than jpg
				string filepath = Path.Combine(SaveLocation, filenameToUse + count + ".jpg");

				// figure out the first available filename
				// this doesn't seem GREAT...
				while (File.Exists(filepath))
				{
					count++;
					filepath = Path.Combine(SaveLocation, filenameToUse + count + ".jpg");
				}

				File.WriteAllBytes(filepath, bytes);
			};
			var url = new Uri(downloadURL);

			try
			{
				Task tasky = webClient.DownloadDataTaskAsync(url);
				await tasky;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}

			return true;
		}
	}
}