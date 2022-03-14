using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace PotatoEngine
{
    public class DrawableObject
    {
        public Model3D Model3D { get; private set; }
        public Texture Texture { get; private set; }
        public Shader Shader { get; private set; }
        public int IdInScene { get; set; }
        public Scene ParentScene { get; set; }

        public Matrix4 TransformationMatrix;

        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private int _elementBufferObject;
        private int _textureBufferObject;

        public DrawableObject ( Model3D model3D, Texture texture, Shader shader)
        {
            Model3D = model3D.Clone();
            Texture = texture;
            Shader = shader;
            TransformationMatrix = Matrix4.Identity;
        }

        public virtual void OnLoad()
        {
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(
                BufferTarget.ArrayBuffer, 
                Model3D.Vertices.Length * sizeof(float), 
                Model3D.Vertices, 
                BufferUsageHint.StaticDraw);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(
                BufferTarget.ElementArrayBuffer, 
                Model3D.Indices.Length * sizeof(uint), 
                Model3D.Indices, 
                BufferUsageHint.StaticDraw);

            Shader.Use();

            var vertexLocation = Shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(
                vertexLocation, 
                3, 
                VertexAttribPointerType.Float, 
                false, 3 * sizeof(float), 
                0);

            _textureBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _textureBufferObject);
            GL.BufferData(
                BufferTarget.ArrayBuffer, 
                Model3D.UVCoords.Length * sizeof(float), 
                Model3D.UVCoords, 
                BufferUsageHint.StaticDraw);

            var texCoordLocation = Shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(
                texCoordLocation, 
                2, 
                VertexAttribPointerType.Float, 
                false, 2 * sizeof(float), 
                0);

            Texture.Use(TextureUnit.Texture0);

            Shader.SetInt("texture0", 0);
        }

        public virtual void OnRenderFrame(Matrix4 view, Matrix4 projection)
        {
            if (Texture.RenderQueue == RenderQueue.Opaque)
                RenderOpaque(view, projection);
            else
                RenderTransparent(view, projection);
        }

        private void RenderOpaque(Matrix4 view, Matrix4 projection)
        {
            GL.BindVertexArray(_vertexArrayObject);

            Texture.Use(TextureUnit.Texture0);
            Shader.Use();

            Shader.SetMatrix4("model", TransformationMatrix);
            Shader.SetMatrix4("view", view);
            Shader.SetMatrix4("projection", projection);

            GL.DrawElements(PrimitiveType.Triangles, Model3D.Indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        private void RenderTransparent(Matrix4 view, Matrix4 projection)
        {
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
          
            GL.BindVertexArray(_vertexArrayObject);

            Texture.Use(TextureUnit.Texture0);
            Shader.Use();

            Shader.SetMatrix4("model", TransformationMatrix);
            Shader.SetMatrix4("view", view);
            Shader.SetMatrix4("projection", projection);

            GL.CullFace(CullFaceMode.Front);
            GL.DrawElements(PrimitiveType.Triangles, Model3D.Indices.Length, DrawElementsType.UnsignedInt, 0);

            GL.CullFace(CullFaceMode.Back);
            GL.DrawElements(PrimitiveType.Triangles, Model3D.Indices.Length, DrawElementsType.UnsignedInt, 0);

            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.Blend);
        }

        public virtual void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            //GL.UseProgram(0);

            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteBuffer(_textureBufferObject);
            GL.DeleteVertexArray(_vertexArrayObject);

            GL.DeleteProgram(Shader.Handle);

            Model3D.Unload();
            Model3D = null;
            Texture = null;
            Shader = null;
            ParentScene = null;
        }

        public void Destroy()
        {
            OnDestroy();
            ParentScene.DeleteGameObject(this as GameObject);
            OnUnload();
        }

        public virtual void OnDestroy()
        {

        }

        public DrawableObject Clone()
        {
            return new DrawableObject(Model3D, Texture, Shader);
        }

        
    }
}
