using Microsoft.Xna.Framework.Input;
using Monocle;
using System;
using System.Xml;
using System.Xml.Serialization;

namespace BakeryBash
{
	[Serializable]
	public class Settings
	{
		public static Settings Instance;
		public static bool Existed;
		public static string LastVersion;
		public const string EnglishLanguage = "english";
		public string Version;
		public string DefaultFileName = "";
		public bool Fullscreen = false;
		public int WindowScale = 5;
		public int ViewportPadding;
		public bool VSync = true;
		public bool DisableFlashes;
		public int MusicVolume = 10;
		public int SFXVolume = 10;
		public int LastSaveFile;
		public string Language = "english";

		public int AimSensitivity = 5;

		public Binding Left = new Binding();
		public Binding Right = new Binding();
		public Binding Down = new Binding();
		public Binding Up = new Binding();
		public Binding MenuLeft = new Binding();
		public Binding MenuRight = new Binding();
		public Binding MenuDown = new Binding();
		public Binding MenuUp = new Binding();
		public Binding Launch = new Binding();
		public Binding Pause = new Binding();
		public Binding Confirm = new Binding();
		public Binding Cancel = new Binding();

		public const string Filename = "settings";

		public Settings()
		{

		}

		public void AfterLoad()
		{
			Binding.SetExclusive(this.MenuLeft, this.MenuRight, this.MenuUp, this.MenuDown, this.Confirm, this.Cancel, this.Pause);
			this.MusicVolume = Calc.Clamp(this.MusicVolume, 0, 10);
			this.SFXVolume = Calc.Clamp(this.SFXVolume, 0, 10);
			this.WindowScale = Math.Min(this.WindowScale, this.MaxScale);
			this.WindowScale = Calc.Clamp(this.WindowScale, 3, 10);
			this.SetDefaultKeyboardControls(false);
			this.SetDefaultButtonControls(false);
		}

		public void SetDefaultKeyboardControls(bool reset)
		{
			if(reset || this.Launch.Keyboard.Count <= 0)
			{
				this.Launch.Keyboard.Clear();
				this.Launch.Keyboard.Add(Keys.Space);
			}
			if (reset || this.Left.Keyboard.Count <= 0)
			{
				this.Left.Keyboard.Clear();
				this.Left.Add(Keys.Left);
			}
			if (reset || this.Right.Keyboard.Count <= 0)
			{
				this.Right.Keyboard.Clear();
				this.Right.Add(Keys.Right);
			}
			if (reset || this.Down.Keyboard.Count <= 0)
			{
				this.Down.Keyboard.Clear();
				this.Down.Add(Keys.Down);
			}
			if (reset || this.Up.Keyboard.Count <= 0)
			{
				this.Up.Keyboard.Clear();
				this.Up.Add(Keys.Up);
			}
			if (reset || this.MenuLeft.Keyboard.Count <= 0)
			{
				this.MenuLeft.Keyboard.Clear();
				this.MenuLeft.Add(Keys.Left);
			}
			if (reset || this.MenuRight.Keyboard.Count <= 0)
			{
				this.MenuRight.Keyboard.Clear();
				this.MenuRight.Add(Keys.Right);
			}
			if (reset || this.MenuDown.Keyboard.Count <= 0)
			{
				this.MenuDown.Keyboard.Clear();
				this.MenuDown.Add(Keys.Down);
			}
			if (reset || this.MenuUp.Keyboard.Count <= 0)
			{
				this.MenuUp.Keyboard.Clear();
				this.MenuUp.Add(Keys.Up);
			}
			
			if (reset || this.Pause.Keyboard.Count <= 0)
			{
				this.Pause.Keyboard.Clear();
				this.Pause.Add(Keys.X);
			}
			if (reset || this.Confirm.Keyboard.Count <= 0)
			{
				this.Confirm.Keyboard.Clear();
				this.Confirm.Add(Keys.Enter);
			}
			if (reset || this.Cancel.Keyboard.Count <= 0)
			{
				this.Cancel.Keyboard.Clear();
				this.Cancel.Add(Keys.Back);
			}
			
			if (!reset)
				return;			
		}

