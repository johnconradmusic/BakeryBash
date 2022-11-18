//using Microsoft.Xna.Framework;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System;
//using Monocle;
//using BakeryBash.Scenes;
//using System.IO;
//using Microsoft.Xna.Framework.Graphics;

//namespace BakeryBash.Entities;

//public class LegacyLevel
//{
//	Scene scene;
//	LevelBackground levelBackground;
//	LevelBoundaries levelBoundaries;
//	BallQueue ballQueue;
//	UI GameUI;
//	public ScoreBoard goalPanel;
//	public List<GridEntity> gridEntities;
//	public Dictionary<string, int> progress = new();
//	int _curLevel;
//	int _curWorld;
//	LevelData LevelData;
//	LevelCompletionData LevelCompletionData;
//	int score;
//	int currentRow;

//	public Point GetEmptyGridSpace()
//	{
//		bool isValid = false;
//		Point p = default;
//		while (isValid == false)
//		{
//			p = new Point(Calc.Random.Range(0, GameScene.GridColumns), Calc.Random.Range(1, 4));
//			if (!gridEntities.Any(e => e.GetGridPosition == p)) isValid = true;
//		}
//		return p;
//	}
//	public void UpdateProgress(string id)
//	{
//		if (progress.ContainsKey(id))
//			progress[id]++;
//		else progress.Add(id, 1);

//		progressIndicator.ProgressIncrement(id);

//	}
//	public Point GetGridPositionOfEntity(Vector2 position)
//	{
//		var gridSpacePos = position - new Vector2(GameScene.GridSize / 2) - GameScene.GameArea.TopLeft;
//		return (gridSpacePos / GameScene.GridSize).ToPoint();
//		//TODO: add getter for other entities that might be on the grid.
//	}

//	public GridEntity EntityAt(int col, int row)
//	{
//		if (col < 0 || col > GameScene.GridColumns || row < 0 || row > GameScene.GridRows) throw new System.IndexOutOfRangeException();
//		return gridEntities.FirstOrDefault(e => e.GetGridPosition.X == col && e.GetGridPosition.Y == row);
//	}

//	public LegacyLevel(Scene scene, int level, int world)
//	{
//		_curLevel = level;
//		score = 0;
//		this.scene = scene;
//		gridEntities = new();

//		LevelData = WorldData.Worlds[world].Levels[level];
//		LevelCompletionData = new LevelCompletionData() { ID = level, World = world };

//		Events.ComboIncreased += ComboIncreased;
//		Events.EnemyDestroyed += EnemyKilled;
//		Events.ScoreChanged += ScoreChanged;

//		levelBoundaries = new LevelBoundaries(scene, GameScene.GameArea);

//		//scene.Add(new Dangerzone());
//		//scene.Add(ballQueue = new BallQueue(new Vector2(GameScene.GameArea.Left - 35, 600)));
//		scene.Add(levelBackground = new LevelBackground());
//		scene.Add(goalPanel = new ScoreBoard(LevelData));

//		BuildUI();
//		goalPanel.Show();
//	}

//	void BuildUI()
//	{
//		scene.Add(GameUI = new UI(new(200, 500), null));
//	}

//	List<Enemy> enemiesToUpdate = new List<Enemy>();

//	public void ScoreChanged(int amount)
//	{
//		score += amount;
//		goalPanel.SetStarProgressFloat(score);
//	}

//	public void EnemyKilled(Enemy enemy)
//	{
//		Events.ScoreChanged?.Invoke(30);
//		enemiesToUpdate.Add(enemy);
//	}


//	public bool CheckRequirements()
//	{
//		bool allRequirementsMet = LevelCompletionData.HasMetRequirements;
//		if (allRequirementsMet)
//		{
//			SaveData.Instance.RegisterCompletion(_curLevel);
//			UserIO.SaveHandler(true, false);
//		}
//		return allRequirementsMet;
//	}

//	public void ComboIncreased(int combo)
//	{
//		//if (combo % 5 == 0 && combo != 0)
//		//{
//		//	var pickup = GetRandomPickup();
//		//	gridEntities.Add(pickup);
//		//	scene.Add(pickup);
//		//	var grid = GetEmptyGridSpace();
//		//	pickup.MoveToNewGridSpace(grid.X, grid.Y, true);
//		//	scene.Add(new Cloud(new Vector2(GameScene.GameArea.Left + grid.X * GameScene.GridSize, GameScene.GameArea.Top + grid.Y * GameScene.GridSize)));

//		//}
//	}

//	public IEnumerator NextRow()
//	{
//		if (LevelData.Rows.Count <= currentRow) yield return SpawnRow("x,x,x,x,x,x,x,x,x,x,x,x"); //HACK
//		else yield return SpawnRow(LevelData.Rows[currentRow++]);
//	}

//	PickupItem GetRandomPickup()
//	{
//		PickupItem pickup = null;
//		switch (Choosers.PickupItems.Choose())
//		{
//			case PickupItem.PickupType.SpecialBall:
//				pickup = new SpecialBallPickup();
//				break;
//			case PickupItem.PickupType.MultiBall:
//				pickup = new MultiBallPickup();
//				break;
//			case PickupItem.PickupType.TackShooter:
//				pickup = new TackShooter();
//				break;
//			case PickupItem.PickupType.Missile:
//				pickup = new MissileLauncherPickup();
//				break;
//		}
//		return pickup;
//	}

