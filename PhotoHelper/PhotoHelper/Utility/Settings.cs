using Autofac;
using PhotoHelper.IoC;
using PhotoHelper.Models;
using System;
using System.Collections.Generic;
using System.Text;
using LiteDB;
using System.Linq;

namespace PhotoHelper.Utility
{
    public static class Settings
    {
		public static string SaveLocation { get; set; }
		public static string DownloadFeedback { get; set; }
		public static string DefaultPageURL { get; private set; }
		
		public static readonly string DownloadNone = "DownloadNone";
		public static readonly string DownloadToast = "DownloadToast";
		public static readonly string DownloadNotif = "DownloadNotif";

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
				DownloadFeedback = DownloadNone;
				SaveSettings();
			}
			else
			{
				settings = settingsFile;
				SaveLocation = settings.SaveLocation;  // this is the public property
				DownloadFeedback = settings.DownloadFeedback;

				AppContainer.Container.Resolve<IFileService>().SaveLocation = settings.SaveLocation;
			}

			// look up the default Page, if any
			var pageCollection = App.Database.GetCollection<PageModel>(PageModel.CollectionName);
			var defaultPages = pageCollection.Find(x => x.DefaultPage == true).ToList();
			if (defaultPages.Count > 0)
			{
				DefaultPageURL = defaultPages[0].PageURL;
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
			settings.DownloadFeedback = DownloadFeedback;
			settingsCollection.Upsert(SettingsModel.IdName, settings);
		}
    }
}
