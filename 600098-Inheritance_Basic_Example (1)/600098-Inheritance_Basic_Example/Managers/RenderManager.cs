using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.OBJLoader;
using OpenGL_Game.Scenes;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Managers
{
    class RenderManager
    {
        protected int pgmID;
        protected int vsID;
        protected int fsID;
        protected int uniform_stex;
        protected int uniform_mmodelviewproj;
        protected int uniform_mmodel;
        protected int uniform_diffuse;  // OBJ NEW

        public RenderManager()
        {
            pgmID = GL.CreateProgram();
            LoadShader("Shaders/vs.glsl", ShaderType.VertexShader, pgmID, out vsID);
            LoadShader("Shaders/fs.glsl", ShaderType.FragmentShader, pgmID, out fsID);
            GL.LinkProgram(pgmID);
            Console.WriteLine(GL.GetProgramInfoLog(pgmID));

            uniform_stex = GL.GetUniformLocation(pgmID, "s_texture");
            uniform_mmodelviewproj = GL.GetUniformLocation(pgmID, "ModelViewProjMat");
            uniform_mmodel = GL.GetUniformLocation(pgmID, "ModelMat");
            uniform_diffuse = GL.GetUniformLocation(pgmID, "v_diffuse");     // OBJ NEW
        }

        void LoadShader(String filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }

        public void Draw(GameObject gameObject)
        {
            if (gameObject.geometry != null)
            {
                Vector3 position = gameObject.position;
                Matrix4 model = Matrix4.CreateTranslation(position);

                GL.UseProgram(pgmID);

                GL.Uniform1(uniform_stex, 0);
                GL.ActiveTexture(TextureUnit.Texture0);

                GL.UniformMatrix4(uniform_mmodel, false, ref model);
                Matrix4 modelViewProjection = model * GameScene.gameInstance.camera.view * GameScene.gameInstance.camera.projection;
                GL.UniformMatrix4(uniform_mmodelviewproj, false, ref modelViewProjection);

                gameObject.geometry.Render(uniform_diffuse);

                GL.UseProgram(0);
            }
        }
    }
}
