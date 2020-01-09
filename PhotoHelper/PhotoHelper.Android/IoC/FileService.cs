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
using Android.Media;
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
			return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
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
				string fileType = downloadURL.Contains(@".mp4?") ? ".mp4" : ".jpg";
				string filepath = Path.Combine(SaveLocation, filenameToUse + count + fileType);

				// figure out the first available filename
				// this doesn't seem GREAT...
				while (File.Exists(filepath))
				{
					count++;
					filepath = Path.Combine(SaveLocation, filenameToUse + count + fileType);
				}

				File.WriteAllBytes(filepath, bytes);

				// manually tell Android Photos Gallery that a new file was downloaded
				MediaScannerConnection.ScanFile(Application.Context, new string[] { filepath }, null, null);
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