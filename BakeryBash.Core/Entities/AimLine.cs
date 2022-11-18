using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BakeryBash.Entities
{
	public class AimLine : Entity
	{
		float maxDistance = 400f;
		float actualDistance;
		MTexture lineTexture;
		MTexture targetTexture;
		Vector2 targetPos;
		bool collision;
		float angle;
		Color transparentWhite;
		public AimLine()
		{
			lineTexture = GFX.Game["Gameplay/aim-line"];
			targetTexture = GFX.Game["Gameplay/aim-line-target"];
			//Add(sprite = GFX.SpriteBank.Create("aim-line"));
			Collider = new Circle(Ball.BALLRADIUS);
			transparentWhite = new Color(1, 1, 1, 0f);
		}

		public void SetRotation(float angle)
		{
			//sprite.Rotation = -angle + MathHelper.Pi;
			this.angle = -angle + MathHelper.PiOver2;
		}

		public override void Update()
		{
			base.Update();
			collision = false;
			Vector2 pos = default;
			var end = Position + Calc.AngleToVector(angle, maxDistance);
			for (int i = 0; i < maxDistance; i++)
			{
				pos = Vector2.Lerp(Position, end, i / maxDistance);
				actualDistance = Math.Clamp(i, 0, maxDistance);
				//if (CollideLine(Position, pos))
				if (CollideCheck<Enemy>(pos))
				{
					collision = true;
					break;
				}
				if (CollideCheck<Wall>(pos))
				{
					collision = true;
					break;
				}
			}
			targetPos = pos;
		}

		float pixelOffset = 0;
		float scrollSpeed = 60f;
		public override void Render()
		{
			Vector2 start, end;
			start = Position;
			end = Position + Calc.AngleToVector(angle, actualDistance);
			float spacing = 30;
			if (pixelOffset >= spacing)
				pixelOffset -= spacing;
			pixelOffset += scrollSpeed * Engine.DeltaTime;
			Vector2 offsetPos = Calc.AngleToVector(angle, pixelOffset);
			int divisions = (int)Math.Round(actualDistance / spacing);
			Color color;
			for (int i = 0; i < divisions; i++)
			{
				if (i == divisions - 1)
					color = Color.Lerp(Color.White, transparentWhite, pixelOffset/spacing);
				else color = Color.White;
				var pos = Vector2.Lerp(start, end, (float)i / divisions);
				if (i * spacing < actualDistance)
					lineTexture.DrawCentered(pos + offsetPos, color, 1, angle + MathHelper.PiOver2);
			}
			if (collision) targetTexture.DrawCentered(targetPos);
		}

	}
}