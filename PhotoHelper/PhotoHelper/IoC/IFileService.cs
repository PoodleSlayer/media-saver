using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PhotoHelper.IoC
{
    public interface IFileService
    {
		string SaveLocation { get; set; }
		string GetAppData();
		void ChooseLocation();
		List<string> GetDirectories();
		List<string> GetDirectories(string filepath);
		Task<bool> DownloadFile(string downloadURL, string filenameToUse);
    }
}
