using System;
using Microsoft.Xna.Framework;
using Monocle;



namespace BakeryBash
{
    public class NineSliceComponent : Component
    {
        int margin;
        MTexture texture;
        public float Width;
        public float Height;

        Vector2 centerSize, centerScale;

        float renderWidth, renderHeight;

        public Vector2 Justify = new Vector2(0.5f, 0.5f);
        public NineSliceComponent(int margin, MTexture texture, float width, float height) : base(true, true)
        {
            this.margin = margin;
            this.texture = texture;
            Width = width;
            Height = height;
        }

        public override void Update()
        {
            renderWidth = Math.Max(Width, margin * 2);
            renderHeight = Math.Max(Height, margin * 2);
            centerSize = new(renderWidth - margin * 2, renderHeight - margin * 2);
            centerScale = new(
                centerSize.X / (texture.Width - (margin * 2)),
                centerSize.Y / (texture.Height - (margin * 2)));

        }

        public override void Render()
        {
            var topLeft = texture.GetSubtexture(0, 0, margin, margin);
            var topCenter = texture.GetSubtexture(margin, 0, texture.Width - margin * 2, margin);
            var topRight = texture.GetSubtexture(topCenter.Width + margin, 0, margin, margin);

            var midLeft = texture.GetSubtexture(0, margin, margin, texture.Height - margin * 2);
            var middle = texture.GetSubtexture(margin, margin, texture.Width - margin * 2, texture.Height - margin * 2);
            var midRight = texture.GetSubtexture(midLeft.Width + middle.Width, margin, margin, texture.Height - margin * 2);

            var btmHeight = topLeft.Height + midLeft.Height;
            var btmLeft = texture.GetSubtexture(0, btmHeight, margin, margin);
            var btmCenter = texture.GetSubtexture(margin, btmHeight, middle.Width, margin);
            var btmRight = texture.GetSubtexture(btmCenter.Width + margin, middle.Height + margin, margin, margin);

            Vector2 pos = Entity.Position - new Vector2(renderWidth/2, renderHeight/2);
            topLeft.Draw(pos);
            pos.X += topLeft.Width;

            topCenter.Draw(pos, new Vector2(centerScale.X, 1), 0);
            pos.X += topCenter.Width*centerScale.X;

            topRight.Draw(pos);
            pos.Y += margin;
            pos.X -= topLeft.Width + topCenter.Width*centerScale.X;

            midLeft.Draw(pos, new Vector2(1, centerScale.Y), 0);
            pos.X += midLeft.Width;

            middle.Draw(pos, new Vector2(centerScale.X, centerScale.Y), 0);
            pos.X += middle.Width*centerScale.X;

            midRight.Draw(pos, new Vector2(1,centerScale.Y), 0);
            pos.Y += middle.Height*centerScale.Y;
            pos.X -= topLeft.Width + (topCenter.Width*centerScale.X);

            btmLeft.Draw(pos);
            pos.X += btmLeft.Width;

            btmCenter.Draw(pos, new Vector2(centerScale.X, 1), 0);
            pos.X += btmCenter.Width*centerScale.X;

            btmRight.Draw(pos);

        }

    }
}

