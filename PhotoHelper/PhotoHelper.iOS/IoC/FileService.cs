using System;
using System.Threading.Tasks;
using PhotoHelper.IoC;

namespace PhotoHelper.iOS.IoC
{
	public class FileService : IFileService
	{
		public string SaveLocation { get; set; }

		public string GetAppData()
		{
			return "";
		}

		public void ChooseLocation()
		{

		}

		public async Task<bool> DownloadFile(string downloadURL, string filenameToUse)
		{
			return true;
		}
	}
}