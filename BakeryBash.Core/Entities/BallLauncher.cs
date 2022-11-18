using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using BakeryBash.Scenes;
using System.Collections;

namespace BakeryBash.Entities
{
	public class BallLauncher : Entity
	{
		private float launchAngle = -MathHelper.Pi;
		private Vector2 launchDirection => new Vector2(MathF.Sin(launchAngle), MathF.Cos(launchAngle));
		Sprite nextBallSprite;
		public const float BALL_LAUNCH_INTERVAL = 0.09f;
		public const float MULTIBALL_INTERVAL = 0.07f;
		public static Vector2 LAUNCH_POSITION = new Vector2(Level.GameArea.Left + Level.GameArea.Width / 2, Level.GameArea.Bottom);
		private AimLine dashLine;
		public bool IsReady;

		public BallLauncher(Scene scene)
		{
			scene.Add(dashLine = new AimLine());
			dashLine.Visible = true;

			Position = LAUNCH_POSITION = new Vector2(Level.GameArea.Left + Level.GameArea.Width / 2, Level.GameArea.Bottom);
			dashLine.Position = LAUNCH_POSITION;
			Add(nextBallSprite = GFX.SpriteBank.Create("ball"));
			Events.WeaponChanged += WeaponChanged;
			nextBallSprite.Play("normal");
		}



		void WeaponChanged(Ball.BallType ballType)
		{
			switch (ballType)
			{
				case Ball.BallType.Normal:
					nextBallSprite.Play("normal");
					break;
				case Ball.BallType.Shock:
					nextBallSprite.Play("shock");
					break;
				case Ball.BallType.Bomb:
					nextBallSprite.Play("bomb");
					break;
				case Ball.BallType.Poison:
					nextBallSprite.Play("poison");
					break;
				case Ball.BallType.Multiball:
					nextBallSprite.Play("multiball-loop");
					break;
			}
		}

		IEnumerator MultiBallLaunch()
		{
			for (int i = 0; i < GameManager.Instance.PlayerAttributes.MultiBallCount; i++)
			{
				float variance = .1f;
				var dir = new Vector2(
					Calc.Random.Range(launchDirection.X - variance, launchDirection.X + variance),
					Calc.Random.Range(launchDirection.Y - variance, launchDirection.Y + variance));
				dir.Normalize();
				SceneAs<Level>().ParticlesBG.Emit(ParticleTypes.BallLaunch, 10, LAUNCH_POSITION, Vector2.Zero);

				Scene.Add(new Ball(LAUNCH_POSITION, dir, GameManager.Instance.CurrentBallType, true) { damageEffect = DamageEffect.Multiply });
				yield return 0.07f;
			}
			GameManager.Instance.RemoveWait("multiball launch");
			launching = false;
		}
		bool launching;
		public void LaunchBall()
		{
			launching = true;
			if (GameManager.Instance.CurrentBallType == Ball.BallType.Multiball)
			{
				GameManager.Instance.AddWait("multiball launch");
				Add(new Coroutine(MultiBallLaunch()));
			}
			else
			{
				DamageEffect damageEffect = DamageEffect.None;
				switch (GameManager.Instance.CurrentBallType)
				{
					case Ball.BallType.Normal:
						break;
					case Ball.BallType.Shock:
						damageEffect = DamageEffect.Shock;
						break;
					case Ball.BallType.Bomb:
						damageEffect = DamageEffect.LargeExplosion;
						break;
					case Ball.BallType.Poison:
						damageEffect = DamageEffect.Poison;
						break;
					case Ball.BallType.Multiball:
						damageEffect = DamageEffect.Multiply;
						break;
				}
				SceneAs<Level>().ParticlesBG.Emit(ParticleTypes.BallLaunch, 10, LAUNCH_POSITION, Vector2.Zero);
				Scene.Add(new Ball(LAUNCH_POSITION, launchDirection, GameManager.Instance.CurrentBallType, true) { damageEffect = damageEffect });
				launching = false;
			}
		}

		public void SetNewLaunchPosition(float x)
		{
			if (!launching)
				LAUNCH_POSITION.X = Math.Clamp(x, Level.GameArea.Left + 50, Level.GameArea.Right - 50);
		}

		public override void Update()
		{

			launchAngle += -Input.Aim.Value.X * Engine.DeltaTime * Settings.Instance.AimSensitivity * 0.5f;
			if (MInput.Touch.Drag != default)
				launchAngle += -MInput.Touch.Drag.X * Engine.DeltaTime * Settings.Instance.AimSensitivity * 0.02f;

			if (MInput.Mouse.WasMoved)
			{
				//launchAngle = ((MInput.Mouse.Position - LAUNCH_POSITION).Angle() + MathHelper.Pi + MathHelper.PiOver2) * -1;
			}
			launchAngle = Math.Clamp(launchAngle, -4.54f, -1.73f);
			dashLine.SetRotation(launchAngle);
			dashLine.Position = LAUNCH_POSITION;
			IsReady = true;

			if (Position.X != LAUNCH_POSITION.X)
			{
				launchAngle = -MathHelper.Pi;
				Position = Calc.Approach(Position, LAUNCH_POSITION, 2000f * Engine.DeltaTime);
				IsReady = false;
			}

			base.Update();
		}

		public void Hide()
		{
			Visible = false;
			dashLine.Visible = false;
		}
		public void Show()
		{
			Visible = true;
			dashLine.Visible = true;
		}

	}
}