using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryBash
{
	public static class Tags
	{
		public static BitTag BallsTag;
		public static BitTag ObstaclesTag;
		public static BitTag PickupsTag;
		public static BitTag NoneTag;
		public static BitTag PauseUpdate;
		public static BitTag HUD;
		public static void InitializeBitTags()
		{
			BallsTag = new BitTag("balls");
			ObstaclesTag = new BitTag("obstacles");
			PickupsTag = new BitTag("pickups");
			NoneTag = new BitTag("none");
			PauseUpdate = new BitTag("PauseUpdate");
			HUD = new BitTag("HUD");
		}
	}
}