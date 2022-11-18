using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BakeryBash.Scenes;
using Microsoft.Xna.Framework;

namespace BakeryBash.Entities;

public class LevelBoundaries
{
	Entity top, left, right;
	public LevelBoundaries(Scene scene, RectangleF gameArea)
	{

		top = new Wall(new Vector2(gameArea.Left, 0));
		top.Collider = new Hitbox(gameArea.Width, Level.GridSize);
		top.Tag = Tags.ObstaclesTag;
		
		scene.Add(top);

		left = new Wall(new Vector2(gameArea.Left - Level.GridSize, gameArea.Top));
		left.Collider = new Hitbox(Level.GridSize, gameArea.Height);
		left.Tag = Tags.ObstaclesTag;
		scene.Add(left);

		right = new Wall(new Vector2(gameArea.Right, gameArea.Top));
		right.Collider = new Hitbox(Level.GridSize, gameArea.Height);
		right.Tag = Tags.ObstaclesTag;
		scene.Add(right);
	}

}