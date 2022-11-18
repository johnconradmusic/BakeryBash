using System;
using Microsoft.Xna.Framework;
using Monocle;

namespace BakeryBash
{
    public abstract class ModalPopup : Entity
    {
        string caption;
        NineSliceComponent background;
        public ModalPopup(string text, Vector2 pos)
        {
            Position = pos;
            caption = text;
            
            Add(background = new NineSliceComponent(160, GFX.Game["UI/modal-dialog"], 500, 500));
            background.Width = Fonts.ComicGecko.Get(40).Measure(caption).X;
            background.Height = Fonts.ComicGecko.Get(40).Measure(caption).Y;
        }

        public override void Update()
        {
            base.Update();
        }


        public override void Render()
        {
            base.Render();
            Fonts.ComicGecko.Draw(40, caption, Position, new Vector2(0.5f), Vector2.One, Color.Black);
        }
    }
}

