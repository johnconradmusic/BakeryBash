using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BakeryBash.Scenes;
using System.Collections;
using Microsoft.Xna.Framework;

namespace BakeryBash.Entities
{
	[Tracked(true)]
	public abstract class Enemy : GridEntity
	{
		protected int health, maxhealth;
		protected List<EnemyArmor> armor = new();
		public bool isMissileTarget;
		public bool IsPoisoned;
		protected HealthBar healthBar;
		public bool IsDead;
		public int Reactions = 0;

		public abstract string ID { get; }
		//public enum HitType
		//{
		//	Ball, Shock, Poison, Bomb, Tack, Missile
		//}
		public enum HitSide
		{
			Center, Top, Right, Bottom, Left
		}
		public Enemy(int hp)
		{
			health = maxhealth = hp;
			Tag = Tags.ObstaclesTag;
			Engine.Scene.Add(healthBar = new HealthBar());
			healthBar.Visible = true;
		}

		public void AddArmor(EnemyArmor prop)
		{
			armor.Add(prop);
			Level.Instance.Add(prop);
		}

		public void RemoveArmor(EnemyArmor prop)
		{
			if (armor.Contains(prop))
				armor.Remove(prop);
		}

		public void Hit(int amount, DamageEffect damageEffect, HitSide hitSide, Entity sender)
		{
			Add(new Coroutine(HitRoutine(amount, damageEffect, hitSide, sender)));
		}



		public abstract IEnumerator HitRoutine(int amount, DamageEffect damageEffect, HitSide hitSide, Entity sender);


		public void ReactToDirectionalHit(HitSide hitSide, float amount = 5)
		{
			switch (hitSide)
			{
				case HitSide.Top:
					Level.Instance.ParticlesBG.Emit(ParticleTypes.BreadCrumbs, 8, Position, new Vector2(40, 40), new Vector2(0, 1).Angle());
					sprite.Scale = new Vector2(1.1f, 0.8f);
					sprite.Position = new Vector2(0, amount);
					break;
				case HitSide.Right:
					Level.Instance.ParticlesBG.Emit(ParticleTypes.BreadCrumbs, 8, Position, new Vector2(40, 40), new Vector2(-1, 0).Angle());
					sprite.Scale = new Vector2(0.8f, 1.1f);
					sprite.Position = new Vector2(-amount, 0);
					break;
				case HitSide.Bottom:
					Level.Instance.ParticlesBG.Emit(ParticleTypes.BreadCrumbs, 8, Position, new Vector2(40, 40), new Vector2(0, -1).Angle());
					sprite.Scale = new Vector2(1.1f, 0.8f);
					sprite.Position = new Vector2(0, -amount);
					break;
				case HitSide.Left:
					Level.Instance.ParticlesBG.Emit(ParticleTypes.BreadCrumbs, 8, Position, new Vector2(40, 40), new Vector2(1, 0).Angle());
					sprite.Scale = new Vector2(0.8f, 1.1f);
					sprite.Position = new Vector2(amount, 0);
					break;
			}
		}

		public void ApplyStatusEffect()
		{
			if (health <= 0) return;
			if (IsPoisoned)
			{
				Hit(GameManager.Instance.PlayerAttributes.PoisonDamage, DamageEffect.Poison, HitSide.Center, this);
			}
		}

		public override void Update()
		{
			//tween the sprite back to center after getting hit
			sprite.Position = Calc.Approach(sprite.Position, Vector2.Zero, 40f * Engine.DeltaTime);
			healthBar.Position = Position;
			base.Update();
		}



	}
}