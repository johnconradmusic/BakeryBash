using System;
using System.Collections;
using BakeryBash.Entities;
using BakeryBash.Scenes;
using Microsoft.Xna.Framework;
using Monocle;

namespace BakeryBash.Entities
{
	public class Toast : Enemy
	{
		Wiggler wiggler;
		public override string ID => "e1";
		public Toast() : base(EnemyStats.Toast.Health)
		{
			Add(sprite = GFX.SpriteBank.Create("toast"));

			Add(wiggler = Wiggler.Create(0.4f, Calc.Random.Range(10, 30), (val) => sprite.Position = Calc.Random.ShakeVector() * 4));
			Collider = new Hitbox(sprite.Width, sprite.Height, -sprite.Width / 2, -sprite.Height / 2);
		}

		public EnemyArmor GetNextArmor()
		{
			if (armor.Count == 0) return new Shield(this);
			if (armor.Count == 1) return new Helmet(this);
			if (armor.Count == 2) return new Faceguard(this);
			if (armor.Count > 2) return null;
			return null;
		}

		void SetFrameForCurrentHealth()
		{
			var healthPercent = (float)health / maxhealth;
			if (IsPoisoned)
			{
				if (healthPercent > 0.75)
					sprite.Play("poisoned-1");
				if (healthPercent <= 0.75f && healthPercent > 0.5)
					sprite.Play("poisoned-2");
				if (healthPercent <= 0.5f && healthPercent > 0.25)
					sprite.Play("poisoned-3");
				if (healthPercent <= 0.25f)
					sprite.Play("poisoned-4");
			}
			else
			{
				if (healthPercent > 0.75f)
					sprite.Play("normal-1");
				if (healthPercent <= 0.75f && healthPercent > 0.5)
					sprite.Play("normal-2");
				if (healthPercent <= 0.5f && healthPercent > 0.25)
					sprite.Play("normal-3");
				if (healthPercent <= 0.25f)
					sprite.Play("normal-4");
			}

		}

		public override IEnumerator HitRoutine(int amount, DamageEffect damageEffect, HitSide hitSide, Entity sender)
		{
			Reactions++;
			if (damageEffect == DamageEffect.Poison) IsPoisoned = true;

			if (damageEffect != DamageEffect.Poison && armor.Count > 0)
				armor[0].Hit(amount);
			else
				health -= amount;

			healthBar.Update((float)health / maxhealth);


			Scene.Add(QuickText.Create(Fonts.ComicGecko, 20, (-amount).ToString(), Position + new Vector2(Calc.Random.Range(-1, 1) * 20, Calc.Random.Range(-1, 1) * 10) - Vector2.UnitY * Level.GridSize / 6, Color.White, 1, true, new Vector2(0, -40)));

			SetFrameForCurrentHealth();

			switch (sender)
			{
				case Ball ball:
					{

					}
					break;
				case Missile missile:
					{
						Scene.Add(new Explosion(Position));

					}
					break;
				case Tack tack:
					{

					}
					break;
				case Enemy enemy:
					{

					}
					break;
			}

			switch (damageEffect)
			{
				case DamageEffect.None:
					{
						ReactToDirectionalHit(hitSide);
						if (IsPoisoned)
							sprite.Play("poisoned-react-" + Calc.Random.Range(1, 3));
						else
							sprite.Play("normal-react-" + Calc.Random.Range(1, 3));

						yield return 0.2f;
						SetFrameForCurrentHealth();
					}
					break;
				case DamageEffect.Shock:
					{
						if (sender is not Enemy)
						{
							ReactToDirectionalHit(hitSide);

							var entities = Level.Instance.GridEntities;
							foreach (var entity in entities)
							{
								if (entity is not Enemy enemy || entity == this) continue;
								float distToEnemy = Vector2.Distance(enemy.Position, Position);
								float range = GameManager.Instance.PlayerAttributes.LightningRange * Level.GridSize;
								if (distToEnemy < range)
								{
									Scene.Add(new LightningBolt(this, enemy));
									enemy.Hit(amount, damageEffect, HitSide.Center, this);
								}
							}
						}

						wiggler.Start(0.4f, 80);

						sprite.Play("shocked");
						for (float t = 0; t < 0.3f; t += Engine.DeltaTime)
						{
							SceneAs<Level>().ParticlesFG.Emit(ParticleTypes.Sparks, 4, Position, new(0));
							yield return null;
						}
						SetFrameForCurrentHealth();
					}
					break;
				case DamageEffect.Poison:
					{
						ReactToDirectionalHit(hitSide);
						sprite.Play("poisoned-react-" + Calc.Random.Range(0, 2));
						yield return 0.2f;
						SetFrameForCurrentHealth();
					}
					break;
				case DamageEffect.SmallExplosion:
					{
						ReactToDirectionalHit(hitSide);

						Scene.Add(new Explosion(Position));
						if (IsPoisoned)
							sprite.Play("poisoned-react-" + Calc.Random.Range(0, 2));
						else
							sprite.Play("normal-react-" + Calc.Random.Range(1, 3));
						wiggler.Start(0.1f, 80);
						yield return 0.2f;
						SetFrameForCurrentHealth();
					}
					break;
				case DamageEffect.LargeExplosion:
					{
						if (sender is not Missile)
							Scene.Add(new Explosion(Position));
						if (sender is not Enemy)
						{
							ReactToDirectionalHit(hitSide);

							//yield return 0.2f;
							SceneAs<Level>().Shake();
							var entities = Level.Instance.GridEntities;
							foreach (var entity in entities)
							{
								if (entity is not Enemy enemy || entity == this) continue;
								float distToEnemy = Vector2.Distance(enemy.Position, Position);
								float range = GameManager.Instance.PlayerAttributes.BombRange * Level.GridSize;
								if (distToEnemy < range)
								{
									enemy.Hit(amount, damageEffect, HitSide.Center, this);
								}
							}
						}
					}
					break;
				case DamageEffect.Multiply:
					{
						ReactToDirectionalHit(hitSide);

						if (IsPoisoned)
							sprite.Play("poisoned-react-" + Calc.Random.Range(1, 3));
						else
							sprite.Play("normal-react-" + Calc.Random.Range(1, 3));
						yield return 0.2f;
						SetFrameForCurrentHealth();
					}
					break;

			}

			if (health <= 0)
			{
				yield return Die();
			}
			else
				Reactions--;

		}

		private IEnumerator Die()
		{
			if (IsDead) yield break;
			Visible = false;
			IsDead = true;
			healthBar.RemoveSelf();
			foreach (var piece in armor)
				piece.RemoveSelf();
			Events.EnemyDestroyed?.Invoke(this);
			Collidable = false;
			if (IsPoisoned)
				Scene.Add(new QuickSprite(GFX.SpriteBank.Create("toast"), "poisoned-dead", Position, Position - Vector2.UnitY * Level.GridSize, 0.6f, true));
			else
				Scene.Add(new QuickSprite(GFX.SpriteBank.Create("toast"), "normal-dead", Position, Position - Vector2.UnitY * Level.GridSize, 0.6f, true));

			Level.Instance.GridEntities.Remove(this);
			//Scene.Add(QuickText.Create(Fonts.SmallGameplayFont, 30, "1000", Position - Vector2.UnitY * Level.GridSize / 6, Color.Yellow, 1, true, new Vector2(0, -40)));
			//TODO should we spawn a coin here?
			yield return 1f;
			Reactions--;
			RemoveSelf();
		}

	}
}

