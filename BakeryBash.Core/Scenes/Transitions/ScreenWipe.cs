using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Monocle;

namespace BakeryBash;

public abstract class ScreenWipe : Renderer
{
    public static Color WipeColor = Color.Black;
    public Scene Scene;
    public bool WipeIn;
    public float Percent;
    public Action OnComplete;
    public bool Completed;
    public float Duration = 0.5f;
    public float EndTimer;
    private bool ending;


    public ScreenWipe(Scene scene, bool wipeIn, Action onComplete = null)
    {
        this.Scene = scene;
        this.WipeIn = wipeIn;
        if (this.Scene is Level)
            (this.Scene as Level).Wipe = this;
        this.Scene.Add((Monocle.Renderer)this);
        this.OnComplete = onComplete;
    }

    public IEnumerator Wait()
    {
        while ((double)this.Percent < 1.0)
            yield return (object)null;
    }

    public override void Update(Scene scene)
    {
        if (!this.Completed)
        {
            if ((double)this.Percent < 1.0)
                this.Percent = Calc.Approach(this.Percent, 1f, Engine.RawDeltaTime / this.Duration);
            else if ((double)this.EndTimer > 0.0)
                this.EndTimer -= Engine.RawDeltaTime;
            else
                this.Completed = true;
        }
        else
        {
            if (this.ending)
                return;
            this.ending = true;
            scene.Remove((Monocle.Renderer)this);
            if (scene is Level && (scene as Level).Wipe == this)
                (scene as Level).Wipe = (ScreenWipe)null;
            if (this.OnComplete == null)
                return;
            this.OnComplete();
        }
    }

    public virtual void Cancel()
    {
        this.Scene.Remove((Monocle.Renderer)this);
        if (!(this.Scene is Level))
            return;
        (this.Scene as Level).Wipe = (ScreenWipe)null;
    }
}

