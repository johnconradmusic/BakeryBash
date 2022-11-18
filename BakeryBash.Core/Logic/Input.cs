using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Monocle;
using System;
using System.Collections.Generic;

namespace BakeryBash
{
	public static class Input
	{
		private static int gamepad = 0;
		public static readonly int MaxBindings = 8;
		public static VirtualButton ESC;
		public static VirtualButton Pause;
		public static VirtualButton MenuLeft;
		public static VirtualButton MenuRight;
		public static VirtualButton MenuUp;
		public static VirtualButton MenuDown;
		public static VirtualButton MenuConfirm;
		public static VirtualButton MenuCancel;

		public static VirtualJoystick Aim;

		public static VirtualButton Launch;

		public static Vector2 LastAim;

		public static string OverrideInputPrefix = (string)null;

		private static Dictionary<Keys, string> keyNameLookup = new Dictionary<Keys, string>();
		private static Dictionary<Buttons, string> buttonNameLookup = new Dictionary<Buttons, string>();
		private static Dictionary<string, Dictionary<string, string>> guiPathLookup = new Dictionary<string, Dictionary<string, string>>();
		private static float[] rumbleStrengths = new float[4]
		{
	  0.15f,
	  0.4f,
	  1f,
	  0.05f
		};
		private static float[] rumbleLengths = new float[5]
		{
	  0.1f,
	  0.25f,
	  0.5f,
	  1f,
	  2f
		};

		public static int Gamepad
		{
			get => Input.gamepad;
			set
			{
				int num = Calc.Clamp(value, 0, MInput.GamePads.Length - 1);
				if (Input.gamepad == num)
					return;
				Input.gamepad = num;
				Input.Initialize();
			}
		}

		public static void Initialize()
		{
			Input.Deregister();
			Input.Aim = new VirtualJoystick(Settings.Instance.Up, Settings.Instance.Down, Settings.Instance.Left, Settings.Instance.Right, Input.Gamepad, 0.05f);

			Binding left = new Binding();
			left.Add(Keys.A);
			left.Add(Buttons.RightThumbstickLeft);

			Binding right = new Binding();
			right.Add(Keys.D);
			right.Add(Buttons.RightThumbstickRight);

			Binding up = new Binding();
			up.Add(Keys.W);
			up.Add(Buttons.RightThumbstickUp);

			Binding down = new Binding();
			down.Add(Keys.S);
			down.Add(Buttons.RightThumbstickDown);

			Binding launch = new Binding();
			launch.Add(Buttons.X);
			

			Binding binding = new Binding();
			binding.Add(Keys.Escape);

			Input.ESC = new VirtualButton(binding, Input.Gamepad, 0f, 0.2f);
			Input.Pause = new VirtualButton(Settings.Instance.Pause, Input.Gamepad, 0f, 0.2f);
			Input.Launch = new VirtualButton(Settings.Instance.Launch, Input.Gamepad, 0f, 0.2f);
			Input.MenuLeft = new VirtualButton(Settings.Instance.MenuLeft, Input.Gamepad, 0.0f, 0.4f);
			Input.MenuLeft.SetRepeat(0.4f, 0.1f);
			Input.MenuRight = new VirtualButton(Settings.Instance.MenuRight, Input.Gamepad, 0.0f, 0.4f);
			Input.MenuRight.SetRepeat(0.4f, 0.1f);
			Input.MenuUp = new VirtualButton(Settings.Instance.MenuUp, Input.Gamepad, 0.0f, 0.4f);
			Input.MenuUp.SetRepeat(0.4f, 0.1f);
			Input.MenuDown = new VirtualButton(Settings.Instance.MenuDown, Input.Gamepad, 0.0f, 0.4f);
			Input.MenuDown.SetRepeat(0.4f, 0.1f);
			Input.MenuConfirm = new VirtualButton(Settings.Instance.Confirm, Input.Gamepad, 0.0f, 0.2f);
			Input.MenuCancel = new VirtualButton(Settings.Instance.Cancel, Input.Gamepad, 0.0f, 0.2f);
		}

		public static void Deregister()
		{
			if (Input.ESC != null)
				Input.ESC.Deregister();
			if (Input.Pause != null)
				Input.Pause.Deregister();
			if (Input.MenuLeft != null)
				Input.MenuLeft.Deregister();
			if (Input.MenuRight != null)
				Input.MenuRight.Deregister();
			if (Input.MenuUp != null)
				Input.MenuUp.Deregister();
			if (Input.MenuDown != null)
				Input.MenuDown.Deregister();
			if (Input.MenuConfirm != null)
				Input.MenuConfirm.Deregister();
			if (Input.MenuCancel != null)
				Input.MenuCancel.Deregister();
			if (Input.Aim != null)
				Input.Aim.Deregister();
			if (Input.Launch != null)
				Input.Launch.Deregister();

		}

