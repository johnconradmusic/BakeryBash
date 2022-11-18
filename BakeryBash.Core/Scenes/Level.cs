using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using BakeryBash.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BakeryBash
{
	public class Level : Scene
	{
		Coroutine Coroutine;
		public const int GridSize = 80;
		public const int GridRows = 12;
		public const int GridColumns = 12;
		public static RectangleF GameArea = new RectangleF(GridSize * 6, GridSize * 1, GridSize * GridColumns, GridSize * GridRows);
		public static Level Instance;
		public ScreenWipe Wipe;
		private LevelBackground LevelBackground;
		private LevelBoundaries LevelBoundaries;
		public ParticleSystem ParticlesFG;
		public ParticleSystem ParticlesBG;
		public BallQueue BallQueue;
		private DangerZone DangerZone;
		private Vector2 shakeDirection;
		private int lastDirectionalShake;
		public Vector2 ShakeVector { get; private set; }
		private float shakeTimer;
		private Vector2 cameraPreShake;
		private float flashAlpha;
		private Color flashColor = Color.White;
		private bool doFlash;
		private GameRenderer GameRenderer;
		private HUDRenderer HudRenderer;
		public Camera Camera;
		//public ScoreBoard ScoreBoard;
		public BallLauncher BallLauncher;
		public List<GridEntity> GridEntities;
		public List<PickupItem> PickupItems;
		public DangerLights DangerLight1;
		public DangerLights DangerLight2;

		public int currentRow;
		TextMenu PauseMenu;
		FadeToColor fadeIn;
		public override void Begin()
		{
			//start rendering
			base.Begin();
			GameplayBuffers.Create();
			GameManager.Instance.Score = 0;
			Add(GameRenderer = new GameRenderer());
			Camera = GameRenderer.Camera;
			Add(HudRenderer = new HUDRenderer());
			Events.EnemyDestroyed += EnemyKilled;
			Add(fadeIn = new FadeToColor(Color.Black, this, true));
			Coroutine = new Coroutine(LevelStartRoutine());
		}
		public void Flash(Color color)
		{
			this.doFlash = true;
			//this.flashAlpha = 1f;
			this.flashColor = color;
		}
		public void Shake(float time = 0.3f)
		{
			this.shakeDirection = Vector2.Zero;
			this.shakeTimer = Math.Max(this.shakeTimer, time);
		}

		public void EnemyKilled(Enemy enemy)
		{
			GameManager.Instance.ComboCounter++;
			Events.ScoreChanged(GameManager.Instance.PlayerAttributes.EnemyDeathScore * GameManager.Instance.ComboCounter);
		}

		IEnumerator LevelStartRoutine()
		{
			if (fadeIn != null)
				yield return fadeIn.Wait();
			Focused = false;
			GameManager.Instance.NextBalls.Clear();
			//yield return ScoreBoard.AnimateIn();
			BallLauncher.Show();
			yield return NextRow();
			Focused = true;
			//ready!
			GameManager.Instance.State = PlayState.Aiming;
			Paused = false;
			Coroutine = null;
			currentRow = 1;
		}

		public IEnumerator MoveRowsDown()
		{
			for (int i = 0; i < GridEntities.Count; i++)
			{
				var thisEnemy = GridEntities[i];
				if (thisEnemy.Active == false) continue;
				thisEnemy.MoveDownOneGridSpace();
			}
			yield return null;
		}

		public IEnumerator NextRow()
		{
			yield return SpawnRow();
			currentRow++;
		}

		public static PickupItem GetRandomPickup()
		{
			PickupItem pickup = null;
			switch (Choosers.PickupItems.Choose())
			{
				case PickupItem.PickupType.SpecialBall:
					pickup = new SpecialBallPickup(Choosers.Balls.Choose());
					break;
				case PickupItem.PickupType.MultiBall:
					pickup = new MultiBallPickup();
					break;
				case PickupItem.PickupType.TackShooter:
					pickup = new TackShooter();
					break;
				case PickupItem.PickupType.Missile:
					pickup = new MissileLauncherPickup();
					break;
			}
			return pickup;
		}
		public IEnumerator SpawnRow()
		{
			for (int i = 0; i < GridColumns; i++)
			{
				int depth = 0;
				GridEntity itemToAdd = null;
				int gridY = 0;
				int gridX = i;

				itemToAdd = GetRandomSpawn();
				if (itemToAdd == null) continue;
				//if (itemToAdd is TackShooter) spawnedTack = true;
				itemToAdd.Depth = depth++;
				Add(itemToAdd);
				GridEntities.Add(itemToAdd);
				itemToAdd.MoveToNewGridSpace(gridX, gridY, true);

				ParticlesFG.Emit(ParticleTypes.Cloud, 10, new Vector2(GameArea.Left + (int)gridX * GridSize + GridSize / 2, GameArea.Top + (int)gridY * GridSize + GridSize / 2), new Vector2(10));
				Add(new Cloud(new Vector2(GameArea.Left + (int)gridX * GridSize, GameArea.Top + (int)gridY * GridSize)));
			}
			yield return null;
			for (int i = 0; i < Calc.Random.Range(1, 3); i++)
			{
				var pos = GetEmptyGridSpace();
				var gridX = pos.X;
				var gridY = pos.Y;
				var item = GetRandomPickup();
				if (item == null) continue;
				Add(item);
				GridEntities.Add(item);

				ParticlesFG.Emit(ParticleTypes.Cloud, 10, new Vector2(GameArea.Left + (int)gridX * GridSize + GridSize / 2, GameArea.Top + (int)gridY * GridSize + GridSize / 2), new Vector2(10));
				Add(new Cloud(new Vector2(GameArea.Left + (int)gridX * GridSize, GameArea.Top + (int)gridY * GridSize)));

				item.MoveToNewGridSpace(pos, true);
			}
			yield return 0.01f;
		}

		public void LoadLevel()
		{
			Instance = this;
			GridEntities = new List<GridEntity>();
			PickupItems = new List<PickupItem>();
			Add(ParticlesFG = new ParticleSystem(-100000, 40));
			Add(ParticlesBG = new ParticleSystem(1000, 40));
			Add(LevelBackground = new LevelBackground());
			Add(BallLauncher = new BallLauncher(this));
			Add(DangerZone = new DangerZone());
			Add(DangerLight1 = new DangerLights() { Position = new(GameArea.X, (GridRows - 2) * GridSize) });
			Add(DangerLight2 = new DangerLights() { Position = new(GameArea.Right, (GridRows - 2) * GridSize) });
			DangerLight1.Visible = DangerLight2.Visible = false;
			//Add(ScoreBoard = new ScoreBoard());
			BallLauncher.Hide();
			LevelBoundaries = new LevelBoundaries(this, GameArea);
			Add(BallQueue = new BallQueue(new Vector2(250, 80)));
		}

		public bool CheckForGameOver() => GridEntities.Any((e => e is Enemy en && en.GetGridPosition.Y > 10));
		public IEnumerator ApplyBuffs()
		{
			var enemies = GridEntities.Where(e => e is Enemy block).ToList();

			foreach (var entity in enemies)
			{
				if (entity is Enemy enemy)
					enemy.ApplyStatusEffect();
				yield return null;
			}

			yield return 0.1f;
		}

		GridEntity GetRandomSpawn()
		{
			int ceiling = (int)Math.Floor(5 + currentRow * 0.2f);
			var res = Calc.Random.Range(0, ceiling);
			Toast e;
			//Calc.Log("Getting spawn " + res);
			if (res > 3 && res < 15) return new Toast();
			if (res > 20)
			{
				e = new Toast();
				for (int i = 0; i < Calc.Random.Range(0, 3); i++)
				{
					e.AddArmor(e.GetNextArmor());
				}
				return e;
			}
			return null;

		}

		private void GameOverRoutine()
		{
			Entity display;
			Add(display = new GameOverCard(new Vector2(Engine.Width / 2, Engine.Height / 2)));
			display.Depth = -200;
		}

		private Point GetEmptyGridSpace()
		{
			List<Point> list = new List<Point>();
			for (int y = 0; y < GridRows; y++)
				for (int x = 0; x < GridColumns; x++)
					list.Add(new(x, y));
			foreach (var ge in GridEntities)
				list.Remove(ge.GetGridPosition);

			return Calc.Random.Choose(list);
		}
		private IEnumerator EndTurnRoutine()
		{
			while (GameManager.Instance.Waiting || !BallLauncher.IsReady || GridEntities.Any(e => e is Enemy en && en.Reactions > 0)) yield return null;

			yield return MoveRowsDown();
			yield return 0.4f;
			yield return NextRow();
			yield return 0.4f;
			yield return ApplyBuffs();
			yield return 0.4f;

			Events.TurnEnded?.Invoke();

			if (CheckForGameOver())
			{
				GameManager.Instance.State = PlayState.GameOver;
				GameOverRoutine();
			}
			else
			{
				currentRow++;
				var isSpecialBall = GameManager.Instance.NextBalls.TryDequeue(out Ball.BallType result);
				GameManager.Instance.CurrentBallType = isSpecialBall ? result : Ball.BallType.Normal;
				Events.WeaponChanged.Invoke(GameManager.Instance.CurrentBallType);
				BallLauncher.Show();
				GameManager.Instance.State = PlayState.Aiming;
			}
		}


		private void StartTurn()
		{
			GameManager.Instance.ComboCounter = 1;
			GameManager.Instance.State = PlayState.Watching;
			BallLauncher.Hide();
		}
		private void EndTurn()
		{
			GameManager.Instance.State = PlayState.BetweenTurns;
			HelperEntity.Add(new Coroutine(EndTurnRoutine()));
		}
		public override void Update()
		{
			base.Update();

			if (PauseMenu != null) PauseMenu.Update();

			if (fadeIn != null)
			{
				fadeIn.Update(this);
			}
			if (GridEntities.Any(e => e is Enemy en && en.GetGridPosition.Y > 9))
			{
				DangerLight1.Visible = true;
				DangerLight2.Visible = true;
			}
			else
			{
				DangerLight1.Visible = false;
				DangerLight2.Visible = false;
			}

			HudRenderer.BackgroundFade = Calc.Approach(HudRenderer.BackgroundFade, Paused ? 1f : 0.0f, 8f * Engine.RawDeltaTime);

			if (Coroutine != null)
				Coroutine.Update();
			#region Shake
			if (shakeTimer > 0.0)
			{
				if (OnRawInterval(0.04f))
				{
					int num2 = (int)Math.Ceiling(shakeTimer * 10.0);
					if (shakeDirection == Vector2.Zero)
					{
						ShakeVector = new Vector2((float)(-num2 + Calc.Random.Next(num2 * 2 + 1)), (float)(-num2 + Calc.Random.Next(num2 * 2 + 1)));
					}
					else
					{
						if (lastDirectionalShake == 0)
							lastDirectionalShake = 1;
						else
							lastDirectionalShake *= -1;
						ShakeVector = -shakeDirection * (float)lastDirectionalShake * (float)num2;
					}
				}
				shakeTimer -= Engine.RawDeltaTime;
			}
			else
				ShakeVector = Vector2.Zero;
			#endregion
			#region Flash
			if (doFlash)
			{
				flashAlpha = 0.4f;
				doFlash = false;
			}
			else if (flashAlpha > 0.0)
				flashAlpha = Calc.Approach(flashAlpha, 0.0f, Engine.DeltaTime * 2f);
			#endregion

			if (Focused)
			{
				if (MInput.Keyboard.Pressed(Microsoft.Xna.Framework.Input.Keys.G))
					GameOverRoutine();
				//gameplay input listening goes here
				if (GameManager.Instance.State == PlayState.Aiming)
				{
					if (Input.Launch.Pressed || MInput.Touch.Tapped || (Engine.Viewport.Bounds.Contains(MInput.Mouse.Position) && MInput.Mouse.PressedLeftButton))
					{
						BallLauncher.LaunchBall();
						StartTurn();
					}
				}
				if (GameManager.Instance.State == PlayState.Watching)
				{
					if (!GameManager.Instance.Waiting) EndTurn();
				}
				if (Input.Pause.Pressed)
				{
					Paused = GameManager.Instance.Paused = !GameManager.Instance.Paused;
					Engine.TimeRate = Paused ? 0 : 1f;
					if (Paused)
					{
						BallLauncher.Active = false;
						Focused = false;
						Add(PauseMenu = Menus.CreatePauseMenu());
						PauseMenu.OnCancel = PauseMenu.OnPause = () =>
						{
							Engine.TimeRate = 1;
							Paused = GameManager.Instance.Paused = false;
							PauseMenu.Close();
							BallLauncher.Active = true;
							Focused = true;
						};
					}
					else
					{
						PauseMenu?.Close();
						BallLauncher.Active = true;
						Focused = true;
					}
				}
			}
		}

		public override void BeforeRender()
		{
			this.cameraPreShake = this.Camera.Position;
			this.Camera.Position += this.ShakeVector;
			this.Camera.Position.Floor();

			base.BeforeRender();
		}
		public override void Render()
		{
			Engine.Instance.GraphicsDevice.SetRenderTarget(GameplayBuffers.Level);
			Engine.Instance.GraphicsDevice.Clear(Color.Black);
			GameRenderer.Render(this);
			if (flashAlpha > 0.0)
			{
				Draw.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, (Effect)null);
				Draw.Rect(-1f, -1f, 1922, 1082f, flashColor * flashAlpha);
				Draw.SpriteBatch.End();
			}
			Matrix transformMatrix = Matrix.CreateScale(1f) * Engine.ScreenMatrix;


			Engine.Instance.GraphicsDevice.SetRenderTarget(null);
			Engine.Instance.GraphicsDevice.Clear(Color.Black);
			Engine.Instance.GraphicsDevice.Viewport = Engine.Viewport;
			Draw.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, transformMatrix);
			Draw.SpriteBatch.Draw((Texture2D)(RenderTarget2D)GameplayBuffers.Level, Vector2.Zero, new Rectangle?(GameplayBuffers.Level.Bounds), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
			Draw.SpriteBatch.End();

			if (fadeIn != null)
				fadeIn.Render(this);

			HudRenderer.Render(this);
		}
		public override void AfterRender()
		{
			base.AfterRender();
			this.Camera.Position = this.cameraPreShake;
		}
	}
}