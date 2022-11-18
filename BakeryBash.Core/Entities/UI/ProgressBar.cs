using System;
using Microsoft.Xna.Framework;
using Monocle;
namespace BakeryBash
{
    public class ProgressBar : UIElement
    {
        MTexture background;
        MTexture fill;
        float progress;
        public ProgressBar(Vector2 position, MTexture background, MTexture fill) : base(position)
        {
            this.background = background;
            this.fill = fill;
            Position = position;
        }

        public override float Height => throw new NotImplementedException();

        public override float Width => throw new NotImplementedException();

        public override void Render()
        {
            background.DrawCentered(Position);
            fill.GetSubtexture(0, 0, (int)(fill.Width * progress), fill.Height).DrawJustified(Position - new Vector2(fill.Width/2,0), new(0,0.5f));
        }

        public void SetFloat(float amt)
        {
            progress = amt;
        }

        public override void Update()
        {
        }
    }
}

