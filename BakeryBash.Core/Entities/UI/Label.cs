using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BakeryBash
{
	public class Label : UIElement
	{
		PixelFont font;
		float size;
		string text;
		public Vector2 Justify = new Vector2(0.5f);

		public override float Height => throw new NotImplementedException();

		public override float Width => throw new NotImplementedException();

		public Label(PixelFont font, float size, string text, Vector2 position):base(position)
		{
			this.font = font;
			this.size = size;
			this.text = text;
			Position = position;
		}
		public override void Render()
		{
			font.Draw(size, text, Position, Justify, Vector2.One, Color.White);
		}

		public override void Update() { }
	}
}