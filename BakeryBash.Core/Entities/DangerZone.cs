using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BakeryBash
{
	public class DangerZone : Entity
	{
		float scrollSpeed = 5f;

		MTexture texture;
		int drawWidth;
		float alpha;
		SineWave sine;
		public override void Awake(Scene scene)
		{
			base.Awake(scene);
			Add(sine = new SineWave(0.5f));
			
			texture = GFX.Game["Gameplay/danger-zone"];
			drawWidth = Level.GridColumns * Level.GridSize;
			Y = Level.GridSize * Level.GridRows;
			X = Level.GameArea.Left;
			Depth = 100;
		}

		public override void Update()
		{
			base.Update();
			alpha = sine.ValueOffset(2)/3;
		}

		public override void Render()
		{
			for (int i = 0; i < Level.GridColumns; i++)
			{
				texture.Draw(new Vector2(X + i * Level.GridSize, Y), Vector2.Zero, new Color(Color.White, alpha));
			}
		}
	}
}