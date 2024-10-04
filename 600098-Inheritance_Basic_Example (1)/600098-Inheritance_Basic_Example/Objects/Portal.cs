using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenGL_Game.OBJLoader;

namespace OpenGL_Game.Objects
{
    public class Portal : GameObject
    {
        public bool isActive = false;

        public Portal(string name, Vector3 position, Vector3 velocity, Geometry geometry)
            : base(name, position, velocity, geometry, false, 1.0f)
        {
        }

        public override void Update()
        {
            if(isActive)
            {
                // use special sound effect for active
            }
        }


    }
}
