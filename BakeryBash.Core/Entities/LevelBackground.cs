using BakeryBash.Scenes;
using Microsoft.Xna.Framework;
using Monocle;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryBash.Entities
{
	public class LevelBackground : Entity
	{
		Image background;
		Image fireGlow;

		Vector2 fireplacePosition = new Vector2(180, 327);
		public override void Awake(Scene scene)
		{
			base.Awake(scene);
			Depth = 10000;
			Add(background = new Image(GFX.Game["Backgrounds/kitchen-background"]));
			Add(fireGlow = new Image(GFX.Game["Backgrounds/kitchen-background-oven"]));
			flickerTime = Calc.Random.Range(minFlickerTime, maxFlickerTime);
			targetOpacity = Calc.Random.Range(fireMinOpacity, fireMaxOpacity);

		}

		Color glowColor;

		float fireMinOpacity = 0.4f;
		float fireMaxOpacity = 0.8f;

		float previousOpacity;
		float targetOpacity;

		float minFlickerTime = 0.2f;
		float maxFlickerTime = 1f;

		float flickerTime;
		float counter;

		public override void Update()
		{
			base.Update();
			counter += Engine.DeltaTime;
			if (counter >= flickerTime)
			{
				SceneAs<Level>()?.ParticlesFG.Emit(ParticleTypes.Embers, 1, fireplacePosition, new Vector2(20, 20));
				counter = 0;
				flickerTime = Calc.Random.Range(minFlickerTime, maxFlickerTime);
				previousOpacity = targetOpacity;
				targetOpacity = Calc.Random.Range(fireMinOpacity, fireMaxOpacity);
			}
			glowColor = new Color(1, 1, 1, MathHelper.Lerp(previousOpacity, targetOpacity, counter / flickerTime));

			fireGlow.Color = glowColor;

		}
	}
}