﻿using System;
using System.IO;
using Autofac;
using LiteDB;
using PhotoHelper.IoC;
using PhotoHelper.Utility;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PhotoHelper
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = new MainPage();
		}

		public App(AppSetup setup)
		{
			AppContainer.Container = setup.CreateContainer();

			InitializeComponent();

			//Resources = new PHResourceDictionary();

			MainPage = new MainPage();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
			// NOTE: this is also closed on successful termination of app

			// Try to clear the WebView cache of any cookies
			AppContainer.Container.Resolve<IWebService>().ClearCache();
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}

		private static LiteDatabase database;
		public static LiteDatabase Database
		{
			get
			{
				if (database == null)
				{
					database = new LiteDatabase(Path.Combine(AppContainer.Container.Resolve<IFileService>().GetAppData(), @"local.db"));
				}
				return database;
			}
		}
	}
}
