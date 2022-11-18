using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Threading;

namespace BakeryBash
{
	public class LevelLoader : Scene
	{
		Session session;
		public Level Level { get; private set; }
		private bool started;
		public bool Loaded { get; private set; }

		int world, level;
		public LevelLoader()
		{
			Level = new Level();
			RunThread.Start(new Action(this.LoadingThread), "level loader");
			Add(new EverythingRenderer());
			//Add(new FadeToColor(Color.Black, this, true));
		}

		private void LoadingThread()
		{
			//load level
			//UserIO.SaveHandler(true, false);
			this.Level.LoadLevel();
			Loaded = true;
		}
		private void StartLevel()
		{
			this.started = true;
			if (Engine.Scene != this)
				return;
			Engine.Scene = this.Level;
		}
		public override void Update()
		{
			base.Update();
			if (!this.Loaded || this.started)
				return;
			this.StartLevel();
		}
		public override void Render()
		{
			base.Render();
			Draw.SpriteBatch.Begin();
			//Fonts.ComicGecko.Draw(24, "Loading...", BakeryBash.Center, new(0.5f), Vector2.One, Color.White);
			Draw.SpriteBatch.End();
		}
	}
}