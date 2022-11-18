#define IOS

using Foundation;
using System;
using UIKit;

namespace BakeryBash.iOS
{
	[Register("AppDelegate")]
	internal class Program : UIApplicationDelegate
	{
		private static BakeryBash game;

		internal static void RunGame()
		{
			game = new BakeryBash(1920,1080,true);
			game.Run();
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			UIApplication.Main(args, null, typeof(Program));
		}

		public override void FinishedLaunching(UIApplication app)
		{
			RunGame();
		}
	}
}
