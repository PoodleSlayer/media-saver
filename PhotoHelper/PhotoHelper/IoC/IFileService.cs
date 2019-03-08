using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PhotoHelper.IoC
{
    public interface IFileService
    {
		string SaveLocation { get; set; }
		void ChooseLocation();
		Task<bool> DownloadFile(string downloadURL);
    }
}
