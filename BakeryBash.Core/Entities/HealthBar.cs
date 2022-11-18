using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
namespace BakeryBash.Entities
{
	public class HealthBar : Entity
	{
		int backgroundThickness = 6;
		int fillThickness = 6;
		int backgrounwidth = 68;
		int fillWidth = 68;
		float yOffset = 40f;
		Color backgroundColor;
		Color fillColor;
		float alpha;

		float curHealth = 1;
		float fillTargetWidth;

		float counter;

		public HealthBar()
		{
			fillColor = Calc.HexToColor("00c646");
			backgroundColor = Calc.HexToColor("111111");
			Depth = -1000;
		}

		public void Update(float health)
		{
			counter = 0.4f;
			curHealth = Math.Max(0, health);
			alpha = 1;
		}

		public override void Update()
		{
			counter = Calc.Approach(counter, 0, Engine.DeltaTime);
			if (fillTargetWidth == fillWidth * curHealth)
				alpha -= Engine.DeltaTime / 1;
			//if (counter <= 0)
				fillTargetWidth = Calc.Approach(fillTargetWidth, fillWidth * curHealth, 1f);
		}

		public override void Render()
		{
			var startPos = Position + new Vector2(-fillWidth / 2, yOffset);

			Draw.Line(startPos, Position + new Vector2(backgrounwidth / 2, yOffset), new Color(backgroundColor, alpha), backgroundThickness);
			Draw.Line(startPos, startPos + new Vector2(fillTargetWidth, 0), new Color(Color.Red, alpha), fillThickness);

			Draw.Line(startPos, startPos + new Vector2(fillWidth * curHealth, 0), new Color(fillColor, alpha), fillThickness);

		}
	}
}