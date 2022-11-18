using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BakeryBash.Entities
{
	public class BallQueue : Entity
	{
		MTexture texture;

		Sprite ballSprite;
		public BallQueue(Vector2 position) : base(position)
		{
			texture = GFX.Game["UI/score-panel"];
			Events.WeaponChanged += RefreshBallQueue;

			ballSprite = GFX.SpriteBank.Create("ball");
			Add(ballSprite);
			ballSprite.Scale = new(1.8f);
			ballSprite.Position = new(-162, 0);
			Depth = -10000;
			Tag = Tags.NoneTag;
		}

		void RefreshBallQueue(Ball.BallType type)
		{
		}

		void HandleBalls()
		{
			var res = GameManager.Instance.NextBalls.TryPeek(out Ball.BallType upcoming);
			if (!res)
				upcoming = Ball.BallType.Normal;
			switch (upcoming)
			{
				case Ball.BallType.Normal:
					ballSprite.Play("normal");
					break;
				case Ball.BallType.Shock:
					ballSprite.Play("shock");
					break;
				case Ball.BallType.Bomb:
					ballSprite.Play("bomb");
					break;
				case Ball.BallType.Poison:
					ballSprite.Play("poison");
					break;
				case Ball.BallType.Multiball:
					ballSprite.Play("multiball-loop");
					break;
			}

		}

		public override void Update()
		{
			base.Update();
			HandleBalls();
		}

		public override void Render()
		{
			texture.DrawCentered(Position);
			base.Render();
			Fonts.ComicGecko.DrawOutline(18, "next up " , Position - new Vector2(240,-40), new Vector2(0, 0.5f), Vector2.One, Color.White, 2, Color.Black);

			Fonts.ComicGecko.Draw(40, "score: " + GameManager.Instance.Score.ToString(), Position - new Vector2(90, 0), new Vector2(0, 0.5f), Vector2.One, Color.White) ;

		}
	}
}