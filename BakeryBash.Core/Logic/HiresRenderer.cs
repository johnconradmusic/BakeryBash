﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;

namespace BakeryBash
{
    public class HiresRenderer : Monocle.Renderer
    {
        public static VirtualRenderTarget Buffer => BakeryBash.RenderTarget;

        public static bool DrawToBuffer
        {
            get
            {
                if (HiresRenderer.Buffer == null)
                    return false;
                return Engine.ViewWidth < 1920 || Engine.ViewHeight < 1080;
            }
        }

        public static void BeginRender(BlendState blend = null, SamplerState sampler = null)
        {
            if (blend == null)
                blend = BlendState.NonPremultiplied;
            if (sampler == null)
                sampler = SamplerState.LinearClamp;
            Matrix transformMatrix = HiresRenderer.DrawToBuffer ? Matrix.Identity : Engine.ScreenMatrix;
            Draw.SpriteBatch.Begin(SpriteSortMode.Deferred, blend, sampler, DepthStencilState.Default, RasterizerState.CullNone, (Effect)null, transformMatrix);
        }

        public static void EndRender() => Draw.SpriteBatch.End();

        public override void BeforeRender(Scene scene)
        {
            if (HiresRenderer.DrawToBuffer)
            {
                Engine.Graphics.GraphicsDevice.SetRenderTarget((RenderTarget2D)HiresRenderer.Buffer);
                Engine.Graphics.GraphicsDevice.Clear(Color.Transparent);
                this.RenderContent(scene);
            }
        }

        public override void Render(Scene scene)
        {
            if (HiresRenderer.DrawToBuffer)
            {
                Draw.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, (Effect)null, Engine.ScreenMatrix);
                Draw.SpriteBatch.Draw((Texture2D)(RenderTarget2D)HiresRenderer.Buffer, new Vector2(-1f, -1f), Color.White);
                Draw.SpriteBatch.End();
            }
            else
                this.RenderContent(scene);
        }

        public virtual void RenderContent(Scene scene)
        {
            
        }
    }
}