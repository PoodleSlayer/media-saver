using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhotoHelper.Utility
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PHResourceDictionary : ResourceDictionary
	{
		public PHResourceDictionary ()
		{
			InitializeComponent ();
		}
	}
}