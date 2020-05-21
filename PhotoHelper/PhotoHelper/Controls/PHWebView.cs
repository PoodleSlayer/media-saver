﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PhotoHelper.Controls
{
	/// <summary>
	/// Subclassed WebView so I can make custom renderers if needed
	/// </summary>
	public class PHWebView : WebView
	{
		Action<string> action;

		public PHWebView ()
		{
			
		}

		public static readonly BindableProperty UriProperty = BindableProperty.Create(
			propertyName: "Uri",
			returnType: typeof(string),
			declaringType: typeof(PHWebView),
			defaultValue: default(string));

		public string Uri
		{
			get => (string)GetValue(UriProperty);
			set => SetValue(UriProperty, value);
		}

		public void RegisterAction(Action<string> callback)
		{
			action = callback;
		}

		public void Cleanup()
		{
			action = null;
		}

		public void InvokeAction(string data)
		{
			if (action == null || data == null)
			{
				return;
			}
			action.Invoke(data);
		}
	}
}