		public void SetDefaultButtonControls(bool reset)
		{
			if (reset || this.Launch.Controller.Count <= 0)
			{
				this.Launch.Controller.Clear();
				this.Launch.Controller.Add(Buttons.A);
			}
			if (reset || this.Left.Controller.Count <= 0)
			{
				this.Left.Controller.Clear();
				this.Left.Add(Buttons.LeftThumbstickLeft, Buttons.DPadLeft);
			}
			if (reset || this.Right.Controller.Count <= 0)
			{
				this.Right.Controller.Clear();
				this.Right.Add(Buttons.LeftThumbstickRight, Buttons.DPadRight);
			}
			if (reset || this.Down.Controller.Count <= 0)
			{
				this.Down.Controller.Clear();
				this.Down.Add(Buttons.LeftThumbstickDown, Buttons.DPadDown);
			}
			if (reset || this.Up.Controller.Count <= 0)
			{
				this.Up.Controller.Clear();
				this.Up.Add(Buttons.LeftThumbstickUp, Buttons.DPadUp);
			}
			if (reset || this.MenuLeft.Controller.Count <= 0)
			{
				this.MenuLeft.Controller.Clear();
				this.MenuLeft.Add(Buttons.LeftThumbstickLeft, Buttons.DPadLeft);
			}
			if (reset || this.MenuRight.Controller.Count <= 0)
			{
				this.MenuRight.Controller.Clear();
				this.MenuRight.Add(Buttons.LeftThumbstickRight, Buttons.DPadRight);
			}
			if (reset || this.MenuDown.Controller.Count <= 0)
			{
				this.MenuDown.Controller.Clear();
				this.MenuDown.Add(Buttons.LeftThumbstickDown, Buttons.DPadDown);
			}
			if (reset || this.MenuUp.Controller.Count <= 0)
			{
				this.MenuUp.Controller.Clear();
				this.MenuUp.Add(Buttons.LeftThumbstickUp, Buttons.DPadUp);
			}
			
			if (reset || this.Pause.Controller.Count <= 0)
			{
				this.Pause.Controller.Clear();
				this.Pause.Add(Buttons.Start);
			}
			if (reset || this.Confirm.Controller.Count <= 0)
			{
				this.Confirm.Controller.Clear();
				this.Confirm.Add(Buttons.A);
			}
			if (reset || this.Cancel.Controller.Count <= 0)
			{
				this.Cancel.Controller.Clear();
				this.Cancel.Add(Buttons.B);
			}
			
		}

		public int MaxScale => Math.Min(Engine.Instance.GraphicsDevice.Adapter.CurrentDisplayMode.Width / 320, Engine.Instance.GraphicsDevice.Adapter.CurrentDisplayMode.Height / 180);

		public void ApplyVolumes()
		{
			this.ApplySFXVolume();
			this.ApplyMusicVolume();
		}

		public void ApplySFXVolume() => throw new NotImplementedException();

		public void ApplyMusicVolume() => throw new NotImplementedException();

		public void ApplyScreen()
		{
			//if (this.Fullscreen)
			//{
			//	Engine.ViewPadding = this.ViewportPadding;
			//	Engine.SetFullscreen();
			//}
			//else
			//{
			//	Engine.ViewPadding = 0;
			//	Engine.SetWindowed(320 * this.WindowScale, 180 * this.WindowScale);
			//}
		}

		public void ApplyLanguage()
		{
			//if (!Dialog.Languages.ContainsKey(this.Language))
			//	this.Language = "english";
			//Dialog.Language = Dialog.Languages[this.Language];
			//Fonts.Load(Dialog.Languages[this.Language].FontFace);
		}

		public static void Initialize()
		{
            if (UserIO.Open(UserIO.Mode.Read))
            {
                Settings.Instance = UserIO.Load<Settings>("settings");
                UserIO.Close();
            }
            Settings.Existed = Settings.Instance != null;
            if (Settings.Instance != null)
                return;
            Settings.Instance = new Settings();
        }

		public static void Reload()
		{
			Settings.Initialize();
			Settings.Instance.AfterLoad();
			Settings.Instance.ApplyScreen();
			//Settings.Instance.ApplyVolumes();
			//Settings.Instance.ApplyLanguage();

		}
	}
}
