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
		
		public string Id = "settingsFile";

		public string SaveLocation { get; set; }
	}
}