		public static bool AnyGamepadConfirmPressed(out int gamepadIndex)
		{
			bool flag = false;
			gamepadIndex = -1;
			int gamepadIndex1 = Input.MenuConfirm.GamepadIndex;
			for (int index = 0; index < MInput.GamePads.Length; ++index)
			{
				Input.MenuConfirm.GamepadIndex = index;
				if (Input.MenuConfirm.Pressed)
				{
					flag = true;
					gamepadIndex = index;
					break;
				}
			}
			Input.MenuConfirm.GamepadIndex = gamepadIndex1;
			return flag;
		}

		public static void Rumble(float strength, float length)
		{
			MInput.GamePads[Input.Gamepad].Rumble(strength, length);
		}



		public static string GuiInputPrefix(Input.PrefixMode mode = Input.PrefixMode.Latest)
		{
			if (!string.IsNullOrEmpty(Input.OverrideInputPrefix))
				return Input.OverrideInputPrefix;
			return (mode != Input.PrefixMode.Latest ? MInput.GamePads[Input.Gamepad].Attached : MInput.ControllerHasFocus) ? "xb1" : "keyboard";
		}

		public static bool GuiInputController(Input.PrefixMode mode = Input.PrefixMode.Latest) => !Input.GuiInputPrefix(mode).Equals("keyboard");

		public static MTexture GuiButton(VirtualButton button, Input.PrefixMode mode = Input.PrefixMode.Latest, string fallback = "controls/keyboard/oemquestion")
		{
			string prefix = Input.GuiInputPrefix(mode);
			int num = Input.GuiInputController(mode) ? 1 : 0;
			string input = "";
			if (num != 0)
			{
				using (List<Buttons>.Enumerator enumerator = button.Binding.Controller.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						Buttons current = enumerator.Current;
						if (!Input.buttonNameLookup.TryGetValue(current, out input))
							Input.buttonNameLookup.Add(current, input = current.ToString());
					}
				}
			}
			else
			{
				Keys key = Input.FirstKey(button);
				if (!Input.keyNameLookup.TryGetValue(key, out input))
					Input.keyNameLookup.Add(key, input = key.ToString());
			}
			MTexture mtexture = Input.GuiTexture(prefix, input);
			return mtexture == null && fallback != null ? GFX.Game[fallback] : mtexture;
		}

		public static MTexture GuiSingleButton(Buttons button, Input.PrefixMode mode = Input.PrefixMode.Latest, string fallback = "controls/keyboard/oemquestion")
		{
			string prefix = !Input.GuiInputController(mode) ? "xb1" : Input.GuiInputPrefix(mode);
			string input = "";
			if (!Input.buttonNameLookup.TryGetValue(button, out input))
				Input.buttonNameLookup.Add(button, input = button.ToString());
			MTexture mtexture = Input.GuiTexture(prefix, input);
			return mtexture == null && fallback != null ? GFX.Game[fallback] : mtexture;
		}

		public static MTexture GuiKey(Keys key, string fallback = "controls/keyboard/oemquestion")
		{
			string input;
			if (!Input.keyNameLookup.TryGetValue(key, out input))
				Input.keyNameLookup.Add(key, input = key.ToString());
			MTexture mtexture = Input.GuiTexture("keyboard", input);
			return mtexture == null && fallback != null ? GFX.Game[fallback] : mtexture;
		}

		public static Buttons FirstButton(VirtualButton button)
		{
			using (List<Buttons>.Enumerator enumerator = button.Binding.Controller.GetEnumerator())
			{
				if (enumerator.MoveNext())
					return enumerator.Current;
			}
			return Buttons.A;
		}

		public static Keys FirstKey(VirtualButton button)
		{
			foreach (Keys keys in button.Binding.Keyboard)
			{
				if (keys != Keys.None)
					return keys;
			}
			return Keys.None;
		}

		public static MTexture GuiDirection(Vector2 direction) => Input.GuiTexture("directions", Math.Sign(direction.X).ToString() + "x" + (object)Math.Sign(direction.Y));

		private static MTexture GuiTexture(string prefix, string input)
		{
			Dictionary<string, string> dictionary;
			if (!Input.guiPathLookup.TryGetValue(prefix, out dictionary))
				Input.guiPathLookup.Add(prefix, dictionary = new Dictionary<string, string>());
			string id;
			if (!dictionary.TryGetValue(input, out id))
				dictionary.Add(input, id = "controls/" + prefix + "/" + input);
			if (GFX.Game.Has(id))
				return GFX.Game[id];
			return prefix != "fallback" ? Input.GuiTexture("fallback", input) : (MTexture)null;
		}

		public static void SetLightbarColor(Color color)
		{
		}

		public enum PrefixMode
		{
			Latest,
			Attached,
		}
	}
}
