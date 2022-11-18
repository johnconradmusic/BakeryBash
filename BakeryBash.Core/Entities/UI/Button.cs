using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BakeryBash
{
	public abstract class Button : SelectableUIElement
	{
		public Button(Vector2 position) : base(position)
		{

		}
		public Action Pressed;
	}
}