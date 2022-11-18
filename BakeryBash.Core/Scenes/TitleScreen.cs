using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Collections;
using Microsoft.Xna.Framework.Input;

namespace BakeryBash.Scenes
{
	public class TitleScreen : Scene
	{
		Image logo;
		Image bg;
		TextMenu titleMenu;
		Entity logoEntity;
		Entity background;

		SineWave sine;

		public override void Begin()
		{
			base.Begin();
			Add(background = new Entity(new Vector2(Engine.Width / 2, Engine.Height / 2)));

			background.Add(bg = new Image(GFX.Game["Backgrounds/title-screen-background"]));
			bg.CenterOrigin();


			Add(logoEntity = new(new(1400, 300)));
			logoEntity.Add(logo = new Image(GFX.Game["Backgrounds/title-screen-logo"]));
			logo.CenterOrigin();
			logoEntity.Add(sine = new(.2f));

			Add(titleMenu = Menus.CreateTitleMenu());
			titleMenu.Focused = true;

			EverythingRenderer everythingRenderer = new EverythingRenderer();
			Add(everythingRenderer);
			FadeToColor fade;
			Add(fade = new FadeToColor(Color.Black, this, true));
			fade.Duration = 2;
		}
		public override void Update()
		{
			base.Update();
			logo.Scale = new((sine.Value / 50) + 1);			
		}
	}
}