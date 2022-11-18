using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BakeryBash.Scenes;
using System.Collections;
using Microsoft.Xna.Framework;

namespace BakeryBash.Entities;
[Tracked(true)]
public abstract class PickupItem : GridEntity
{
    public enum PickupType
    {
        ShockJar, PoisonJar, MultiBall, Missile, TackShooter, SpecialBall
    }

    public Color ParticleColor;
    private bool IsActive;

    public bool DoesFloat = true;

    public PickupItem() : base()
    {
        Collider = new Circle(Level.GridSize / 2);
        Collidable = true;
        Tag = Tags.PickupsTag;
        
    }

    public override void Added(Scene scene)
    {
        base.Added(scene);
        SceneAs<Level>().PickupItems.Add(this);
    }

    public abstract IEnumerator DoPickup(Entity other);

    private IEnumerator CollectionRoutine(Entity other)
    {
        GameManager.Instance.AddWait("pickup");
        yield return DoPickup(other);
        GameManager.Instance.RemoveWait("pickup");
        Level.Instance.GridEntities.Remove(this);
        SceneAs<Level>().PickupItems.Remove(this);
        RemoveSelf();
    }


    public void Collect(Entity other)
    {
        if (!IsActive)
        {
            IsActive = true;
            Add(new Coroutine(CollectionRoutine(other)));
        }
    }


    public override void Update()
    {
        if (!Moving)
        {
            if (DoesFloat)
                Position = target;
        }



        base.Update();
    }
}