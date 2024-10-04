using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenGL_Game.OBJLoader;
using OpenGL_Game.Scenes;

namespace OpenGL_Game.Objects
{
    public class Drone : GameObject
    {
        public Drone(string name, Vector3 position, Vector3 velocity, Geometry geometry)
          : base(name, position, velocity, geometry, true, 1.5f)
        {
            
        }

        public override void Close()
        {

        }

        public override void Update()
        {
            position += velocity * GameScene.dt;


        }

    }
}
