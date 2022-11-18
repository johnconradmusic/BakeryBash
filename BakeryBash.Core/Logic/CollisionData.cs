using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using Monocle;

namespace BakeryBash
{
    public delegate void Collision(CollisionData data);

    public struct CollisionData
    {
        public Vector2 Direction;
        public Vector2 Moved;
        public Vector2 Remaining;
        public Vector2 TargetPosition;
        public Entity Other;
        public static readonly CollisionData Empty;
    }
}

