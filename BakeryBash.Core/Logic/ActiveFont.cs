using Microsoft.Xna.Framework;
using Monocle;

namespace BakeryBash
{
	public static class ActiveFont
	{
		public static PixelFont Font => Fonts.ComicGecko;

		public static PixelFontSize FontSize => ActiveFont.Font.Get(ActiveFont.BaseSize);

		public static float BaseSize => 24;

		public static float LineHeight => Font.Get(BaseSize).LineHeight;

		public static Vector2 Measure(char text) => ActiveFont.Font.Get(ActiveFont.BaseSize).Measure(text);

		public static Vector2 Measure(string text) => ActiveFont.Font.Get(ActiveFont.BaseSize).Measure(text);

		public static float WidthToNextLine(string text, int start) => ActiveFont.Font.Get(ActiveFont.BaseSize).WidthToNextLine(text, start);

		public static float HeightOf(string text) => ActiveFont.Font.Get(ActiveFont.BaseSize).HeightOf(text);

		public static void Draw(
		  char character,
		  Vector2 position,
		  Vector2 justify,
		  Vector2 scale,
		  Color color)
		{
			ActiveFont.Font.Draw(ActiveFont.BaseSize, character, position, justify, scale, color);
		}

		private static void Draw(
			float baseSize,
		  string text,
		  Vector2 position,
		  Vector2 justify,
		  Vector2 scale,
		  Color color,
		  float edgeDepth,
		  Color edgeColor,
		  float stroke,
		  Color strokeColor)
		{
			ActiveFont.Font.Draw(baseSize, text, position, justify, scale, color, edgeDepth, edgeColor, stroke, strokeColor);
		}

		private static void Draw(
		  string text,
		  Vector2 position,
		  Vector2 justify,
		  Vector2 scale,
		  Color color,
		  float edgeDepth,
		  Color edgeColor,
		  float stroke,
		  Color strokeColor)
		{
			ActiveFont.Font.Draw(ActiveFont.BaseSize, text, position, justify, scale, color, edgeDepth, edgeColor, stroke, strokeColor);
		}

		public static void Draw(string text, Vector2 position, Color color) => ActiveFont.Draw(text, position, Vector2.Zero, Vector2.One, color, 0.0f, Color.Transparent, 0.0f, Color.Transparent);

		public static void Draw(
		  string text,
		  Vector2 position,
		  Vector2 justify,
		  Vector2 scale,
		  Color color)
		{
			ActiveFont.Draw(text, position, justify, scale, color, 0.0f, Color.Transparent, 0.0f, Color.Transparent);
		}

		public static void DrawOutline(
		  string text,
		  Vector2 position,
		  Vector2 justify,
		  Vector2 scale,
		  Color color,
		  float stroke,
		  Color strokeColor)
		{
			ActiveFont.Draw(text, position, justify, scale, color, 0.0f, Color.Transparent, stroke, strokeColor);
		}
		public static void DrawOutline(
			float baseSize,
  string text,
  Vector2 position,
  Vector2 justify,
  Vector2 scale,
  Color color,
  float stroke,
  Color strokeColor)
		{
			ActiveFont.Draw(baseSize, text, position, justify, scale, color, 0.0f, Color.Transparent, stroke, strokeColor);
		}

		public static void DrawEdgeOutline(
		  string text,
		  Vector2 position,
		  Vector2 justify,
		  Vector2 scale,
		  Color color,
		  float edgeDepth,
		  Color edgeColor,
		  float stroke = 0.0f,
		  Color strokeColor = default(Color))
		{
			ActiveFont.Draw(text, position, justify, scale, color, edgeDepth, edgeColor, stroke, strokeColor);
		}
	}
}