using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using System.Drawing;
using System;
using System.Collections.Generic;

namespace OpenGL_Game.Scenes
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    class GameScene : Scene
    {
        public static float dt = 0;
        public RenderManager renderManager;
        public CollisionManager collisionManager;
        List<GameObject> objectList = new List<GameObject>();

        bool[] keysPressed = new bool[255];

        public int score = 0;
        public Camera camera;
        public Player player;
        public Drone drone;
        public PortalKey key;
        public Portal portal;
        public Maze maze;
        
        public Vector3 playerStartingPosition = new Vector3(0, 1.8f, 7);
        public Vector3 cameraStartingLookAt = new Vector3(0, 1.8f, 0);
        public Vector3 droneStartingPosition = new Vector3(-4, 0, 0);

        public static GameScene gameInstance;

        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
            gameInstance = this;
            renderManager = new RenderManager();
            collisionManager = new CollisionManager();

            // Set the title of the window
            sceneManager.Title = "Game";
            // Set the Render and Update delegates to the Update and Render methods of this class
            sceneManager.renderer = Render;
            sceneManager.updater = Update;
            // Set Keyboard events to go to a method in this class
            sceneManager.keyboardDownDelegate += Keyboard_KeyDown;
            sceneManager.keyboardUpDelegate   += Keyboard_KeyUp;

            // Enable Depth Testing
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            // Set Camera
            camera = new Camera(playerStartingPosition, cameraStartingLookAt, (float)(sceneManager.Width) / (float)(sceneManager.Height), 0.1f, 100f);
            // Player is at the camera
            player = new Player("Player", playerStartingPosition, new Vector3(0, 0, 0), null);
            objectList.Add(player);
            // Drone 
            drone = new Drone("Drone", droneStartingPosition, new Vector3(0, 0, 0), ResourceManager.LoadGeometry("Geometry/Moon/moon.obj"));
            objectList.Add(drone);
            // Portal Key
            key = new PortalKey("Key", new Vector3(+4, 0, 0), new Vector3(0, 0, 0), ResourceManager.LoadGeometry("Geometry/Moon/moon_key.obj"));
            objectList.Add(key);
            // Portal
            portal = new Portal("Portal", new Vector3(0, 0, 0), new Vector3(0, 0, 0), ResourceManager.LoadGeometry("Geometry/Moon/moon_portal.obj"));
            objectList.Add(portal);
            // Maze
            maze = new Maze("Maze", new Vector3(27.5f, 0, -27.5f), new Vector3(0, 0, 0), ResourceManager.LoadGeometry("Geometry/Maze/maze.obj"));
            objectList.Add(maze);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        public override void Update(FrameEventArgs e)
        {
            dt = (float)e.Time;
            //System.Console.WriteLine("fps=" + (int)(1.0/dt));

            if (GamePad.GetState(1).Buttons.Back == ButtonState.Pressed)
                sceneManager.Exit();

            if (keysPressed[(char)Key.Up])
            {
                camera.MoveForward(0.1f);
            }
            if (keysPressed[(char)Key.Down])
            {
                camera.MoveForward(-0.1f);
            }
            if (keysPressed[(char)Key.Left])
            {
                camera.RotateY(-0.01f);
            }
            if (keysPressed[(char)Key.Right])
            {
                camera.RotateY(0.01f);
            }
            if (keysPressed[(char)Key.M])
            {
                sceneManager.ChangeScene(SceneTypes.SCENE_GAME_OVER);
            }

            // TODO: Add your update logic here

            // Set player at camera
            player.position = camera.cameraPosition;
            // Update enities
            foreach(GameObject gameObject in objectList)
            {
                gameObject.Update();
            }

            if(collisionManager.SphereCollision(player, drone))
            {
                // Player and Drone have collided
                LoseLife();
            }
            if (collisionManager.SphereCollision(player, key))
            {
                // Player and Key have collided
                KeyCollected();
            }
            if (collisionManager.SphereCollision(player, portal))
            {
                // Player has won
                sceneManager.ChangeScene(SceneTypes.SCENE_GAME_OVER);
            }
            if (collisionManager.PlaneCollision(player, maze))
            {
                // Handle player/maze collisions
            }

        }

        public void LoseLife()
        {
            --player.lives;
            if(player.lives == 0)
            {
                sceneManager.ChangeScene(SceneTypes.SCENE_GAME_OVER);
            }
            else
            {
                // Set Camera
                camera = new Camera(playerStartingPosition, cameraStartingLookAt, (float)(sceneManager.Width) / (float)(sceneManager.Height), 0.1f, 100f);
                // Player is at the camera
                player.position = playerStartingPosition;
                // Drone 
                drone.position = droneStartingPosition;
            }
        }

        public void KeyCollected()
        {
            ++score;
            key.geometry = null;
            key.isCollidable = false;
            portal.isActive = true;
            portal.isCollidable = true;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        public override void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, sceneManager.Width, sceneManager.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Update enities
            foreach (GameObject gameObject in objectList)
            {
                renderManager.Draw(gameObject);
            }

            // Render score
            float width = sceneManager.Width, height = sceneManager.Height, fontSize = Math.Min(width, height) / 10f;
            GUI.clearColour = Color.Transparent;
            GUI.Label(new Rectangle(0, 0, (int)width, (int)(fontSize * 2f)), "Lives:" +player.lives + " Score:" + score, 18, StringAlignment.Near, Color.White);
            GUI.Render();
        }

        /// <summary>
        /// This is called when the game exits.
        /// </summary>
        public override void Close()
        {
            sceneManager.keyboardDownDelegate -= Keyboard_KeyDown;
            sceneManager.keyboardUpDelegate   -= Keyboard_KeyUp;
            ResourceManager.RemoveAllAssets();

        }

        public void Keyboard_KeyDown(KeyboardKeyEventArgs e)
        {
            keysPressed[(char)e.Key] = true;
        }

        public void Keyboard_KeyUp(KeyboardKeyEventArgs e)
        {
            keysPressed[(char)e.Key] = false;
        }

    }
}
