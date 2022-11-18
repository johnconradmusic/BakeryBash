using Monocle;
using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryBash
{
	public class TapHotSpot : Entity
	{
		public TapHotSpot(float x, float y, float width, float height)
		{
			X = x;
			Y = y;
			Collider = new Hitbox(width, height);
			Collider.CenterOrigin();
		}

		public Action OnTap;


		public override void Update()
		{
			base.Update();

			if (MInput.Touch.Tapped || MInput.Mouse.PressedLeftButton)
			{
				if (Collider.Bounds.Contains(MInput.Touch.ScreenPosition) || Collider.Bounds.Contains(MInput.Mouse.Position))
				{
					OnTap?.Invoke();
				}
			}

		}
	}
}
