using PhotoHelper.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoHelper.UWP.IoC
{
	public class WebService : IWebService
	{
		public void ClearCache()
		{
			// nothing! UWP seems fine?
		}
	}
}
