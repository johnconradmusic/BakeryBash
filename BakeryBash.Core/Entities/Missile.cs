using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BakeryBash.Entities;
using BakeryBash.Scenes;
using Microsoft.Xna.Framework;
using System.Collections;

namespace BakeryBash
{
	public class Missile : Entity
	{
		public DamageEffect DamageEffect;
		Image sprite;
		Vector2 start, prevPos;
		Enemy target;
		float flighttime = 2.5f;
		public Missile(Vector2 start, Enemy target, DamageEffect damageEffect)
		{
			DamageEffect = damageEffect;
			Position = start;
			this.start = start;
			this.target = target;
			Depth = -200;
			Add(sprite = new(GFX.Game["Pickups/missile"]));
			sprite.CenterOrigin();
			sprite.Rotation = Calc.Up;
		}
		public static Enemy FindTarget() => (Enemy)Calc.Random.Choose(Level.Instance.GridEntities.Where(e => e is Enemy enemy && !enemy.isMissileTarget).ToList());

		public float Rotation { get { return sprite.Rotation; } set { sprite.Rotation = value; } }

		public void Launch()
		{
			GameManager.Instance.AddWait("missile");
			Add(new Coroutine(FlightRoutine()));
		}

		IEnumerator FlightRoutine()
		{
			var distance = Vector2.Distance(Position, target.Position);
			float flighttime = 1f;

			yield return 0.3f;
			for (float i = 0; i < flighttime; i += Engine.DeltaTime)
			{
				if (target is not Enemy enemy || enemy.IsDead)
				{
					target = FindTarget();
					if (target == null)
					{
						GameManager.Instance.RemoveWait("missile");
						RemoveSelf();
						yield break;
					}
					target.isMissileTarget = true;
					i = 0;
				}
				Position = Flightpath().GetPoint(Ease.CubeIn(i / flighttime));
				//Position = new Vector2(XPath().Evaluate(Ease.CubeIn(i / flighttime)), YPath().Evaluate(Ease.CubeIn(i / flighttime)));
				if (i != 0)
					Rotation = (Position - prevPos).Angle();
				if (Scene.OnInterval(0.03f))
					SceneAs<Level>().ParticlesFG.Emit(ParticleTypes.MissileTrail, prevPos);
				prevPos = Position;
				yield return null;
			}
			target.isMissileTarget = false;

			SceneAs<Level>().Shake();
			MInput.Touch.Vibrate(2);
			target.Hit(GameManager.Instance.PlayerAttributes.MissileDamage, DamageEffect, Enemy.HitSide.Top, this);
			sprite.Visible = false;
			yield return 0.5f;
			GameManager.Instance.RemoveWait("missile");

			RemoveSelf();
		}

		SimpleCurve Flightpath()
		{
			return new SimpleCurve(start, target.Position, new Vector2(start.X, start.Y - 200));
		}
	}
}