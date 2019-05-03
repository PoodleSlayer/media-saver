using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoHelper.IoC
{
    public interface IToastService
    {
		void MakeToast(string message);
    }
}
