using System.Diagnostics;
using BakeryBash.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monocle;

namespace BakeryBash {

	public class BakeryBash : Engine
	{
		public static Coroutine SaveRoutine;
		public static Vector2 Center => new Vector2(Engine.Width / 2, Engine.Height / 2);
		public static VirtualRenderTarget RenderTarget;


		public BakeryBash(int width, int height, bool fullscreen) : base(1920, 1080, width, height, "Bakery Bash", fullscreen, true)
		{
			Version = new System.Version(0, 1, 0, 0);
			Content.RootDirectory = "Content";
			Window.Title = "Bakery Bash";
			ExitOnEscapeKeypress = false;
			Microsoft.Xna.Framework.Input.Touch.TouchPanel.EnableMouseGestures = true;
			Microsoft.Xna.Framework.Input.Touch.TouchPanel.EnableMouseTouchPoint = true;
		}

		public BakeryBash() : base(1920, 1080, 1920, 1080, "Bakery Bash", true, true)
		{

		}

		protected override void LoadContent()
		{
			GFX.Load();
			GFX.LoadData();
			GFX.LoadEffects();
			Fonts.Prepare();
			Fonts.LoadFonts();
			Choosers.Load();
			base.LoadContent();
		}

		protected override void Initialize()
		{
			base.Initialize();
			Settings.Reload();
			UserIO.TryLoad();
			Tags.InitializeBitTags();
			Input.Initialize();

			RenderTarget = VirtualContent.CreateRenderTarget("RenderTarget", 1922, 1082);

			Scene = new TitleScreen();
		}


		protected override void Update(GameTime gameTime)
		{

			base.Update(gameTime);

			if (SaveRoutine != null)
			{
				SaveRoutine.Update();
			}
		}


	}

}