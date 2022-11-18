using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryBash
{
	public abstract class SelectableUIElement : UIElement
	{
		public bool Selected;
		public SelectableUIElement(Vector2 position) : base(position)
		{

		}
		public abstract void Enter();
		public abstract void Leave();


	}
}