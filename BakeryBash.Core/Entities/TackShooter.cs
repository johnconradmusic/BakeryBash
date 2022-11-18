using System;
using System.Collections;
using BakeryBash.Entities;
using BakeryBash.Scenes;
using Microsoft.Xna.Framework;
using Monocle;
using static BakeryBash.Entities.Ball;

namespace BakeryBash
{
	public class TackShooter : PickupItem
	{
		public TackShooter()
		{
			Add(sprite = GFX.SpriteBank.Create("tackshooter"));
			sprite.Play("idle");
		}

		public override IEnumerator DoPickup(Entity other)
		{
			DamageEffect damageEffect;
			if (other is Ball ball) damageEffect = ball.damageEffect;
			else if (other is Missile missile) damageEffect = missile.DamageEffect;
			else if (other is Tack tack) damageEffect = tack.damageEffect;
			else damageEffect = DamageEffect.None;

			switch (damageEffect)
			{
				case DamageEffect.None:
					Scene.Add(QuickText.Create(Fonts.ComicGecko, 30, "Tack Shooter!", Position + new Vector2(Calc.Random.Range(-1, 1) * 20, Calc.Random.Range(-1, 1) * 10) - Vector2.UnitY * Level.GridSize / 6, Color.White, 1, true, new Vector2(0, -40)));
					break;
				case DamageEffect.Shock:
					Scene.Add(QuickText.Create(Fonts.ComicGecko, 30, "Electric Tacks!", Position + new Vector2(Calc.Random.Range(-1, 1) * 20, Calc.Random.Range(-1, 1) * 10) - Vector2.UnitY * Level.GridSize / 6, Color.White, 1, true, new Vector2(0, -40)));
					break;
				case DamageEffect.Poison:
					Scene.Add(QuickText.Create(Fonts.ComicGecko, 30, "Poison Tacks!", Position + new Vector2(Calc.Random.Range(-1, 1) * 20, Calc.Random.Range(-1, 1) * 10) - Vector2.UnitY * Level.GridSize / 6, Color.White, 1, true, new Vector2(0, -40)));
					break;
				case DamageEffect.LargeExplosion:
				case DamageEffect.SmallExplosion:
					Scene.Add(QuickText.Create(Fonts.ComicGecko, 30, "Explosive Tacks!", Position + new Vector2(Calc.Random.Range(-1, 1) * 20, Calc.Random.Range(-1, 1) * 10) - Vector2.UnitY * Level.GridSize / 6, Color.White, 1, true, new Vector2(0, -40)));
					break;
				case DamageEffect.Multiply:
					Scene.Add(QuickText.Create(Fonts.ComicGecko, 30, "Multi-Tacks!", Position + new Vector2(Calc.Random.Range(-1, 1) * 20, Calc.Random.Range(-1, 1) * 10) - Vector2.UnitY * Level.GridSize / 6, Color.White, 1, true, new Vector2(0, -40)));
					break;
			}

			SceneAs<Level>().ParticlesFG.Emit(ParticleTypes.TackShooter, 80, Position, new(20));
			if (damageEffect == DamageEffect.Multiply)
			{
				Scene.Add(new Tack(Position, new Vector2(-1, 0), damageEffect));//left
				Scene.Add(new Tack(Position, new Vector2(1, 0), damageEffect));//right
				Scene.Add(new Tack(Position, new Vector2(0, -1), damageEffect));//up
				Scene.Add(new Tack(Position, new Vector2(0, 1), damageEffect));//down
				Scene.Add(new Tack(Position, new Vector2(-1, -1), damageEffect));//topleft
				Scene.Add(new Tack(Position, new Vector2(1, -1), damageEffect));//topright
				Scene.Add(new Tack(Position, new Vector2(-1, 1), damageEffect));//btmleft
				Scene.Add(new Tack(Position, new Vector2(1, 1), damageEffect));//btmright
			}
			else
			{
				Scene.Add(new Tack(Position, new Vector2(-1, 0), damageEffect));
				Scene.Add(new Tack(Position, new Vector2(1, 0), damageEffect));
			}
			sprite.Play("shoot");
			yield return 1;
			yield return null;
		}
	}
}

