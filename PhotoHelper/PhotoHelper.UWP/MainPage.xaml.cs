using System;
using PhotoHelper.UWP.IoC;

namespace PhotoHelper.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new PhotoHelper.App(new SetupUWP()));
        }
    }
}
