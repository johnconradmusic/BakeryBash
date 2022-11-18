using Microsoft.Xna.Framework.Graphics;
using Monocle;

namespace BakeryBash
{
    public class GameRenderer : Renderer
    {
        public Camera Camera;
        private static GameRenderer instance;

        public GameRenderer()
        {
            GameRenderer.instance = this;
            this.Camera = new Camera(1920, 1080);
        }

        public static void Begin() => Draw.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, (Effect)null, GameRenderer.instance.Camera.Matrix);

        public override void Render(Scene scene)
        {
            GameRenderer.Begin();
            scene.Entities.RenderExcept(Tags.HUD);
            if (Engine.Commands.Open)
                scene.Entities.DebugRender(this.Camera);
            GameRenderer.End();
        }

        public static void End() => Draw.SpriteBatch.End();
    }
}
