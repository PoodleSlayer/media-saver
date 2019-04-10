﻿using Autofac;
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
		private static LiteCollection<SettingsModel> settingsCollection { get; set; }

		/// <summary>
		/// Call this method once upon app startup to initialize the Settings class, loaded from LiteDB.
		/// </summary>
		public static void LoadSettings()
		{
			// would be cool to check that AutoFac has the IFileService registered before trying this
			settingsCollection = App.Database.GetCollection<SettingsModel>(SettingsModel.CollectionName);
			var settingsFile = settingsCollection.FindOne(x => x.Id == SettingsModel.IdName);
			if (settingsFile == null)
			{
				// initialize a new settings file? or just leave null? HMMM
				settings = new SettingsModel();
				SaveSettings();
			}
			else
			{
				settings = settingsFile;
				SaveLocation = settings.SaveLocation;  // this is the public property
				AppContainer.Container.Resolve<IFileService>().SaveLocation = settings.SaveLocation;
			}
		}

		/// <summary>
		/// Call this method to write the current values in the Settings class to the LiteDB settings file.
		/// </summary>
		public static void SaveSettings()
		{
			if (settingsCollection == null)
			{
				// probably called before calling LoadSettings, so just run that once
				LoadSettings();
			}

			// update values in the private SettingsModel before storing. for now just SaveLocation
			settings.SaveLocation = SaveLocation;
			settingsCollection.Upsert(SettingsModel.IdName, settings);
		}
    }
}