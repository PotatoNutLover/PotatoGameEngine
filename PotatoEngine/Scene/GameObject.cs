using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;

using System.ComponentModel;

namespace PotatoEngine
{
    public class GameObject : DrawableObject
    {
        public string Name { get; private set; }
        public TransformComponent Transform;
        public List<Component> Components;
        

        public GameObject(string name,Model3D model3D, Texture texture, Shader shader)
            : base(model3D, texture, shader)
        {
            Name = name;
            Transform = new TransformComponent(this);
            Components = new List<Component>();
        }

        public override void OnLoad()
        {
            base.OnLoad();
            foreach (Component component in Components)
            {
                component.OnLoad();
            }
        }

        public override void OnRenderFrame(Matrix4 view, Matrix4 projection)
        {
            TransformationMatrix = Transform.Transform;
            foreach (Component component in Components)
            {
                component.OnRenderFrame();
            }

            TransformationMatrix = Transform.Transform;
            base.OnRenderFrame(view, projection);
        }

        public virtual void OnUpdateFrame()
        {
            
            TransformationMatrix = Transform.Transform;
            foreach (Component component in Components)
            {
                component.OnUpdateFrame();    
            }
            //Transform.PoolToTransform();
            //Transform.PoolToTransform();
            //TransformationMatrix = Transform.Transform;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            foreach (Component component in Components)
            {
                component.OnDestroy();
            }
        }

        public override void OnUnload()
        {
            
            foreach (Component component in Components)
            {
                component.OnUnload();
            }
            Components = null;
            Transform = null;
            Name = null;

            base.OnUnload();

        }

        public T GetComponent<T>()
        {
            return Components.OfType<T>().FirstOrDefault();
        }

        public List<T> GetComponents<T>()
        {
            return Components.OfType<T>().ToList();
        }

        public new GameObject Clone()
        {
            GameObject instance = new GameObject(Name, Model3D, Texture, Shader);

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
            /*foreach(Component component in Components)
            {
                var type = component.GetType();
                //componentsList.Add(Typed<dynamic>(component, type));
                Console.WriteLine();
            }*/

            instance.Components = componentsList;

            return instance;
        }

    }
}
