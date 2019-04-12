using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoHelper.IoC;

namespace PhotoHelper.iOS.IoC
{
	public class FileService : IFileService
	{
		public string SaveLocation { get; set; }

		public string GetAppData()
		{
			string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			return appDataPath;
		}

		public void ChooseLocation()
		{

		}

		public List<string> GetDirectories()
		{
			// call GetDirectories with the root directory for iOS
			// for now just return empty list
			return new List<string>();
		}

		public List<string> GetDirectories(string filepath)
		{
			// just return empty list for now for iOS
			return new List<string>();
		}

		public async Task<bool> DownloadFile(string downloadURL, string filenameToUse)
		{
			return true;
		}
	}
}