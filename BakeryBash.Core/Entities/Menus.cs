using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using BakeryBash.Scenes;

namespace BakeryBash
{
	public static class Menus
	{
		private static TextMenu titleMenu;
		private static TextMenu optionsMenu;
		private static TextMenu pauseMenu;

		public static TextMenu CreateTitleMenu()
		{
			titleMenu = new TextMenu();
			titleMenu.Position += new Vector2(400, 200);
			titleMenu.Add(new TextMenu.Button("PLAY!").Pressed(() => new ToastWipe(Engine.Scene, false, () => Engine.Scene = new LevelLoader())));
			//titleMenu.Add(new TextMenu.Button("OPTIONS").Pressed(() => OpenOptionsMenu(titleMenu)));
			titleMenu.Add(new TextMenu.Button("EXIT").Pressed(() => new FadeToColor(Color.Black, Engine.Scene, false, () => Environment.Exit(0))));
			return titleMenu;
		}


		private static void OpenOptionsMenu(TextMenu sender)
		{
			sender.Visible = false;
			sender.Focused = false;
			Engine.Scene.Add(optionsMenu = CreateOptionsMenu());

			optionsMenu.OnCancel = (() =>
			{
				optionsMenu.Close();
			}
			);
			optionsMenu.OnClose = (() =>
			{
				UserIO.SaveHandler(false, true);
				sender.Visible = true;
				sender.Focused = true;
			});

			Engine.Scene.OnEndOfFrame += (() => Engine.Scene.Entities.UpdateLists());
		}

		private static TextMenu CreateOptionsMenu()
		{
			optionsMenu = new TextMenu();
			optionsMenu.Add(new TextMenu.Header("OPTIONS"));

			optionsMenu.Add(new TextMenu.SubHeader("SOUND", false));
			optionsMenu.Add(new TextMenu.OnOff("SFX", true));
			optionsMenu.Add(new TextMenu.OnOff("MUSIC", true));

			optionsMenu.Add(new TextMenu.SubHeader("GRAPHICS", false));
			optionsMenu.Add(new TextMenu.OnOff("FULLSCREEN", Settings.Instance.Fullscreen).Change((b) =>
			{
				Settings.Instance.Fullscreen = b;
				Settings.Instance.ApplyScreen();
			}));
			optionsMenu.Add(new TextMenu.Slider("SCREEN ADJUST", (i) => i.ToString(), 1, 100, Settings.Instance.AimSensitivity).Change((amt) =>
			{
				Settings.Instance.ViewportPadding = amt;
				Settings.Instance.ApplyScreen();
			}));

			optionsMenu.Add(new TextMenu.SubHeader("ACCESSIBILITY", false));
			optionsMenu.Add(new TextMenu.OnOff("Screen Flashes", true));

			optionsMenu.Add(new TextMenu.SubHeader("CONTROLS", false));
			optionsMenu.Add(new TextMenu.Slider("AIM SENSITIVITY", (i) => i.ToString(), 1, 10, Settings.Instance.AimSensitivity).Change((amt) => Settings.Instance.AimSensitivity = amt));

			optionsMenu.OnClose += () => UserIO.SaveHandler(false, true);

			return optionsMenu;
		}

		public static TextMenu CreatePauseMenu()
		{
			pauseMenu = new TextMenu();
			pauseMenu.Add(new TextMenu.Header("PAUSED"));
			pauseMenu.Add(new TextMenu.Button("RESUME").Pressed(() =>
			{
				pauseMenu.OnCancel();
				Input.Launch.ConsumePress();
			}));
			pauseMenu.Add(new TextMenu.Button("OPTIONS").Pressed(() => OpenOptionsMenu(pauseMenu)));
			pauseMenu.Add(new TextMenu.Button("EXIT TO MAIN MENU").Pressed(() => Engine.Scene = new TitleScreen()));
			return pauseMenu;
		}
	}
}