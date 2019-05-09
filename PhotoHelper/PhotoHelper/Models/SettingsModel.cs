using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoHelper.Models
{
	public class SettingsModel
	{
		[BsonIgnore]
		public static readonly string CollectionName = "settings";

		[BsonIgnore]
		public static readonly string IdName = "settingsFile";

		public string Id = "settingsFile";

		public string SaveLocation { get; set; }

		public string DownloadFeedback { get; set; }
	}
}
