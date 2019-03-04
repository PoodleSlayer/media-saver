using System;

namespace PhotoHelper.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new PhotoHelper.App());
        }
    }
}
