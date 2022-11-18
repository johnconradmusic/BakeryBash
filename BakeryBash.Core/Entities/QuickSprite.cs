using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Collections;

namespace BakeryBash.Entities
{
	public class QuickSprite : Entity
	{
		Sprite sprite;
		float lifespan;
		Vector2 target;
		Vector2 start;
		bool fade;
		public QuickSprite(Sprite sprite, string animId, Vector2 position, float lifespan, bool fadeOut)
		{
			Add(this.sprite = sprite);
			Position = target = start = position;
			this.lifespan = lifespan;
			fade = fadeOut;
			sprite.Play(animId);
		}

		public QuickSprite(Sprite sprite, string animId, Vector2 position, Vector2 target, float lifespan, bool fadeOut)
		{
			Add(this.sprite = sprite);
			Position = start = position;
			this.target = target;
			this.lifespan = lifespan;
			fade = fadeOut;
			sprite.Play(animId);
			
		}

		public override void Awake(Scene scene)
		{
			base.Awake(scene);
			Add(new Coroutine(Lifespan()));
		}

		IEnumerator Lifespan()
		{
			for (float t = lifespan; t > 0; t -= Engine.DeltaTime)
			{
				if (fade) sprite.Color = new Color(1f, 1f, 1f, t / lifespan);
				Position = Vector2.Lerp(target, start, t / lifespan);
				yield return null;
			}
			RemoveSelf();
		}
	}
}