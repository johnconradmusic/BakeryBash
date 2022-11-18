using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BakeryBash
{
	public class TextButton : Button
	{

		PixelFont font;
		float size;
		public Vector2 Justify = new Vector2(0.5f);
		public string text;
		float sizeMultiplier = 1;

		float selectAnimTime = 0.4f;
		float sizeMin = 1;
		float sizeMax = 1.15f;
		float counter;
		Vector2 targetPos;
		bool scaling;
		public override float Height => size;

		public override float Width => 0;

		public TextButton(PixelFont font, string text, float size, Vector2 position, Vector2 justify, Action pressed) : base(position)
		{
			this.targetPos = position;
			this.text = text;
			this.Pressed = pressed;
			this.font = font;
			this.size = size;
			Justify = justify;
		}

		public override void Enter()
		{
			Selected = true;
			scaling = true;
			counter = 0;
		}

		public override void Leave()
		{
			Selected = false;
			scaling = true;
			counter = 0;
		}

		public override void Render()
		{
			font.Draw(size, text, RenderPosition, Justify, Vector2.One * sizeMultiplier, Selected ? Colors.TitleColor : Color.White);
		}

		public override void Update()
		{


			if (scaling)
			{
				counter += Engine.DeltaTime;
				if (Selected)
					sizeMultiplier = MathHelper.Lerp(sizeMin, sizeMax, Ease.ElasticOut(counter / selectAnimTime));
				else
					sizeMultiplier = MathHelper.Lerp(sizeMax, sizeMin, Ease.ElasticOut(counter / selectAnimTime));

				if (counter >= selectAnimTime) scaling = false;
			}
		}
	}
}