using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Monocle;

namespace BakeryBash
{
	public class FadeToColor : ScreenWipe
	{
		Color color;
		float alpha;
		public FadeToColor(Color color, Scene scene, bool wipeIn, Action onComplete = null) : base(scene, wipeIn, onComplete)
		{
			this.color = color;
		}

		public override void Render(Scene scene)
		{
			alpha = WipeIn ? 1 - Ease.QuadInOut(Percent) : Ease.QuadInOut(Percent);

			if (alpha > 0)
			{
				Monocle.Draw.SpriteBatch.Begin();
				Monocle.Draw.Pixel.DrawCentered(new Vector2(Engine.ViewWidth / 2, Engine.ViewHeight / 2), color * alpha, Engine.ViewWidth);
				Monocle.Draw.SpriteBatch.End();
			}
		}

	}
}

