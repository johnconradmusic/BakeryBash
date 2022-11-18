using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace BakeryBash.XBox
{
	public sealed partial class GamePage : Page
	{
		readonly BakeryBash _game;

		public GamePage()
		{
			this.InitializeComponent();
			try
			{
				// Create the game.
				var launchArguments = string.Empty;
				_game = MonoGame.Framework.XamlGame<BakeryBash>.Create(launchArguments, Window.Current.CoreWindow, swapChainPanel);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}
	}
}
