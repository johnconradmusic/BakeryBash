using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryBash
{
	public class EnemyStats
	{
		public int Health;

		public static EnemyStats Toast => new EnemyStats() { Health = 10 };
	}


}