using Microsoft.Xna.Framework;
using Monocle;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryBash.Entities
{
	public class Shield : EnemyArmor
	{
		public Shield(Enemy enemy) : base(enemy: enemy, depth: -22, offset: new(18, 30), health: 6)
		{
			Add(Sprite = GFX.SpriteBank.Create("shield"));
			Sprite.SetAnimationFrame(0);
		}

		public override void Destroy()
		{
			Sprite.RemoveSelf();
			for (int i = 0; i < 3; i++)
			{
				Vector2 dir = new(Calc.Random.MinusOneToOne() , -Calc.Random.NextFloat(1));
				dir *= 500f;
				Image img = new Image(GFX.Game["Particles/toast-shield-particle"]);
				Debris.Spawn(img, Position, dir);
			}
			RemoveSelf();
		}

		public override void Hit(int amount)
		{
			base.Hit(amount);
			var percentDead = 1 - ((float)health / maxHealth);

			//need percent dead, then we can play the correct frame (percent dead * current anim framecount)

			Sprite.SetAnimationFrame((int)Math.Floor(percentDead * Sprite.CurrentAnimationTotalFrames));

		}

	}
}