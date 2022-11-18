using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BakeryBash;

public struct RectangleF
{
	public override string ToString()
	{
		return $"Rect: {X}, {Y}, {Width}, {Height}";
	}

	public float X, Y;
	public float Width, Height;

	public float Top => Y;
	public float Right => X + Width;
	public float Bottom => Y + Height;
	public float Left => X;

	public bool Contains(float x, float y) => x > X && x < (x + Width) && y > Y && y < (Y + Height);

	public Vector2 TopLeft => new(Left, Top);
	public Vector2 TopRight => new(Right, Top);
	public Vector2 BottomRight => new(Right, Bottom);
	public Vector2 BottomLeft => new(Left, Bottom);

	public RectangleF(float x, float y, float width, float height)
	{
		X = x;
		Y = y;
		Width = width;
		Height = height;
	}
}