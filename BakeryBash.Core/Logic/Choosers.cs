using System;
using BakeryBash.Entities;
using Monocle;

namespace BakeryBash
{
	public static class Choosers
	{
		public static Chooser<PickupItem.PickupType> PickupItems;
		public static Chooser<Ball.BallType> Balls;

		public static void Load()
		{
			PickupItems = new Chooser<PickupItem.PickupType>();
			PickupItems.Add(PickupItem.PickupType.TackShooter, 1);
			PickupItems.Add(PickupItem.PickupType.MultiBall, 1);
			PickupItems.Add(PickupItem.PickupType.SpecialBall, 4);
			PickupItems.Add(PickupItem.PickupType.Missile, 2);

			Balls = new Chooser<Ball.BallType>();
			Balls.Add(Ball.BallType.Multiball, 1);
			Balls.Add(Ball.BallType.Poison, 1);
			Balls.Add(Ball.BallType.Bomb, 1);
			Balls.Add(Ball.BallType.Shock, 1);

		}
	}
}

