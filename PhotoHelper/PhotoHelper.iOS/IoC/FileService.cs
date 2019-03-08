using System;
using System.Threading.Tasks;
using PhotoHelper.IoC;

namespace PhotoHelper.iOS.IoC
{
	public class FileService : IFileService
	{
		public string SaveLocation { get; set; }

		public void ChooseLocation()
		{

		}

		public async Task<bool> DownloadFile(string path)
		{
			return true;
		}
	}
}