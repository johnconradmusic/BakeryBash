using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monocle
{
	public class VirtualTap : VirtualInput
	{
		public Binding Binding;

		public VirtualTap(Binding binding)
		{
			Binding = binding;
		}

		public Vector2 Position => Tapped ? MInput.Touch.Position : default;
		public bool Tapped
		{
			get
			{
				return !MInput.Disabled || Binding.Pressed(0, 0);
			}
		}

		public override void Update()
		{

		}
	}
}
