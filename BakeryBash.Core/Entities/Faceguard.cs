using Microsoft.Xna.Framework;
using Monocle;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryBash.Entities
{
	public class Faceguard : EnemyArmor
	{
		public Faceguard(Enemy enemy) : base(enemy: enemy, depth: -20, offset: new(0, 20), health: 10)
		{
			Add(Sprite = GFX.SpriteBank.Create("faceguard"));
			Sprite.SetAnimationFrame(0);
		}

		public override void Destroy()
		{
			Sprite.RemoveSelf();
			{
				Vector2 dir = new(Calc.Random.MinusOneToOne() - 0.5f, -Calc.Random.NextFloat(1));
				dir *= 500f;
				Image img = new Image(GFX.Game["Particles/toast-faceguard-particle0"]);
				Debris.Spawn(img, Position, dir);
			}
			{
				Vector2 dir = new(Calc.Random.MinusOneToOne() + 0.5f, -Calc.Random.NextFloat(1));
				dir *= 500f;
				Image img = new Image(GFX.Game["Particles/toast-faceguard-particle1"]);
				Debris.Spawn(img, Position, dir);
			}
			RemoveSelf();
		}

		public override void Hit(int amount)
		{
			base.Hit(amount);
			var percentDead = 1 - ((float)health / maxHealth);
			//Sprite.Position = -Vector2.UnitY * 20f;
			//need percent dead, then we can play the correct frame (percent dead * current anim framecount)

			Sprite.SetAnimationFrame((int)Math.Floor(percentDead * Sprite.CurrentAnimationTotalFrames));

		}

	}
}