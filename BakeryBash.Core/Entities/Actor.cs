using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using Monocle;
namespace BakeryBash.Entities
{
	public class Actor : Entity
	{
		private Vector2 movementCounter;
		public Vector2 ExactPosition => this.Position + this.movementCounter;

		public Vector2 PositionRemainder => this.movementCounter;

		protected BitTag CollidesWithTag;

		public void ZeroRemainderX() => this.movementCounter.X = 0.0f;

		public void ZeroRemainderY() => this.movementCounter.Y = 0.0f;
		public bool MoveH(float moveH, Collision onCollide = null)
		{
			this.movementCounter.X += moveH;
			int moveH1 = (int)Math.Round((double)this.movementCounter.X, MidpointRounding.ToEven);
			if (moveH1 == 0)
				return false;
			this.movementCounter.X -= (float)moveH1;
			return this.MoveHExact(moveH1, onCollide);
		}

		public bool MoveV(float moveV, Collision onCollide = null)
		{
			this.movementCounter.Y += moveV;
			int moveV1 = (int)Math.Round((double)this.movementCounter.Y, MidpointRounding.ToEven);
			if (moveV1 == 0)
				return false;
			this.movementCounter.Y -= (float)moveV1;
			return this.MoveVExact(moveV1, onCollide);
		}

		public bool MoveHExact(float moveH, Collision onCollide = null)
		{
			Vector2 vector2 = this.Position + Vector2.UnitX * (float)moveH;
			int num1 = Math.Sign(moveH);
			int num2 = 0;
			while (moveH != 0)
			{
				Entity solid = this.CollideFirst(CollidesWithTag, this.Position + Vector2.UnitX * (float)num1);
				if (solid != null)
				{
					this.movementCounter.X = 0.0f;

					if (onCollide != null)
						onCollide(new CollisionData()
						{
							Direction = Vector2.UnitX * (float)num1,
							Moved = Vector2.UnitX * (float)num2,
							Remaining = Vector2.UnitX * moveH,
							TargetPosition = vector2,
							Other = solid
						});
					return true;
				}
				num2 += num1;
				moveH -= num1;
				this.X += (float)num1;
			}
			return false;
		}

		public bool MoveVExact(float moveV, Collision onCollide = null)
		{
			Vector2 vector2 = this.Position + Vector2.UnitY * (float)moveV;
			int num1 = Math.Sign(moveV);
			int num2 = 0;
			while (moveV != 0)
			{

				Entity solid = this.CollideFirst(CollidesWithTag, Position + Vector2.UnitY * (float)num1);
				if (solid != null)
				{
					this.movementCounter.Y = 0.0f;

					if (onCollide != null)
						onCollide(new CollisionData()
						{
							Direction = Vector2.UnitY * (float)num1,
							Moved = Vector2.UnitY * (float)num2,
							Remaining = Vector2.UnitY * moveV,

							TargetPosition = vector2,
							Other = solid
						});
					return true;
				}

				num2 += num1;
				moveV -= num1;
				this.Y += (float)num1;
			}
			return false;
		}

		public void MoveTowardsX(float targetX, float maxAmount, Collision onCollide = null) => this.MoveToX(Calc.Approach(this.ExactPosition.X, targetX, maxAmount), onCollide);

		public void MoveTowardsY(float targetY, float maxAmount, Collision onCollide = null) => this.MoveToY(Calc.Approach(this.ExactPosition.Y, targetY, maxAmount), onCollide);

		public void MoveToX(float toX, Collision onCollide = null) => this.MoveH(toX - this.ExactPosition.X, onCollide);

		public void MoveToY(float toY, Collision onCollide = null) => this.MoveV(toY - this.ExactPosition.Y, onCollide);

		public void NaiveMove(Vector2 amount)
		{
			this.movementCounter += amount;
			int x = (int)Math.Round((double)this.movementCounter.X);
			int y = (int)Math.Round((double)this.movementCounter.Y);
			this.Position = this.Position + new Vector2((float)x, (float)y);
			this.movementCounter -= new Vector2((float)x, (float)y);
		}

	}
}

