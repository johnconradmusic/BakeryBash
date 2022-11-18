using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using Monocle;
using System.Security.AccessControl;


namespace BakeryBash
{
	public abstract class ImageMaskTransition : ScreenWipe
	{


		public float MinScale = 0.001f;
		public float MaxScale = 2f;
		public float MinRotation = 0;
		public float MaxRotation = MathHelper.TwoPi;


		float _renderScale;
		float _renderRotation;


		MTexture _maskTexture;
		Vector2 _maskPosition;

		BlendState _blendState;
		RenderTarget2D _maskRenderTarget;


		public ImageMaskTransition(MTexture maskTexture, Scene scene, bool wipeIn, Action onComplete = null) : base(scene, wipeIn, onComplete)
		{
			_maskPosition = new Vector2(Engine.ViewWidth / 2, Engine.ViewHeight / 2);
			_maskRenderTarget = new RenderTarget2D(Engine.Graphics.GraphicsDevice, Engine.ViewWidth, Engine.ViewHeight, false,
				SurfaceFormat.Color, DepthFormat.None);
			_maskTexture = maskTexture;
			MaxScale = (float)Engine.ViewWidth / (_maskTexture.Width);
			_renderScale = MaxScale;

			_blendState = new BlendState
			{
				ColorSourceBlend = Blend.DestinationColor,
				ColorDestinationBlend = Blend.Zero,
				ColorBlendFunction = BlendFunction.Add
			};
		}

		public override void BeforeRender(Scene scene)
		{
			Engine.Graphics.GraphicsDevice.SetRenderTarget(_maskRenderTarget);
			var batcher = Monocle.Draw.SpriteBatch;
			batcher.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, null);
			_maskTexture.DrawCentered(_maskPosition, Color.White, _renderScale, _renderRotation);
			batcher.End();
			Engine.Graphics.GraphicsDevice.SetRenderTarget(null);
		}

		public override void Update(Scene scene)
		{
			base.Update(scene);
			float percent = Percent;
			if (WipeIn)
			{
				_renderScale = MathHelper.Lerp(MinScale, MaxScale, Ease.CubeInOut(percent));
				_renderRotation = MathHelper.Lerp(MinRotation, MaxRotation, Ease.CubeInOut(percent));
			}
			else
			{
				_renderScale = MathHelper.Lerp(MaxScale, MinScale, Ease.CubeInOut(percent));
				_renderRotation = MathHelper.Lerp(MaxRotation, MinRotation, Ease.CubeInOut(percent));
			}

		}

		public override void Render(Scene scene)
		{		
			Engine.Graphics.GraphicsDevice.SetRenderTarget(null);
			var batcher = Monocle.Draw.SpriteBatch;
			batcher.Begin(SpriteSortMode.Deferred, _blendState, SamplerState.AnisotropicClamp, DepthStencilState.None, null);
			batcher.Draw(_maskRenderTarget, new Vector2(Engine.ViewWidth / 2, Engine.ViewHeight / 2), null, Color.White, 0, new Vector2(_maskRenderTarget.Width / 2, _maskRenderTarget.Height / 2), 1, SpriteEffects.None, 0);
			batcher.End();
		}
	}
}