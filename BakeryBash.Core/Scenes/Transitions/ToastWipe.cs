using Monocle;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryBash
{
	public class ToastWipe : ImageMaskTransition
	{
		public ToastWipe(Scene scene, bool wipeIn, Action onComplete = null) : base(GFX.Game["Effects/toast-mask"], scene, wipeIn, onComplete)
		{
			Duration = 1f;
		}
	}
}