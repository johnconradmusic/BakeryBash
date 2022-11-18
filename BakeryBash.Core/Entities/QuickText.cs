using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BakeryBash.Entities
{
	public class QuickText : Entity
	{
		float lifespan;
		bool fadeout;
		string text;
		Color color;
		float alpha = 1f;
		Vector2 travelDistance;
		private QuickText() { }
		private float lifeRemaining;
		private Vector2 startPos, endPos;
		PixelFont font;
		float size;


		public static QuickText Create(PixelFont font, float size, string text, Vector2 pos, Color color, float lifespan, bool fadeout, Vector2 travel)
		{
			return new QuickText()
			{
				size = size,
				font = font,
				text = text,
				Position = pos,
				color = color,
				lifespan = lifespan,
				lifeRemaining = lifespan,
				fadeout = fadeout,
				travelDistance = travel,
				startPos = pos,
				endPos = pos + travel,
			};
		}

		public override void Update()
		{
			base.Update();
			Position = Vector2.Lerp(endPos, startPos, lifeRemaining / lifespan);
			lifeRemaining -= Engine.DeltaTime;
			if (fadeout)
				alpha -= Engine.DeltaTime / lifespan;
			if (lifeRemaining <= 0) RemoveSelf();
		}

		public override void Render()
		{
			base.Render();
			font.Draw(size, text, Position, new Vector2(0.5f, 0.5f), Vector2.One * 1.2f, new Color(color, alpha));

		}
	}
}