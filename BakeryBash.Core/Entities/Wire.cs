using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BakeryBash
{
    public class Wire : Entity
    {
        public Color Color = Calc.HexToColor("ffffff");
        public SimpleCurve Curve;
        private float sineX;
        private float sineY;
        Random random;

        public Wire(Vector2 from, Vector2 to, bool above)
        {
            this.Curve = new SimpleCurve(from, to, Vector2.Zero);
            this.Depth = above ? -8500 : 2000;
            random = new Random((int)Math.Min(from.X, to.X));
            this.sineX = random.NextFloat(4f);
            this.sineY = random.NextFloat(4f);
        }
        float counter;
        public override void Render()
        {
            counter += Engine.DeltaTime;
            this.sineX = random.NextFloat(4f);
            this.sineY = random.NextFloat(4f);
            this.Curve.Control =
                (this.Curve.Begin + this.Curve.End) / 2f + new Vector2(0.0f, 240f) + new Vector2((float)Math.Sin(sineX*counter), (float)Math.Sin((double)this.sineY*counter));
            Vector2 start = this.Curve.Begin;
            for (int index = 1; index <= 16; ++index)
            {
                Vector2 point = this.Curve.GetPoint((float)index / 16f);
                Draw.Line(start, point, this.Color, 2);
                start = point;
            }
        }
    }
}