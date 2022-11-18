using BakeryBash.Entities;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryBash
{
	public class Events
	{
		public delegate void LevelCompletedDelegate(int level, int stars, int score);
		public static LevelCompletedDelegate LevelCompleted;

		public delegate void ScoreChangedDelegate(int delta);
		public static ScoreChangedDelegate ScoreChanged;

		public delegate void EnemyDestroyedDelegate(Enemy enemy);
		public static EnemyDestroyedDelegate EnemyDestroyed;

		public delegate void WeaponChangedDelegate(Ball.BallType type);
		public static WeaponChangedDelegate WeaponChanged;

		public delegate void ComboIncreasedDelegate(int combo);
		public static ComboIncreasedDelegate ComboIncreased;

		public delegate void TurnEndedDelegate();
		public static TurnEndedDelegate TurnEnded;
	}
}