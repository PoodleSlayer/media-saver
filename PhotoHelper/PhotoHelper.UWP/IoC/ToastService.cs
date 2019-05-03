using PhotoHelper.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoHelper.UWP.IoC
{
	public class ToastService : IToastService
	{
		public void MakeToast(string message)
		{
			// not sure how best to toast on UWP either...
		}
	}
}
