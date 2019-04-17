﻿using Autofac;
using GalaSoft.MvvmLight.Messaging;
using PhotoHelper.IoC;
using PhotoHelper.Utility.Messages;
using PhotoHelper.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Web.Http;

namespace PhotoHelper.UWP.IoC
{
	public class FileService : IFileService
	{
		public string SaveLocation { get; set; }

		public string GetAppData()
		{
			return ApplicationData.Current.LocalCacheFolder.Path;
		}

		public async void ChooseLocation()
		{
			var folderPicker = new Windows.Storage.Pickers.FolderPicker();
			folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
			folderPicker.FileTypeFilter.Add("*");

			StorageFolder folder = await folderPicker.PickSingleFolderAsync();
			if (folder != null)
			{
				// I don't understand this but thanks stackoverflow
				Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);

				string destFolder = folder.Name;
				SaveLocation = folder.Path;

				// not ideal but nicer than hooking up some events across projects?
				Messenger.Default.Send(new SettingsFolderMessage(SaveLocation));
			}
		}

		public List<string> GetDirectories()
		{
			// UWP doesn't use this, just return a new empty list
			return new List<string>();
		}

		public List<string> GetDirectories(string filepath)
		{
			// UWP doesn't use this, just return a new empty list
			return new List<string>();
		}

		public async Task<bool> DownloadFile(string downloadURL, string filenameToUse)
		{
			if (String.IsNullOrEmpty(SaveLocation))
			{
				return false;
			}

			try
			{
				Uri source = new Uri(downloadURL);

				HttpClient client = new HttpClient();
				HttpResponseMessage result = await client.GetAsync(new Uri(downloadURL));

				StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(SaveLocation);

				// generate the first available filename
				int count = 1;
				while (await folder.TryGetItemAsync(filenameToUse + count + ".jpg") != null)
				{
					count++;
				}

				StorageFile destinationFile = await folder.CreateFileAsync(filenameToUse + count + ".jpg", CreationCollisionOption.GenerateUniqueName);

				using (var filestream = await destinationFile.OpenAsync(FileAccessMode.ReadWrite))
				{
					await result.Content.WriteToStreamAsync(filestream);
				}
			}
			catch (Exception ex)
			{
				string oops = ex.Message;
			}

			return true;
		}
	}
}
