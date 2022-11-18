using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using BakeryBash.Scenes;

namespace BakeryBash.Entities
{
	[Tracked(true)]
	public abstract class GridEntity : Entity
	{
		protected Vector2 target;
		int _gridX, _gridY;
		public Sprite sprite;
		public Point GetGridPosition => new Point(_gridX, _gridY);
		public Vector2 GetScreenPosition(int x, int y) => new Vector2(Level.GameArea.Left + (x * Level.GridSize) + Level.GridSize / 2, (y * Level.GridSize) + (Level.GridSize / 2) + Level.GameArea.Top);
		public GridEntity(int gridX, int gridY)
		{
			_gridX = gridX;
			_gridY = gridY;
			Position = new Vector2(Level.GameArea.Left + Level.GridSize * gridX, (Level.GridSize * gridY) + Level.GameArea.Top);
			Position += Vector2.One * Level.GridSize / 2;
			target = Position;
		}
		public GridEntity()
		{
			Position = new Vector2(Level.GridSize, (Level.GridSize) + Level.GameArea.Top);
			Position += Vector2.One * Level.GridSize / 2;
			target = Position;
		}
		public void MoveToNewGridSpace(Point point, bool immediately = false) => MoveToNewGridSpace(point.X, point.Y, immediately);
		public void MoveToNewGridSpace(int x, int y, bool immediately = false)
		{
			_gridX = x;
			_gridY = y;
			Moving = true;
			if (immediately)
				Position = target = new Vector2(Level.GameArea.Left + (x * Level.GridSize) + Level.GridSize / 2, (y * Level.GridSize) + (Level.GridSize / 2) + Level.GameArea.Top);
			else
				target = new Vector2(Level.GameArea.Left + (x * Level.GridSize) + Level.GridSize / 2, (y * Level.GridSize) + (Level.GridSize / 2) + Level.GameArea.Top);
			
		}

		public void MoveDownOneGridSpace()
		{
			sprite.Scale = new Vector2(0.5f, 1.5f);
			MoveToNewGridSpace(_gridX, _gridY + 1);
			if(_gridY+1 > Level.GridRows)
			{
				Level.Instance.GridEntities.Remove(this);
				if (this is PickupItem p) Level.Instance.PickupItems.Remove(p);
				RemoveSelf();
			}
		}

		protected bool Moving;
		public override void Update()
		{
			base.Update();
			sprite.Scale = Calc.Approach(sprite.Scale, Vector2.One, 5 * Engine.DeltaTime);

			if (Moving && Vector2.DistanceSquared(target, Position) < 0.5f)
			{
				Moving = false;
				Position = target;
				return;
			}
			if (Moving)
			{
				Position = Calc.Approach(Position, target, 1500 * Engine.DeltaTime);
			}
		}
	}
}