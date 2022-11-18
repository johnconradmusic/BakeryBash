using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BakeryBash.Entities;
using System.Collections;
using BakeryBash.Scenes;
using Microsoft.Xna.Framework;
using static BakeryBash.Entities.Ball;

namespace BakeryBash
{
	public class MissileLauncherPickup : PickupItem
	{
		int numMissiles = 5;
		float rotation;
		Image platform;
		Image dummyMissile;
		public MissileLauncherPickup()
		{
			Add(sprite = GFX.SpriteBank.Create("missilelauncher"));
			Add(platform = new Image(GFX.Game["Pickups/missile-launcher-platform"]));
			Add(dummyMissile = new Image(GFX.Game["Pickups/missile"]));
			dummyMissile.CenterOrigin();
			dummyMissile.Rotation = Calc.Up;
			platform.CenterOrigin();
		}


		public override IEnumerator DoPickup(Entity other)
		{
			DamageEffect damageEffect;
			if (other is Ball ball) damageEffect = ball.damageEffect;
			else if (other is Tack tack) damageEffect = tack.damageEffect;
			else damageEffect = DamageEffect.SmallExplosion;

			if (damageEffect == DamageEffect.None) damageEffect = DamageEffect.SmallExplosion;

			switch (damageEffect)
			{
				case DamageEffect.Shock:
				case DamageEffect.Poison:
					Scene.Add(QuickText.Create(Fonts.ComicGecko, 30, damageEffect.ToString() + " Missile Launcher!", Position + new Vector2(Calc.Random.Range(-1, 1) * 20, Calc.Random.Range(-1, 1) * 10) - Vector2.UnitY * Level.GridSize / 6, Color.White, 1, true, new Vector2(0, -40)));
					break;
				case DamageEffect.LargeExplosion:
					Scene.Add(QuickText.Create(Fonts.ComicGecko, 30, "Bomb + Missile Launcher!", Position + new Vector2(Calc.Random.Range(-1, 1) * 20, Calc.Random.Range(-1, 1) * 10) - Vector2.UnitY * Level.GridSize / 6, Color.White, 1, true, new Vector2(0, -40)));
					break;
				case DamageEffect.SmallExplosion:
					Scene.Add(QuickText.Create(Fonts.ComicGecko, 30, "Missile Launcher!", Position + new Vector2(Calc.Random.Range(-1, 1) * 20, Calc.Random.Range(-1, 1) * 10) - Vector2.UnitY * Level.GridSize / 6, Color.White, 1, true, new Vector2(0, -40)));
					break;
				case DamageEffect.Multiply:
					Scene.Add(QuickText.Create(Fonts.ComicGecko, 30, "Multi-Missile Launcher!", Position + new Vector2(Calc.Random.Range(-1, 1) * 20, Calc.Random.Range(-1, 1) * 10) - Vector2.UnitY * Level.GridSize / 6, Color.White, 1, true, new Vector2(0, -40)));
					break;
			}

			SceneAs<Level>().ParticlesFG.Emit(ParticleTypes.TackShooter, 80, Position, new(20));
			float timeBetweenMissiles = 0.4f;
			for (int i = 0; i < numMissiles * (damageEffect == DamageEffect.Multiply ? 3 : 1); i++)
			{
				Missile missile;
				var target = Missile.FindTarget();
				if (target != null)
				{
					dummyMissile.RemoveSelf();
					Scene.Add(missile = new Missile(Position, target, damageEffect));
					missile.Launch();
					yield return timeBetweenMissiles;
				}
				else
				{
					break;
				}
			}
		}
	}
}