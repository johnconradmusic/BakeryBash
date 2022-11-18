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
	public class Cloud : Entity
	{
		Sprite sprite;
		public Cloud(Vector2 position) 
		{
			Position = position + new Vector2(((float)Level.GridSize) / 2);

			Add(sprite = GFX.SpriteBank.Create("cloud"));
			sprite.Scale = Vector2.One * 1.5f;
			sprite.Rotation = Calc.Random.NextAngle();
			sprite.OnFinish += Finish;
		}

		void Finish(string str)
		{
			RemoveSelf();
		}
	}
}