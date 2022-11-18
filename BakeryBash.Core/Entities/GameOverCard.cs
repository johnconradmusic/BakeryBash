using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using BakeryBash.Scenes;

namespace BakeryBash
{
	public class GameOverCard : Entity
	{
		Image image;
		int scoreCounter = 0;
		float endY;
		string scoreString = "";
		string highScoreString = "N/A";

		public GameOverCard(Vector2 position) : base()
		{
			Position = position;
			Add(image = new Image(GFX.Game["UI/game-over-window"]));
			image.CenterOrigin();
			endY = Y;
			Y = Engine.ViewHeight;
			Add(new Coroutine(Presentation()));


		}


		IEnumerator Presentation()
		{
			float counter = 0;
			while (counter < 1)
			{
				counter += Engine.DeltaTime;
				Y = Calc.LerpClamp(Y, endY, Ease.BounceOut(counter));
				yield return null;
			}
			yield return 0.4f;
			while (scoreCounter < GameManager.Instance.Score)
			{
				scoreCounter += 59;
				scoreString = scoreCounter.ToString();
				yield return null;
			}
			scoreCounter = GameManager.Instance.Score;
			scoreString = scoreCounter.ToString();

			Scene.Add(new TapHotSpot(1030, 710, 80, 80) { OnTap = () => Scene.Add(new ToastWipe(Engine.Scene, false, () => Engine.Scene = new LevelLoader())) });
			Scene.Add(new TapHotSpot(890, 710, 80, 80) { OnTap = () => Scene.Add(new FadeToColor(Color.Black, Engine.Scene, false, () => Engine.Scene = new TitleScreen())) });

		}

		public override void Update()
		{
			//if (MInput.Touch.Tapped)
			//{
			//	var pos = MInput.Touch.ScreenPosition;
			//	if (pos.Y < Position.Y) return;
			//	if (pos.X < Position.X) Scene.Add(new FadeToColor(Color.Black, Engine.Scene, false, () => Engine.Scene = new TitleScreen()));
			//	if (pos.X > Position.X) Scene.Add(new ToastWipe(Engine.Scene, false, () => Engine.Scene = new LevelLoader()));
			//}
			base.Update();
		}

		public override void Render()
		{
			base.Render();
			Fonts.ComicGecko.Draw(40, scoreString, new Vector2(X + 40, Y - 43), new Vector2(0, 0.5f), Vector2.One, Color.Black);
			Fonts.ComicGecko.Draw(40, highScoreString, new Vector2(X + 40, Y + 36), new Vector2(0, 0.5f), Vector2.One, Color.Black);
		}

	}
}