//	public void LoadLevel(int id)
//	{
//		LevelData = WorldData.Worlds[0].Levels[id];
//		currentRow = 0;
//	}
//	public IEnumerator SpawnRow(string rowContent)
//	{
//		var items = rowContent.Split(',');
//		int depth = 0;
//		for (int i = 0; i < items.Length; i++)
//		{
//			GridEntity itemToAdd = null;
//			int gridY = 0;
//			int gridX = i;
//			var item = items[i].Trim();
//			if (item == "x") continue;
//			if (item == "e1")//toast
//				scene.Add(itemToAdd = new Toast(gridX, gridY, scene, 6));
//			if (item == "e2")//toast warrior
//				scene.Add(itemToAdd = new ToastWarrior(gridX, gridY, scene, 6));
//			if (item == "b1")
//				scene.Add(itemToAdd = new MegaToast(100, gridX, gridY, scene));
//			if (item == "p")
//			{ //pickups
//				GridEntity entity = itemToAdd = GetRandomPickup();
//				//scene.Add(itemToAdd = new MissileLauncherPickup());
//				if (entity != null) scene.Add(entity);
//			}

//			if (itemToAdd == null) continue;
//			itemToAdd.Depth = depth++;

//			gridEntities.Add(itemToAdd);
//			itemToAdd.MoveToNewGridSpace(gridX, gridY, true);

//			GameScene.Instance.ParticlesFG.Emit(ParticleTypes.Cloud, 10, new Vector2(GameScene.GameArea.Left + (int)gridX * GameScene.GridSize + GameScene.GridSize / 2, GameScene.GameArea.Top + (int)gridY * GameScene.GridSize + GameScene.GridSize / 2), new Vector2(0));
//			scene.Add(new Cloud(new Vector2(GameScene.GameArea.Left + (int)gridX * GameScene.GridSize, GameScene.GameArea.Top + (int)gridY * GameScene.GridSize)));
//			yield return 0.02f;
//		}
//	}

//	private void GameOverRoutine()
//	{
//		Entity display;
//		scene.Add(display = new GameOverCard(new Vector2(Engine.Width / 2, Engine.Height / 2)));
//		display.Depth = -200;
//		Entity darken = new Entity();
//		var img = new Image(Draw.Pixel);
//		img.Scale.X = Engine.Width;
//		img.Scale.Y = Engine.Height;
//		img.Color = new Color(.1f, .1f, .2f, .5f);
//		darken.Add(img);
//		darken.Depth = -100;
//		scene.Add(darken);

//	}
//	public bool CheckForGameOver()
//	{
//		foreach (var item in gridEntities)
//		{
//			if (item is Enemy enemy && enemy.Position.Y / GameScene.GridSize > 12)
//			{
//				GameManager.Instance.State = PlayState.GameOver;
//				GameOverRoutine();
//				return true;
//			}
//		}
//		return false;
//	}
//	public IEnumerator ApplyBuffs()
//	{
//		var enemies = gridEntities.Where(e => e is Enemy block).ToList();

//		foreach (var entity in enemies)
//		{
//			if (entity is Enemy enemy)
//				enemy.ApplyStatusEffect();
//			yield return null;
//		}

//		yield return 0.1f;
//	}

//	public IEnumerator MoveRowsDown()
//	{
//		for (int i = 0; i < gridEntities.Count; i++)
//		{
//			var thisEnemy = gridEntities[i];
//			if (thisEnemy.Active == false) continue;
//			thisEnemy.MoveDownOneGridSpace();
//			yield return null;
//		}
//	}

//	float flyingIndicatorsCounter = 0;
//	float flyingWaitTime = 0.1f;

//	public void Update()
//	{
//		if (enemiesToUpdate.Count > 0)
//		{
//			if (flyingIndicatorsCounter < flyingWaitTime)
//				flyingIndicatorsCounter += Engine.DeltaTime;
//			else
//			{
//				flyingIndicatorsCounter = 0;
//				var e = enemiesToUpdate.First();
//				GameScene.Instance.Add(new FlyingProgressIndicator(e.ID, e.Position));
//				enemiesToUpdate.Remove(e);
//			}
//		}

//		if (CheckRequirements())
//		{
//			GameManager.Instance.State = PlayState.BetweenTurns;

//		}

//		if (MInput.Keyboard.Pressed(Microsoft.Xna.Framework.Input.Keys.T))
//		{
//			scene.Add(new Wire(new Vector2(Engine.Width / 2, Engine.Height / 2), MInput.Mouse.Position, true));
//		}

//		if (MInput.Keyboard.Pressed(Microsoft.Xna.Framework.Input.Keys.R))
//		{
//			scene.HelperEntity.Add(new Coroutine(NextRow()));
//			scene.HelperEntity.Add(new Coroutine(MoveRowsDown()));
//		}


//	}
//}
