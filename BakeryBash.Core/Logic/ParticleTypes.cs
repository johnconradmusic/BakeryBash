using System;
using Microsoft.Xna.Framework;
using Monocle;

namespace BakeryBash
{
	public class ParticleTypes
	{
		public static Color TackShooterColor = Calc.HexToColor("e88399");
		public static ParticleType PickupCollected;
		public static ParticleType Impact;
		public static ParticleType Sparks;
		public static ParticleType BallTrail;
		public static ParticleType Cloud;
		public static ParticleType SpecialBall;
		public static ParticleType BreadCrumbs;
		public static ParticleType Debris;
		public static ParticleType BallLaunch;
		public static ParticleType Embers;
		public static ParticleType TackShooter;
		public static ParticleType MissileTrail;
		public static void Load()
		{
			Chooser<MTexture> CrumbChooser = new Chooser<MTexture>(new MTexture[]
			{
				GFX.Game["Particles/breadcrum0"],
				GFX.Game["Particles/breadcrum1"],
			});
			Chooser<MTexture> PickupChooser = new Chooser<MTexture>(new MTexture[]
			{
				GFX.Game["Particles/confetti"],
				GFX.Game["Particles/sparkle"]
			});
			Embers = new ParticleType()
			{
				Size = 2f,
				Color = Color.Orange,
				Color2 = Calc.HexToColor("a4f5ff"),
				LifeMin = 0.8f,
				LifeMax = 2f,
				SpeedMax = 20,
				SpeedMin = 100,
				FadeMode = ParticleType.FadeModes.InAndOut,
				Acceleration = new(0, -200f),
				DirectionRange = MathHelper.TwoPi,
				
			};
			Debris = new ParticleType()
			{
				Color = Color.White,
				Color2 = Color.White,
				ColorMode = ParticleType.ColorModes.Blink,
				FadeMode = ParticleType.FadeModes.Late,
				Acceleration = new(0, 1000f),
				LifeMin = 0.2f,
				LifeMax = .3f,
				SpeedMin = 300f,
				SpeedMax = 400f,
				DirectionRange = MathHelper.PiOver2,
				RotationMode = ParticleType.RotationModes.Random
			};
			BreadCrumbs = new ParticleType(Debris)
			{
				SourceChooser = CrumbChooser,
				Size = 1f
			};
			BallLaunch = new ParticleType()
			{
				SourceChooser = PickupChooser,
				Size = 0.4f,
				Color = Color.White,
				Color2 = Calc.HexToColor("a4f5ff"),
				LifeMin = 0.1f,
				LifeMax = 0.6f,
				SpeedMax = 500,
				SpeedMin = 100,
				FadeMode = ParticleType.FadeModes.Late,
				Acceleration = new(0, 500f),
				DirectionRange = MathHelper.Pi,
				Direction = MathHelper.ToRadians(270)
			};
			BallTrail = new ParticleType()
			{
				Color = Color.White,
				Color2 = Color.White,
				Source = GFX.Game["Particles/sparkle"],
				RotationMode = ParticleType.RotationModes.Random,
				Size = 0.3f,
				SizeRange = 0.3f,
				LifeMin = 0.3f,
				LifeMax = 0.6f,
				FadeMode = ParticleType.FadeModes.Linear,
				ColorMode = ParticleType.ColorModes.Blink,
				SpinMin = 0.5f,
				SpinMax = 2f
			};
			SpecialBall = new ParticleType()
			{
				Color = new Color(1, 1, 1, 0.7f),
				Color2 = new Color(1, 1, 1, 0.0f),
				Source = GFX.Game["Particles/pickup-glow"],
				RotationMode = ParticleType.RotationModes.None,
				Size = .4f,
				LifeMin = 2,
				LifeMax = 2,
				FadeMode = ParticleType.FadeModes.None,
				ColorMode = ParticleType.ColorModes.Fade,
				SpinMin = 0.5f,
				SpinMax = 0.5f,
				ScaleOut = true,
				ScaleOutIncrease = true				
			};
			Cloud = new ParticleType()
			{
				Size = 1f,
				SpinMin = 1,
				SpinMax = 2,
				SpinFlippedChance = true,
				Source = GFX.Game["Particles/cloud0"],
				LifeMin = 0.3f,
				LifeMax = 0.8f,
				SpeedMin = 30,
				SpeedMax = 200,
				ColorMode = ParticleType.ColorModes.Fade,
				FadeMode = ParticleType.FadeModes.Late,
				RotationMode = ParticleType.RotationModes.Random,
				DirectionRange = MathHelper.TwoPi,
				Direction = 0
			};
			Sparks = new ParticleType()
			{
				Size = 2f,
				Color = Color.White,
				Color2 = Calc.HexToColor("a4f5ff"),
				LifeMin = 0.1f,
				LifeMax = 0.6f,
				SpeedMax = 500,
				SpeedMin = 100,
				FadeMode = ParticleType.FadeModes.Late,
				Acceleration = new(0, 2000f),
				DirectionRange = MathHelper.Pi,
				Direction = MathHelper.ToRadians(270)
			};
			Impact = new ParticleType()
			{
				Source = GFX.Game["Effects/impact"],
				Size = 1f,
				Color = Color.White,
				Color2 = Color.White,
				ColorMode = ParticleType.ColorModes.Blink,
				FadeMode = ParticleType.FadeModes.Late,
				LifeMin = 0.1f,
				LifeMax = 0.1f,
				SpeedMin = 0,
				SpeedMax = 0,
				DirectionRange = 0,
				RotationMode = ParticleType.RotationModes.SameAsDirection,
			};
			PickupCollected = new ParticleType()
			{
				SourceChooser = PickupChooser,
				Size = 1f,
				Color = Color.White,
				Color2 = Color.White,
				ColorMode = ParticleType.ColorModes.Blink,
				FadeMode = ParticleType.FadeModes.Late,
				Acceleration = new(0, 1000f),
				LifeMin = 0.3f,
				LifeMax = 1f,
				SpeedMin = 100f,
				SpeedMax = 400f,
				DirectionRange = MathHelper.TwoPi,
				RotationMode = ParticleType.RotationModes.Random,

			};
			TackShooter = new ParticleType(PickupCollected)
			{
				Color = TackShooterColor
            };
			MissileTrail = new ParticleType(Cloud)
			{
				Source = GFX.Game["Particles/cloud0"],
				Color = Color.White,
				Color2 = Color.Orange,
				SpeedMax = 30,
				ColorMode = ParticleType.ColorModes.Choose,
				FadeMode = ParticleType.FadeModes.None,
				ScaleOut = true
			};
		}
	}
}

