using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoHelper.Utility.Messages
{
    public class SettingsFolderMessage
    {
		public string SelectedFolder { get; set; }

		public SettingsFolderMessage(string folderName)
		{
			SelectedFolder = folderName;
		}
    }
}
