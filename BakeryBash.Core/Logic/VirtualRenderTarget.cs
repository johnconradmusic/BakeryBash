using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocle
{
    public class VirtualRenderTarget : VirtualAsset
    {
        public RenderTarget2D Target;

        public int MultiSampleCount;

        public Rectangle Bounds
        {
            get
            {
                return this.Target.Bounds;
            }
        }

        public bool Depth
        {
            get;
            private set;
        }

        public bool IsDisposed
        {
            get
            {
                if (this.Target == null || this.Target.IsDisposed)
                {
                    return true;
                }
                return this.Target.GraphicsDevice.IsDisposed;
            }
        }

        public bool Preserve
        {
            get;
            private set;
        }

        internal VirtualRenderTarget(string name, int width, int height, int multiSampleCount, bool depth, bool preserve)
        {
            base.Name = name;
            base.Width = width;
            base.Height = height;
            this.MultiSampleCount = multiSampleCount;
            this.Depth = depth;
            this.Preserve = preserve;
            this.Reload();
        }

        public override void Dispose()
        {
            this.Unload();
            this.Target = null;
            VirtualContent.Remove(this);
        }

        public static implicit operator RenderTarget2D(VirtualRenderTarget target)
        {
            return target.Target;
        }

        internal override void Reload()
        {
            this.Unload();
            this.Target = new RenderTarget2D(Engine.Instance.GraphicsDevice, base.Width, base.Height, false, SurfaceFormat.Color, (this.Depth ? DepthFormat.Depth24Stencil8 : DepthFormat.None), this.MultiSampleCount, (this.Preserve ? RenderTargetUsage.PreserveContents : RenderTargetUsage.DiscardContents));
        }

        internal override void Unload()
        {
            if (this.Target != null && !this.Target.IsDisposed)
            {
                this.Target.Dispose();
            }
        }
    }
}