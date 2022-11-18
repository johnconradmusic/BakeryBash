using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using BakeryBash.Scenes;

namespace BakeryBash.Entities;
[Tracked]
public class Ball : Actor
{
    protected const float BALLSPEED = 900;
    protected const int BALLSIZE = 20;
    public const int BALLRADIUS = BALLSIZE / 2;
    protected Vector2 velocity;
    protected Sprite sprite;
    public bool IsMainBall;
    public DamageEffect damageEffect;
    public enum BallType
    {
        Normal, Shock, Bomb, Poison, Multiball
    }

    public BallType ballType;

    public Ball(Vector2 position, Vector2 direction, BallType type, bool isMainBall)
    {
        IsMainBall = isMainBall;
        ballType = type;
        this.velocity = direction;
        GameManager.Instance.AddWait("ball");
        Add(sprite = GFX.SpriteBank.Create("ball"));
        sprite.Justify = new Vector2(0.5f);
        Position = position;
        Collider = new Circle(BALLRADIUS);
        Tag = Tags.BallsTag;
        CollidesWithTag = Tags.ObstaclesTag;

        switch (type)
        {
            case BallType.Normal:
                damageEffect = DamageEffect.None;
                sprite.Play("normal");
                break;
            case BallType.Shock:
                 damageEffect = DamageEffect.Shock;
                sprite.Play("shock");
                break;
            case BallType.Bomb:
                damageEffect = DamageEffect.LargeExplosion;
                sprite.Play("bomb");
                break;
            case BallType.Poison:
                damageEffect = DamageEffect.Poison;
                sprite.Play("poison");
                break;
            case BallType.Multiball:
                damageEffect = DamageEffect.Multiply;
                sprite.Play("multiball", false, true);
                break;
        }


        //Add(new TrailRibbon());
    }
    bool falling;


    public override void Update()
    {

        base.Update();

        if (falling)
        {
            velocity.Y += 0.05f;
            velocity.X *= 0.98f;
        }
        else
        {
            velocity.Normalize();
        }

        MoveH(velocity.X * BALLSPEED * Engine.DeltaTime, new Collision(OnCollide));
        MoveV(velocity.Y * BALLSPEED * Engine.DeltaTime, new Collision(OnCollide));


        for (int i = 0; i < Level.Instance.PickupItems.Count; i++)
        {
            PickupItem p = Level.Instance.PickupItems[i];
            if (CollideCheck(p)) p.Collect(this);
        }

        if (Level.Instance.GridEntities.Count == 0 || (velocity.Y > 0 && Level.Instance.GridEntities.TrueForAll((gridEntity) => gridEntity.Position.Y + Level.GridSize < Position.Y)))
            falling = true;


        if (Y > Engine.Height)
        {
            if (IsMainBall) Level.Instance.BallLauncher.SetNewLaunchPosition(X);
            GameManager.Instance.RemoveWait("ball");
            RemoveSelf();
        }
        if (ballType != BallType.Normal)
            Level.Instance.ParticlesBG.Emit(ParticleTypes.BallTrail, 1, Position, new Vector2(10));
    }

    private void OnCollide(CollisionData collisionData)
    {
        Enemy.HitSide hitSide = Enemy.HitSide.Center;

		if (collisionData.Direction == Vector2.UnitX)
		{  //right side of ball
			hitSide = Enemy.HitSide.Left;
			velocity.X *= -1;
		}
		if (collisionData.Direction == -Vector2.UnitX)
        {  //left side of ball
            hitSide = Enemy.HitSide.Right;
            velocity.X *= -1;
        }
        if (collisionData.Direction == Vector2.UnitY)
        { //bottom side of ball
            hitSide = Enemy.HitSide.Top;
            velocity.Y *= -1;
        }
        if (collisionData.Direction == -Vector2.UnitY)
        {  //top side of ball
            hitSide = Enemy.HitSide.Bottom;
            velocity.Y *= -1;
        }

        if (collisionData.Other is Enemy enemy)
        {
			switch (ballType)
			{
				case BallType.Normal:

					enemy.Hit(GameManager.Instance.PlayerAttributes.BallDamage, DamageEffect.None, hitSide, this);
					break;
				case BallType.Multiball:
					enemy.Hit(GameManager.Instance.PlayerAttributes.BallDamage,  DamageEffect.Multiply, hitSide, this);
					break;
				case BallType.Shock:
                    Level.Instance.Flash(Color.LightBlue);
					enemy.Hit(GameManager.Instance.PlayerAttributes.LightningDamage,  DamageEffect.Shock, hitSide, this);
					break;
				case BallType.Bomb:
					Scene.Add(new Explosion(Position));
					enemy.Hit(GameManager.Instance.PlayerAttributes.BombDamage,  DamageEffect.LargeExplosion, hitSide, this);
					GameManager.Instance.RemoveWait("ball");
                    SceneAs<Level>().Shake();
                    RemoveSelf();
					break;
				case BallType.Poison:
					enemy.Hit(GameManager.Instance.PlayerAttributes.PoisonDamage,  DamageEffect.Poison, hitSide, this);
					break;
			}
		}

        if (collisionData.Other is Wall)
            SceneAs<Level>().ParticlesFG.Emit(ParticleTypes.Impact, 1, Position - (collisionData.Direction * 3), Vector2.Zero, (-collisionData.Direction).Angle());
		MInput.Touch.Vibrate(4);

		Position -= collisionData.Remaining;

    }
}

