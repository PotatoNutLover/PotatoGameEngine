using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;

namespace PotatoEngine
{
    public class ColliderComponent : Component
    {
        public bool Enable = true;
        private Box3 _collider;
        private Model3D _currentModel3D;
        private Vector3 _colliderMin = Vector3.One;
        private Vector3 _colliderMax = Vector3.One;
        public bool IsTrigger { get; set; } = false;
        public Vector3 ColliderOffset { get; private set; } = Vector3.Zero;
        public Box3 Collider
        {
            get
            {
                return _collider;
            }
        }

        public ColliderComponent(GameObject gameObject, Model3D model3D, bool enable)
            :base(gameObject)
        {
            Enable = enable;
            _collider = new Box3();
            _currentModel3D = model3D;
            SetColliderSize();
            SetColliderTransformation();
        }

        public void SetColliderTransformation()
        {
            var _currentTransform = gameObject.Transform;
            //Console.WriteLine(_colliderMin);
            _collider.Min = _colliderMin;
            _collider.Max = _colliderMax;
            _collider.Translate(_currentTransform.GetPosition());
            //Console.WriteLine(_currentTransform.GetPosition());
            Vector3 size = _collider.Size;
            _collider.Scale(_currentTransform.GetScale() + ColliderOffset, _collider.Center);
        }

        private void SetColliderSize()
        {
            float max = 0;
            int index = 0;
            float current = 0;
            for (int i = 0; i < _currentModel3D.Vertices.Length; i = i + 3)
            {
                current = _currentModel3D.Vertices[i] + _currentModel3D.Vertices[i + 1] + _currentModel3D.Vertices[i + 2];
                if (current > max)
                {
                    max = current;
                    index = i;
                }
            }
            _collider.Max = new Vector3(_currentModel3D.Vertices[index], _currentModel3D.Vertices[index + 1], _currentModel3D.Vertices[index + 2]);

            index = 0;
            float min = max;
            for (int i = 0; i < _currentModel3D.Vertices.Length; i = i + 3)
            {
                current = _currentModel3D.Vertices[i] + _currentModel3D.Vertices[i + 1] + _currentModel3D.Vertices[i + 2];
                if (current < min)
                {
                    min = current;
                    index = i;
                }
            }
            _collider.Min = new Vector3(_currentModel3D.Vertices[index], _currentModel3D.Vertices[index + 1], _currentModel3D.Vertices[index + 2]);

            _colliderMin = _collider.Min;
            _colliderMax = _collider.Max;

        }

        public bool CollisionDetected(Vector3 point)
        {
            return _collider.Contains(point);
        }

        public void SetOffset(Vector3 offset)
        {
            ColliderOffset = offset;
        }

        public Vector3 GetSize()
        {
            return _collider.Size;
        }

        public override void OnUpdateFrame()
        {
            
            base.OnUpdateFrame();
            SetColliderTransformation();

        }

        public static Collision CollisionDetected(ColliderComponent collider)
        {
            Collision CollisionInfo = new Collision() { ColliderComponents = new List<ColliderComponent>(), Detected = false };
            foreach (GameObject gameObject in WindowVariables.window.CurrentScene.GameObjects)
            {
                if (gameObject.GetComponent<ColliderComponent>() != null)
                {                    
                    foreach(ColliderComponent colliderComponent in gameObject.GetComponents<ColliderComponent>())
                        if (colliderComponent._collider.Contains(collider._collider))
                        {
                            CollisionInfo.Detected = true;
                            CollisionInfo.ColliderComponents.Add(colliderComponent);
                        }                 
                }
                
            }

            if (WindowVariables.window.CurrentScene.CameraObject.GetComponent<ColliderComponent>() != null)
            {
                foreach (ColliderComponent colliderComponent in WindowVariables.window.CurrentScene.CameraObject.GetComponents<ColliderComponent>())
                    if (colliderComponent._collider.Contains(collider._collider))
                    {
                        CollisionInfo.Detected = true;
                        CollisionInfo.ColliderComponents.Add(colliderComponent);
                    }
            }

            return CollisionInfo;
        }

        public static Collision CollisionDetectedPoint(Vector3 point)
        {
            Collision CollisionInfo = new Collision() { ColliderComponents = new List<ColliderComponent>(), Detected = false };
            foreach (GameObject gameObject in WindowVariables.window.CurrentScene.GameObjects)
            {
                if (gameObject.GetComponent<ColliderComponent>() != null)
                {
                    foreach (ColliderComponent colliderComponent in gameObject.GetComponents<ColliderComponent>())
                        if (colliderComponent.CollisionDetected(point))
                        {
                            CollisionInfo.Detected = true;
                            CollisionInfo.ColliderComponents.Add(colliderComponent);
                        }
                }
                
            }
            return CollisionInfo;
        }

        public override void OnUnload()
        {
            base.OnUnload();

            _currentModel3D = null;
        }

        public override Component Clone(GameObject gameObject)
        {
            
            return new ColliderComponent(gameObject, _currentModel3D, Enable) { IsTrigger = this.IsTrigger };
        }
    }
}
