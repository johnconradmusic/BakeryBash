using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using BakeryBash.Scenes;

namespace BakeryBash.Entities
{
	public class LightningBolt : Entity
	{
		Sprite sprite;
		float lifespan = 0.3f;
		const int boltlength = 500;
		Entity entityA, entityB;
		public LightningBolt(Entity a, Entity b)
		{
			entityA = a;
			entityB = b;

			Depth = -1000;
			var centerPosition = Vector2.Lerp(a.Position, b.Position, 0.5f);

			//sprite.Scale = new Vector2(Vector2.Distance(a.Position, b.Position) / boltlength);
			var angle = Calc.Angle(a.Position, b.Position);

			Position = centerPosition;
			Add(sprite = GFX.SpriteBank.Create("lightning"));
			sprite.Rotation = angle;// + MathHelper.PiOver2;

		}
		float flipTime = 0.09f;
		float counter;
		public override void Update()
		{
			base.Update();
			if (entityB == null || entityA == null || !entityB.Active || !entityA.Active) RemoveSelf();

			counter += Engine.DeltaTime;
			lifespan -= Engine.DeltaTime;
			if (lifespan <= 0) RemoveSelf();
			if (counter >= flipTime)
			{
				counter = 0;
				sprite.FlipX = !sprite.FlipX;
			}
		}
	}
}