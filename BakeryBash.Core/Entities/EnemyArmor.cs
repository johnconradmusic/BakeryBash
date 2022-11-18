using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BakeryBash.Entities
{
	public abstract class EnemyArmor : Entity
	{
		protected Sprite Sprite;

		public int health, maxHealth;
		Vector2 offsetFromEnemy;
		Vector2 gravity = new Vector2(0, 25);
		bool IsEquipped;
		Vector2 velocity = new(0, 0);
		Enemy enemy;
		public EnemyArmor(Enemy enemy, int depth, Vector2 offset, int health = 0)
		{
			this.Depth = depth;
			this.enemy = enemy;
			this.health = this.maxHealth = health;
			this.offsetFromEnemy = offset;
			this.Position = enemy.Position + offset;
			IsEquipped = true;
		}



		public virtual void Hit(int amount)
		{
			if (!IsEquipped) return;

			health -= amount;
			if (health <= 0)
			{
				enemy.RemoveArmor(this);
				IsEquipped = false;
				Destroy();

				velocity = new(Calc.Random.Range(-3, 4), -10);
				velocity.Normalize();
				velocity *= 500f;
			}
		}

		public abstract void Destroy();

		public override void Update()
		{
			base.Update();

			Sprite.Position = enemy.sprite.Position;
			Sprite.Scale = enemy.sprite.Scale;

			if (IsEquipped)
				Position = enemy.Position + offsetFromEnemy;

			Position += velocity * Engine.DeltaTime;

			if (!IsEquipped)
				velocity += gravity;
		}
	}
}