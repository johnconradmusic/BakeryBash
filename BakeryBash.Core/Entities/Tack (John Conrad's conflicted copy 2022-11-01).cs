using System;
using System.Collections.Generic;
using System.Linq;
using BakeryBash.Entities;
using BakeryBash.Scenes;
using Microsoft.Xna.Framework;
using Monocle;
namespace BakeryBash
{
	[Tracked]
	public class Tack : Actor
	{
		float speed = 1200;
		Vector2 direction;
		MTexture mTexture;
		float lifespan = 1f;
		float counter;
		List<GridEntity> entitiesHit = new List<GridEntity>();
		Enemy.HitType hitType;
		public Tack(Vector2 position, Vector2 direction, Ball.BallType ballType)
		{
			Collider = new Hitbox(5, 5);
			this.direction = direction;
			direction.Normalize();
			Position = position;
			mTexture = GFX.Game["Pickups/tack"];
			Tag = Tags.PickupsTag;
			CollidesWithTag = Tags.NoneTag;
            switch (ballType)
            {
				case Ball.BallType.Normal:

					break
            }
		}

		public override void Update()
		{
			base.Update();
			counter += Engine.DeltaTime;
			if (counter >= lifespan) RemoveSelf();
			MoveH(direction.X * speed * Engine.DeltaTime, new Collision(OnCollide));
			foreach (var entity in Level.Instance.GridEntities)
			{
				if (this.CollideCheck(entity) && !entitiesHit.Contains(entity))
				{
					entitiesHit.Add(entity);
					if (entity is PickupItem pickup) pickup.Collect(this);
					if (entity is Enemy enemy) enemy.Hit(5, Enemy.HitType.Tack, direction.X < 0 ? Enemy.HitSide.Right : Enemy.HitSide.Left, this);
				}
			}
		}
		private void OnCollide(CollisionData collisionData)
		{
		}
		public override void Render()
		{
			base.Render();
			mTexture.DrawCentered(Position, Color.White, direction.X);
		}
	}
}

