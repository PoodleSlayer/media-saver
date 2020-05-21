using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation;
using Photos;
using GalaSoft.MvvmLight.Messaging;
using PhotoHelper.IoC;
using PhotoHelper.Utility;
using System.Linq;
using System.Net;
using System.Diagnostics;
using UIKit;
using PhotoHelper.Models;
using System.IO;

namespace PhotoHelper.iOS.IoC
{
	public class FileService : IFileService
	{
		private string saveLocation;
		public string SaveLocation {
			get => saveLocation;
			set
			{
				if (saveLocation != value)
				{
					saveLocation = value;
					if (albumCollection == null)
					{
						PopulateAlbumCollection();
					}
					foreach (PHAssetCollection album in albumCollection)
					{
						if (album.LocalizedTitle == saveLocation)
						{
							selectedCollection = album;
						}
					}
				}
			}
		}
		private List<PHAssetCollection> albumCollection;
		private PHAssetCollection selectedCollection;

		public string GetAppData()
		{
			string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			return appDataPath;
		}
		
		public void ChooseLocation()
		{
			// send a message to load the FileBrowserPage
			Messenger.Default.Send(new NotificationMessage(MessageHelper.LoadFileBrowserPage));
		}

		/// <summary>
		/// Need to look up where all users actually CAN save photos on iOS... so far seems like the
		/// app directory itself (kinda useless) or the Photos library.
		/// </summary>
		/// <returns></returns>
		public List<string> GetDirectories()
		{
			// call GetDirectories with the root directory for iOS
			// for now just return empty list
			List<string> albumNames = new List<string>();
			//List<PHAssetCollection> albums = new List<PHAssetCollection>();
			//var allAlbums = PHAssetCollection.FetchAssetCollections(PHAssetCollectionType.Album, PHAssetCollectionSubtype.Any, null).Cast<PHAssetCollection>();
			//albums.AddRange(allAlbums);
			//var smartAlbums = PHAssetCollection.FetchAssetCollections(PHAssetCollectionType.SmartAlbum, PHAssetCollectionSubtype.Any, null).Cast<PHAssetCollection>();
			//albums.AddRange(smartAlbums);
			PopulateAlbumCollection();

			foreach (PHAssetCollection collecty in albumCollection)
			{
				string localid = collecty.LocalIdentifier; // might need this, not sure
				string title = collecty.LocalizedTitle; // this is the album name
				albumNames.Add(title);
			}

			return albumNames;
		}

		private void PopulateAlbumCollection()
		{
			List<PHAssetCollection> albums = new List<PHAssetCollection>();

			// this appears to be the same as just getting the allAlbums below...
			//List<PHCollection> topLevelAlbums = new List<PHCollection>();
			//var allTopLevelAlbums = PHCollection.FetchTopLevelUserCollections(null).Cast<PHCollection>();
			//topLevelAlbums.AddRange(allTopLevelAlbums);

			var allAlbums = PHAssetCollection.FetchAssetCollections(PHAssetCollectionType.Album, PHAssetCollectionSubtype.Any, null).Cast<PHAssetCollection>();
			albums.AddRange(allAlbums);
			var smartAlbums = PHAssetCollection.FetchAssetCollections(PHAssetCollectionType.SmartAlbum, PHAssetCollectionSubtype.Any, null).Cast<PHAssetCollection>();
			albums.AddRange(smartAlbums);

			albumCollection = albums;
		}

		public List<string> GetDirectories(string filepath)
		{
			// just return empty list for now for iOS
			return new List<string>();
		}

		public async Task<bool> DownloadFile(string downloadURL, string filenameToUse)
		{
			// download the image and get it into an NSData
			var webClient = new WebClient();
			webClient.DownloadDataCompleted += (s, e) =>
			{
				var bytes = e.Result; // gets downloaded data

				var library = PHPhotoLibrary.SharedPhotoLibrary;
				var collection = selectedCollection;

				library.PerformChanges(() =>
				{
					var options = new PHAssetResourceCreationOptions();
					options.OriginalFilename = "testimage.jpg";
					var createRequest = PHAssetCreationRequest.CreationRequestForAsset();
					createRequest.AddResource(PHAssetResourceType.Photo, NSData.FromArray(bytes), options);

					// save to specific album. need to figure out how to not duplicate image to Camera Roll
					var placeholder = createRequest.PlaceholderForCreatedAsset;
					var albumChangeRequest = PHAssetCollectionChangeRequest.ChangeRequest(collection);
					albumChangeRequest.AddAssets(new PHObject[] { placeholder });
				}, 
				(ok, error) =>
				{
					if (error != null)
					{
						Debug.WriteLine(error.LocalizedDescription);
					}
				});

				// adapt this to detect duplicate filenames in iOS Photo Gallery
				//int count = 1;
				//// need to figure out extension from the downloadURL for cases where the file is
				//// and mp4 or something other than jpg
				//string filepath = Path.Combine(SaveLocation, filenameToUse + count + ".jpg");

				//// figure out the first available filename
				//// this doesn't seem GREAT...
				//while (File.Exists(filepath))
				//{
				//	count++;
				//	filepath = Path.Combine(SaveLocation, filenameToUse + count + ".jpg");
				//}

				//File.WriteAllBytes(filepath, bytes);
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

		public async Task<bool> BackupList(List<PageModel> pages)
		{
			var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var backupFile = Path.Combine(documents, "backup.txt");

			using (var writer = File.CreateText(backupFile))
			{
				foreach (PageModel page in pages)
				{
					await writer.WriteLineAsync(page.PageName + ", " + page.PageURL);
				}
			}

			return true;
		}
	}
}