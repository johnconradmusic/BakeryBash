using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BakeryBash.Entities
{
	public class Explosion : Entity
	{
		Sprite sprite;

		public Explosion(Vector2 position) : base(position)
		{
            GameManager.Instance.AddWait("explosion");
            Add(sprite = GFX.SpriteBank.Create("explosion"));
			sprite.OnFinish += OnFinishedPlaying;
			sprite.Scale = Vector2.One * 2f;
			Depth = -20000;
			Level.Instance.Flash(Color.White);
			MInput.Touch.Vibrate(2);

		}

		void OnFinishedPlaying(string str)
		{
            GameManager.Instance.RemoveWait("explosion");
            RemoveSelf();
		}
	}


}