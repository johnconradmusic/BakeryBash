using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using BakeryBash.Scenes;

namespace BakeryBash
{

	public class Debris : Entity
	{
		Image sprite;
		Vector2 velocity;
		Vector2 gravity = new(0, 30);


		private Debris(Image sprite, Vector2 position, Vector2 velocity)
		{
			Position = position;
			sprite.Rotation = Calc.Random.NextAngle();
			Add(this.sprite = sprite);
			this.sprite = sprite;
			this.velocity = velocity;
		}

		public static Debris Spawn(Image sprite, Vector2 position, Vector2 velocity)
		{
			Debris debris = new Debris(sprite, position, velocity);
			Level.Instance.Add(debris);
			return debris;
		}

		public override void Update()
		{
			base.Update();
			velocity += gravity;
			Position += velocity * Engine.DeltaTime;
		}
	}
}