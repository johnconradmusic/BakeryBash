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

namespace BakeryBash.Entities
{
	public class SpecialBallPickup : PickupItem
	{
		Ball.BallType ballType;
		public SpecialBallPickup(Ball.BallType type)
		{
			Add(sprite = GFX.SpriteBank.Create("ball"));

			//var type = (Ball.BallType)Calc.Random.Range(0, Enum.GetValues<Ball.BallType>().Count());
			//while (type == Ball.BallType.Normal) type = (Ball.BallType)Calc.Random.Range(0, Enum.GetValues<Ball.BallType>().Count());
			ballType = type;
			switch (type)
			{
				case Ball.BallType.Shock:
					sprite.Play("shock");
					break;
				case Ball.BallType.Bomb:
					sprite.Play("bomb");
					break;
				case Ball.BallType.Poison:
					sprite.Play("poison");
					break;
				case Ball.BallType.Multiball:
					sprite.Play("multiball-loop", false, true);
					break;
			}
		}

		public override IEnumerator DoPickup(Entity other)
		{
			SceneAs<Level>().ParticlesFG.Emit(ParticleTypes.PickupCollected, 80, Position, new(20));
			SceneAs<Level>().ParticlesFG.Emit(ParticleTypes.PickupCollected, 80, Level.Instance.BallQueue.Position, new(20));

			Scene.Add(QuickText.Create(Fonts.ComicGecko, 30, ballType.ToString() + " Ball!", Position + new Vector2(Calc.Random.Range(-1, 1) * 20, Calc.Random.Range(-1, 1) * 10) - Vector2.UnitY * Level.GridSize / 6, Color.White, 1, true, new Vector2(0, -40)));

			GameManager.Instance.NextBalls.Enqueue(ballType);
			Events.WeaponChanged?.Invoke(default);
			yield return null;
		}

		public override void Update()
		{
			if (Scene.OnInterval(1))
			{
				SceneAs<Level>().ParticlesBG.Emit(ParticleTypes.SpecialBall, Position);
			}
			base.Update();
		}
	}
}