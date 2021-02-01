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
using Autofac;
using GalaSoft.MvvmLight.Messaging;
using PhotoHelper.IoC;
using PhotoHelper.Models;
using PhotoHelper.Utility;

namespace PhotoHelper.Droid.IoC
{
	public class FileService : IFileService
	{
		private WebClient ttWebClient;
		private IWebService webService;
		private string ttCookieString;

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

		public async Task<bool> DownloadTTFile(string downloadURL, string filenameToUse)
		{
			if (ttWebClient == null)
			{
				ttWebClient = new WebClient();
			}

			if (webService == null)
			{
				webService = AppContainer.Container.Resolve<IWebService>();
			}

			if (String.IsNullOrEmpty(ttCookieString))
			{

				string cookies = webService.GetCookies();
				List<string> cookieList = cookies.Split(" ").ToList();

				string webidv2 = "", webid = "", csrf_token = "";
				foreach (string cookie in cookieList)
				{
					if (cookie.StartsWith("tt_webid_v2="))
					{
						webidv2 = cookie;
					}
					if (cookie.StartsWith("tt_webid="))
					{
						webid = cookie;
					}
					if (cookie.StartsWith("tt_csrf_token="))
					{
						csrf_token = cookie;
					}
				}
				ttCookieString = webidv2 + " " + webid + " " + csrf_token;
			}

			ttWebClient.Headers.Clear();
			ttWebClient.Headers.Add("Host", "v16-web.tiktok.com");
			//webClient.Headers.Add("Connection", "keep-alive");  // lol this broke some stuff
			ttWebClient.Headers.Add("Pragma", "no-cache");
			ttWebClient.Headers.Add("Cache-Control", "no-cache");
			ttWebClient.Headers.Add("User-Agent", webService.UserAgent);
			ttWebClient.Headers.Add("Accept-Encoding", "identity;q=1, *;q=0");
			ttWebClient.Headers.Add("Accept", "*/*");
			ttWebClient.Headers.Add("Sec-Fetch-Site", "same-site");
			ttWebClient.Headers.Add("Sec-Fetch-Mode", "no-cors");
			ttWebClient.Headers.Add("Sec-Fetch-Dest", "video");
			ttWebClient.Headers.Add("Referer", @"https://www.tiktok.com/");
			ttWebClient.Headers.Add("Accept-Language", "en-US,en;q=0.9");
			ttWebClient.Headers.Add("Cookie", ttCookieString);

			;

			ttWebClient.DownloadDataCompleted += (s, e) =>
			{
				var bytes = e.Result; // gets downloaded data

				int count = 1;
				string fileType = ".mp4";  // for now we'll just assume all TikTok videos are mp4s thumbsUpCat.jpg
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
				Task tasky = ttWebClient.DownloadDataTaskAsync(url);
				await tasky;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}

			return true;
		}

		public async Task<bool> BackupList(List<PageModel> pages)
		{
			string backupPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);
			string backupFile = Path.Combine(backupPath, "backup.txt");
			;
			using (var writer = File.CreateText(backupFile))
			{
				foreach (PageModel page in pages)
				{
					await writer.WriteLineAsync(page.PageName + ", " + page.PageURL);
				}
			}

			// this is SO ANNOYING, ANDROID
			MediaScannerConnection.ScanFile(Application.Context, new string[] { backupFile }, null, null);

			return true;
		}
	}
}