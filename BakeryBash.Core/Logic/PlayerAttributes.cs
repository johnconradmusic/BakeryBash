using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryBash;
public class AttackAttributes
{

	public int EnemyDeathScore;
	public int PickupCollectedScore;

	public int MissileDamage;

	public int BallDamage;

	public int LightningDamage;
	public int LightningRange;

	public int PoisonDamage;
	public int PoisonRange;

	public int MultiBallDamage;
	public int MultiBallCount;

	public int BombDamage;
	public int BombRange;
	public AttackAttributes()
	{
		EnemyDeathScore = 10;
		PickupCollectedScore = 5;
		BallDamage = 1;
		LightningDamage = 3;
		PoisonDamage = 1;
		MultiBallDamage = 1;
		LightningRange = 2;
		PoisonRange = 2;
		MultiBallCount = 20;
		BombDamage = 4;
		BombRange = 2;
		MissileDamage = 8;
	}
}


