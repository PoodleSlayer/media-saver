using PhotoHelper.IoC;
using System;
using System.Collections.Generic;
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

		public async void ChooseLocation()
		{
			var folderPicker = new Windows.Storage.Pickers.FolderPicker();
			folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
			folderPicker.FileTypeFilter.Add("*");

			StorageFolder folder = await folderPicker.PickSingleFolderAsync();
			if (folder != null)
			{
				string destFolder = folder.Name;
				SaveLocation = folder.Path;
			}
		}

		public async Task<bool> DownloadFile(string downloadURL)
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
				StorageFile destinationFile = await folder.CreateFileAsync("test123.jpg", CreationCollisionOption.GenerateUniqueName);

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
