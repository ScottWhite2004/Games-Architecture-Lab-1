using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenTK;
using OpenGL_Game.Managers;
using OpenGL_Game.OBJLoader;

namespace OpenGL_Game.Objects
{
    public abstract class GameObject
    {
        public string name;
        public Vector3 position;
        public Vector3 velocity;
        public Geometry geometry;
        public bool isCollidable;
        public float collisionRadius;

        public GameObject(string name, Vector3 position, Vector3 velocity, Geometry geometry,
            bool isCollidable, float collisionRadius)
        {
            this.name = name;
            this.position = position;
            this.velocity = velocity;
            this.geometry = geometry;
            this.isCollidable = isCollidable;
            this.collisionRadius = collisionRadius;
        }

        public abstract void Update();
    }
}
