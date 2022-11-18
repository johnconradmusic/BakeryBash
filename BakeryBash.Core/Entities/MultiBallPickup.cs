using BakeryBash.Scenes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using static BakeryBash.Entities.Ball;

namespace BakeryBash.Entities;

public class MultiBallPickup : PickupItem
{
	public const float MULTIBALL_INTERVAL = 0.07f;
	float spinSpeed = 6f;
	Ball.BallType BallType;

	public MultiBallPickup()
	{
		Add(sprite = GFX.SpriteBank.Create("pickup"));
		sprite.Play("multiball");
	}


	private IEnumerator DoMultiBallRoutine()
	{
		var numBalls = GameManager.Instance.PlayerAttributes.MultiBallCount;
		//for (float i = 0; i < 0.05f; i += Engine.DeltaTime)
		//{
		//	spinSpeed += MathHelper.ToRadians(i);
		//	yield return null;
		//}
		yield return LaunchBalls(BallType == Ball.BallType.Multiball ? numBalls * 3 : numBalls);
		//for (float i = 0; i < 1; i += Engine.DeltaTime)
		//{
		//	sprite.Scale = Vector2.Lerp(Vector2.One, Vector2.Zero, Ease.QuadInOut(i / 1f));
		//	yield return null;
		//}
	}

	private IEnumerator LaunchBalls(int numOfBalls)
	{
		float launchTime = 0.6f;

		for (int i = 0; i < numOfBalls; i++)
		{
			var percentage = (float)i / (float)numOfBalls;
			var angle = Calc.AngleLerp(MathHelper.ToRadians(95), MathHelper.ToRadians(265), percentage);
			var direction = new Vector2(MathF.Sin(angle), MathF.Cos(angle));
			switch (GameManager.Instance.CurrentBallType)
			{
				case Ball.BallType.Normal:
					Level.Instance.Add(new Ball(Position, direction, Ball.BallType.Multiball, false));
					break;
				case Ball.BallType.Multiball:
					Level.Instance.Add(new Ball(Position, direction, Ball.BallType.Multiball, false));
					break;
				case Ball.BallType.Shock:
					Level.Instance.Add(new Ball(Position, direction, Ball.BallType.Shock, false));
					break;
				case Ball.BallType.Poison:
					Level.Instance.Add(new Ball(Position, direction, Ball.BallType.Poison, false));
					break;
				case Ball.BallType.Bomb:
					Level.Instance.Add(new Ball(Position, direction, Ball.BallType.Bomb, false));
					break;
			}
			yield return launchTime / numOfBalls;
		}
		yield return null;
	}

	public override void Update()
	{
		sprite.Rotation -= spinSpeed * Engine.DeltaTime;
		base.Update();
	}


	public override IEnumerator DoPickup(Entity other)
	{
		SceneAs<Level>().ParticlesFG.Emit(ParticleTypes.PickupCollected, 80, Position, new(20));
		if (other is Ball ball)
		{
			BallType = ball.ballType;
			if (BallType == BallType.Normal)
				Scene.Add(QuickText.Create(Fonts.ComicGecko, 30, "Multiball!", Position + new Vector2(Calc.Random.Range(-1, 1) * 20, Calc.Random.Range(-1, 1) * 10) - Vector2.UnitY * Level.GridSize / 6, Color.White, 1, true, new Vector2(0, -40)));
			else
				Scene.Add(QuickText.Create(Fonts.ComicGecko, 30, BallType.ToString() + " Multiball!", Position + new Vector2(Calc.Random.Range(-1, 1) * 20, Calc.Random.Range(-1, 1) * 10) - Vector2.UnitY * Level.GridSize / 6, Color.White, 1, true, new Vector2(0, -40)));

			yield return DoMultiBallRoutine();
		}
		if (other is Tack tack)
		{
			Scene.Add(QuickText.Create(Fonts.ComicGecko, 30, "Multi-tack!", Position + new Vector2(Calc.Random.Range(-1, 1) * 20, Calc.Random.Range(-1, 1) * 10) - Vector2.UnitY * Level.GridSize / 6, Color.White, 1, true, new Vector2(0, -40)));

			yield return DoTackRoutine();
		}
	}

	private IEnumerator DoTackRoutine()
	{
		var numTacks = GameManager.Instance.PlayerAttributes.MultiBallCount;
		//for (float i = 0; i < 0.3f; i += Engine.DeltaTime)
		//{
		//	spinSpeed += MathHelper.ToRadians(i);
		//	yield return null;
		//}
		yield return LaunchTacks(24);
		//for (float i = 0; i < 1; i += Engine.DeltaTime)
		//{
		//	sprite.Scale = Vector2.Lerp(Vector2.One, Vector2.Zero, Ease.QuadInOut(i / 1f));
		//}
		yield return null;
	}
	private IEnumerator LaunchTacks(int numTacks)
	{
		for (int i = 0; i < numTacks; i++)
		{
			var percentage = (float)i / (float)numTacks;
			var angle = Calc.AngleLerp(MathHelper.ToRadians(0), MathHelper.ToRadians(180), percentage);
			var direction = new Vector2(MathF.Sin(angle), MathF.Cos(angle));
			Level.Instance.Add(new Tack(Position, direction, DamageEffect.None));
		}
		yield return .02f;
		yield return null;
	}
}