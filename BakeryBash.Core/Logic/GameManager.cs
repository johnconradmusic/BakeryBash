using BakeryBash.Entities;
using Monocle;
using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryBash;

public enum PlayState
{
	Aiming, Watching, Upgrading, GameOver, BetweenTurns
}
public class GameManager
{
	private static GameManager instance;
	public int Score;
	public bool Paused;
	public static GameManager Instance
	{
		get
		{
			if (instance == null) instance = new GameManager();
			return instance;
		}
	}
	private GameManager()
	{
		if (PlayerAttributes == null) PlayerAttributes = new();
		ActiveEntities = new();
		Events.ScoreChanged += ScoreChanged;
	}

	void ScoreChanged(int delta)
	{
		Score += delta;
	}

	public PlayState State;
	private int comboCounter;
	public Ball.BallType NextBallType { get; set; } = Ball.BallType.Normal;

	public Queue<Ball.BallType> NextBalls = new();
	public AttackAttributes PlayerAttributes { get; set; }
	public bool UpgradeAvailable { get; set; }
	public Ball.BallType CurrentBallType { get; set; } = Ball.BallType.Normal;
	public int ComboCounter { get => comboCounter; set { comboCounter = value; Events.ComboIncreased?.Invoke(comboCounter); } }

	private List<string> ActiveEntities;

	public bool Waiting => ActiveEntities.Count > 0;

	public void AddWait(string id)
	{
		ActiveEntities.Add(id);
		if (id == "react")
			Calc.Log("Adding " + id + " for a total of " + ActiveEntities.Where(i => i == id).Count());
	}
	public void RemoveWait(string id)
	{
		ActiveEntities.Remove(id);
		if (id == "react")
			Calc.Log("Removing " + id + " for a total of " + ActiveEntities.Where(i => i == id).Count());
	}
}