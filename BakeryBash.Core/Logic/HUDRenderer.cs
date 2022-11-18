using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BakeryBash
{
	public class HUDRenderer : HiresRenderer
	{
		public float BackgroundFade;

		public override void RenderContent(Scene scene)
		{
			HiresRenderer.BeginRender();
			if ((double)this.BackgroundFade > 0.0)
				Draw.Rect(-1f, -1f, 1922f, 1082f, Color.Black * this.BackgroundFade * 0.8f);
			scene.Entities.RenderOnly(Tags.HUD);
			HiresRenderer.EndRender();
		}
	}
}