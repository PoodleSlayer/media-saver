using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoHelper.IoC
{
	/// <summary>
	/// This service is for assisting in platform-specific Web/WebView settings
	/// </summary>
	public interface IWebService
	{
		string UserAgent { get; set; }
		void ClearCache();
		string GetCookies();
	}
}
