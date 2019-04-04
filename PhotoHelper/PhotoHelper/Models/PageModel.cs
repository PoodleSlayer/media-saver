using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoHelper.Models
{
    public class PageModel
    {
		[BsonIgnore]
		public static readonly string CollectionName = "pages";

		public string PageName { get; set; }

		public string PageURL { get; set; }

		public string PageFileName { get; set; }

		public bool DefaultPage { get; set; }
    }
}
