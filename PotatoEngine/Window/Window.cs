using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;


namespace PotatoEngine
{
    public class Window : GameWindow
    {
        public Color BackgroundColor;
        public string WindowTitle;
        public Scene CurrentScene = null;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings, Color backgroundColor /*Scene scene*/) 
            : base(gameWindowSettings, nativeWindowSettings)
        {
            BackgroundColor = backgroundColor;
            //CurrentScene = scene;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            WindowTitle = Title;
            GL.ClearColor(BackgroundColor);
            //GL.Enable(EnableCap.DepthTest);
            GLFW.SwapInterval(1);
            //GL.Enable(EnableCap.Multisample);

            WindowVariables.window = this;

            CurrentScene.OnLoad();

            CursorGrabbed = true;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            //GL.Enable(EnableCap.Multisample);

            CurrentScene.OnRenderFrame();

            GL.Disable(EnableCap.DepthTest);
            //GL.Disable(EnableCap.Multisample);

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            WindowVariables.deltaTime = e.Time;
            WindowVariables.input = KeyboardState;
            WindowVariables.mouseState = MouseState;

            CurrentScene.OnUpdateFrame();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);

            CurrentScene.CameraObject.Camera.AspectRatio = Size.X / (float)Size.Y;
        }

        protected override void OnUnload()
        {
            CurrentScene.OnUnload();
            CurrentScene = null;
            
            base.OnUnload();
        }
    }
}
