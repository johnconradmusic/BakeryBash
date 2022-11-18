using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using BakeryBash.Scenes;
using System.Collections;

namespace BakeryBash
{
	public class ScoreBoard : Entity
	{
		MTexture panelBG;
		MTexture fill;
		MTexture starScoreIndicator;
		MTexture fillSubTexture;
		float progress = 0;
		float targetAmount = 0;
		Vector2 progressBarOffset = new(-58.5f, 121.5f);
		Vector2 s1pos, s2pos, s3pos;
		MTexture starTexture;
		bool s1earned, s2earned, s3earned;
		float star1Percentage, star2Percentage, star3Percentage, full;
		Vector2 animationOffset, animStartPos, animTargetPos;
		float animTime = 0.6f;
		public Dictionary<string, GoalIcon> Icons;
		Dictionary<string, int> localRequirements;
		public int Score;

		public int StarsEarned
		{
			get
			{
				int result = 0;
				if (s1earned) result++;
				if (s2earned) result++;
				if (s3earned) result++;
				return result;
			}
		}

		public ScoreBoard()
		{
			localRequirements = new();
			animStartPos = new Vector2(400, 0);
			animTargetPos = Vector2.Zero;
			animationOffset = animStartPos;
			star3Percentage = 0.9f;
			

			starScoreIndicator = GFX.Game["UI/star-progress-bar"];
			fill = GFX.Game["UI/star-progress-fill"];
			panelBG = GFX.Game["UI/goal-panel"];
			Position = new(Engine.Width - (panelBG.Width / 2) + 50, 100 + panelBG.Height / 2);
			Icons = new();
			
			starTexture = GFX.Game["UI/level-star"];
			fillSubTexture = fill.GetSubtexture(0, 0, (int)(fill.Width * progress), fill.Height);

		}

		public override void Update()
		{
			base.Update();
			
				progress = Calc.Approach(progress, targetAmount, 0.003f);
			if (Scene.OnInterval(.1f))
				fillSubTexture = fill.GetSubtexture(0, 0, (int)(fill.Width * progress), fill.Height);
			
			if (progress > star1Percentage && !s1earned)
			{
				s1earned = true;
				Level.Instance.ParticlesFG.Emit(ParticleTypes.PickupCollected, 20, s1pos, Vector2.Zero);
			}
			if (progress > star2Percentage && !s2earned)
			{
				s2earned = true;
				Level.Instance.ParticlesFG.Emit(ParticleTypes.PickupCollected, 20, s2pos, Vector2.Zero);
			}
			if (progress > star3Percentage && !s3earned)
			{
				s3earned = true;
				Level.Instance.ParticlesFG.Emit(ParticleTypes.PickupCollected, 20, s3pos, Vector2.Zero);
			}

			foreach (var icon in Icons)
				icon.Value.Position = Position + animationOffset;
		}

		public void Show()
		{
			Add(new Coroutine(AnimateIn()));
		}

		public IEnumerator AnimateIn()
		{
			float counter = 0;
			while (counter < animTime)
			{
				counter += Engine.DeltaTime;
				animationOffset = Vector2.Lerp(animStartPos, animTargetPos, Ease.BackOut(counter / animTime));
				yield return null;
			}
		}

		public IEnumerator AnimateOut()
		{
			float counter = 0;
			while (counter < animTime)
			{
				counter += Engine.DeltaTime;
				animationOffset = Vector2.Lerp(animTargetPos, animStartPos, Ease.BackOut(counter / animTime));
				yield return null;
			}
		}

		public void Hide()
		{
			Add(new Coroutine(AnimateOut()));
		}

		public override void Render()
		{
			var fillPos = Position - new Vector2(fill.Width / 2, 0) + progressBarOffset;
			panelBG.DrawCentered(Position + animationOffset);
			fillSubTexture.DrawJustified(fillPos + animationOffset, new(0, 0.5f));
			starScoreIndicator.DrawCentered(s1pos = fillPos + (Vector2.UnitX * fill.Width * star1Percentage) + animationOffset);
			starScoreIndicator.DrawCentered(s2pos = fillPos + (Vector2.UnitX * fill.Width * star2Percentage) + animationOffset);
			starScoreIndicator.DrawCentered(s3pos = fillPos + (Vector2.UnitX * fill.Width * star3Percentage) + animationOffset);

			if (s1earned) starTexture.DrawCentered(s1pos);
			if (s2earned) starTexture.DrawCentered(s2pos);
			if (s3earned) starTexture.DrawCentered(s3pos);


			foreach (var icon in Icons)
				icon.Value.Draw();

		}


		public class GoalIcon
		{
			MTexture Texture;
			public Vector2 Position;
			public string Text;
			Vector2 textOffset = new Vector2(80, 0);
			Vector2 localOffset;
			public string ID;
			public Vector2 GetPositionOfGoal => Position + localOffset;
			public GoalIcon(MTexture texture, string id, Vector2 offset)
			{
				ID = id;
				Texture = texture;
				localOffset = offset;
			}

			public void Draw()
			{
				Texture.DrawCentered(Position + localOffset);
				Fonts.ComicGecko.Draw(30, Text, Position + localOffset + textOffset, Vector2.One / 2, Vector2.One, Color.Black);
			}
		}
	}
}