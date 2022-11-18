using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BakeryBash
{
	public abstract class UIElement
	{
		public UIElement(Vector2 position)
		{
			Position = position;
		}

		public UI Parent;
		public MTexture Texture;
		public Vector2 Position;
		protected Vector2 RenderPosition => Parent.Position + Position;
		abstract public float Height { get; }
		abstract public float Width { get; }
		abstract public void Update();
		abstract public void Render();
	}
}