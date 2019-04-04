using Autofac;
using PhotoHelper.IoC;
using PhotoHelper.Models;
using System;
using System.Collections.Generic;
using System.Text;
using LiteDB;

namespace PhotoHelper.Utility
{
    public static class Settings
    {
		public static string SaveLocation { get; set; }

		private static SettingsModel settings { get; set; }

		public static void LoadSettings()
		{
			// would be cool to check that AutoFac has the IFileService registered before trying this
			var settingsCollection = App.Database.GetCollection<SettingsModel>(SettingsModel.CollectionName);
			var settingsFile = settingsCollection.FindOne(x => x.Id == "settingsFile");
			if (settingsFile == null)
			{
				// initialize a new settings file? or just leave null? HMMM
				settings = new SettingsModel();
				settingsCollection.Upsert("settingsFile", settings);
			}
		}

		public static void SaveSettings()
		{

		}
    }
}
