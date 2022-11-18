using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BakeryBash
{
	public class DangerLights : Entity
	{
		MTexture texture;
		float angle;


		public override void Added(Scene scene)
		{
			angle = Calc.Random.NextAngle();
			base.Added(scene);
			texture = GFX.Game["Primitives/circle"];
			Depth = -1000;
		}

		public override void Update()
		{
			base.Update();
			angle += Engine.DeltaTime;
			angle = Calc.WrapAngle(angle);
		}

		public override void Render()
		{
			base.Render();
			texture.DrawCentered(Position + Calc.AngleToVector(angle, 200), new Color(Color.Red, 0.1f), 3f);
			texture.DrawCentered(Position + Calc.AngleToVector(angle + MathHelper.Pi, 200), new Color(Color.Red, 0.1f), 4f);
		}
	}
}