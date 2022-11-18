using System;
using Monocle;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace BakeryBash
{
	public static class GFX
	{
		public static Atlas Game;
		public static SpriteBank SpriteBank;
		public static BasicEffect FXDebug;
		public static bool Loaded { get; private set; } = false;
		public static bool DataLoaded { get; private set; } = false;
		public static void Load()
		{
			if (!GFX.Loaded)
			{
				Stopwatch stopwatch = Stopwatch.StartNew();

				Game = Atlas.FromAtlas("Atlases/Sprites", Atlas.AtlasDataFormat.CrunchXmlOrBinary);
				//Draw.Particle = Game["Effects/particle"];
				//Draw.Pixel = Draw.Particle.GetSubtexture(1, 2, 1, 1);

				ParticleTypes.Load();

				Calc.Log(" - GFX LOAD: " + (object)stopwatch.ElapsedMilliseconds + "ms");
			}
			FXDebug = new BasicEffect(Engine.Graphics.GraphicsDevice);

            GFX.Loaded = true;
		}

		public static void LoadData()
		{
			if (!GFX.DataLoaded)
			{
				Stopwatch stopwatch = Stopwatch.StartNew();

				SpriteBank = new SpriteBank(Game, "spriteData/SpriteData.xml");				
				
				Calc.Log(" - GFX DATA LOAD: " + (object)stopwatch.ElapsedMilliseconds + "ms");
			}
			GFX.DataLoaded = true;
		}
		public static void LoadEffects()
		{

		}

		public static Effect LoadFx(string name) => Engine.Instance.Content.Load<Effect>(Path.Combine("Effects", name));
		        
    }
}


