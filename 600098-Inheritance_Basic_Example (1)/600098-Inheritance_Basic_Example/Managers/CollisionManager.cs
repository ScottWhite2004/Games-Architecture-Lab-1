using OpenGL_Game.Objects;

namespace OpenGL_Game.Managers
{
    class CollisionManager
    {

        public CollisionManager()
        {
        }

        public bool SphereCollision(GameObject object1, GameObject object2)
        {
            // Simple spherical collsion detection
            if (object1.isCollidable && object2.isCollidable)
            {
                if((object1.position-object2.position).Length < object1.collisionRadius+object2.collisionRadius)
                {
                    return true;
                }
            }

            return false;
        }

        public bool PlaneCollision(GameObject object1, GameObject object2)
        {
            // There would be an implementation of player agianst maze

            return false;
        }
    }
}
