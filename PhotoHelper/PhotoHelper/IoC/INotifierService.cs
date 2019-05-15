using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoHelper.IoC
{
	public interface INotifierService
	{
		void Notify(string msg);
	}
}
