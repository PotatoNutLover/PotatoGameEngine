using System;
using System.Collections.Generic;
using System.Text;

namespace PotatoEngine
{
    public class CameraObject : GameObject
    {
        public Camera Camera { get; private set; }

        public CameraObject(string name, float aspectRatio, CameraSettings cameraSettings, Model3D model3D, Texture texture, Shader shader)
            :base(name, model3D, texture, shader)
        {
            Camera = new Camera(Transform.GetPosition(), aspectRatio, cameraSettings);
        }

        public override void OnUpdateFrame()
        {
            Camera.Position = Transform.GetPosition();
            base.OnUpdateFrame();
        }

        public new CameraObject Clone()
        {
            //GameObject cameraGo =  base.Clone();
            CameraObject instance =  new CameraObject(
                Name,
                Camera.AspectRatio,
                Camera.CameraSettings,
                Model3D.Clone(),
                Texture,
                Shader);

            TransformComponent transform = new TransformComponent(
                instance,
                Transform.Transform,
                Transform.ParentTransform,
                Transform.Pivot);
            List<Component> componentsList = new List<Component>();
            instance.Transform = transform;

            for (int i = 0; i < Components.Count; i++)
            {
                componentsList.Add(Components[i].Clone(instance));
            }
            
            instance.Components = componentsList;

            return instance;
        }

    }
}
