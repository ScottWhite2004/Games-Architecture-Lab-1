using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenGL_Game.OBJLoader;

namespace OpenGL_Game.Objects
{
    public class Player : GameObject
    {
        public int lives = 3;

        public Player(string name, Vector3 position, Vector3 velocity, Geometry geometry)
            : base(name, position, velocity, geometry, true, 1.5f)
        {
        }

        public override void Update()
        {

        }


    }
}
