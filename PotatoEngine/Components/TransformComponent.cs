using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;

namespace PotatoEngine
{
    public class TransformComponent : Component
    {
        private TransformationPool _transformationPool = new TransformationPool();
        public Matrix4 Transform;
        private TransformComponent _parentTransform = null;
        public TransformComponent ParentTransform
        {
            get
            {
                return _parentTransform;
            }
            set
            {
                _parentTransform = value;
                ParentOffset = Transform;
            }
        }
        public Matrix4 ParentOffset { get; set; }
        public Vector3 Pivot { get; set; } = new Vector3(-1f, 0f, 0f);

        public TransformComponent(GameObject gameObject) 
            : base(gameObject)
        {
            Transform = Matrix4.Identity;
        }

        public TransformComponent(GameObject gameObject, Matrix4 transform, TransformComponent parentTransform, Vector3 pivot)
            : base(gameObject)
        {
            Transform = transform;
            ParentTransform = parentTransform;
            Pivot = pivot;
            //_transformationPool = new TransformationPool();
            _transformationPool.WritePool(Transform);
        }


        public void SetPosition(Vector3 position)
        {
            //_transformationPool.WritePool(Transform);
            _transformationPool.Translation = Matrix4.Identity * Matrix4.CreateTranslation(position);
            Transform = _transformationPool.GetTransformationMatrix();
        }

        public void SetRotation(Vector3 rotation)
        {
            //_transformationPool.WritePool(Transform);
            _transformationPool.Rotation = Matrix4.Identity * Matrix4.CreateRotationX(rotation.X)
                                                            * Matrix4.CreateRotationY(rotation.Y)
                                                            * Matrix4.CreateRotationZ(rotation.Z);
            Transform = _transformationPool.GetTransformationMatrix();
        }

        public void SetScale(Vector3 scale)
        {
            //_transformationPool.WritePool(Transform);
            _transformationPool.Scale = Matrix4.Identity * Matrix4.CreateScale(scale);
            Transform = _transformationPool.GetTransformationMatrix();
        }

        public void Translate(Vector3 translation)
        {
            //_transformationPool.WritePool(Transform);
            _transformationPool.Translation = Matrix4.Identity * Matrix4.CreateTranslation(_transformationPool.Translation.ExtractTranslation() + translation);
            Transform = _transformationPool.GetTransformationMatrix();
        }

        public void Rotate(Vector3 rotation)
        {
            //_transformationPool.WritePool(Transform);
            _transformationPool.Rotation = _transformationPool.Rotation * Matrix4.CreateRotationX(rotation.X)
                                                                        * Matrix4.CreateRotationY(rotation.Y)
                                                                        * Matrix4.CreateRotationZ(rotation.Z);
            Transform = _transformationPool.GetTransformationMatrix();
        }

        public Vector3 GetPosition()
        {
            return _transformationPool.Translation.ExtractTranslation();
        }

        public Quaternion GetRotation()
        {
            return _transformationPool.Rotation.ExtractRotation(false);
        }

        public Vector3 GetScale()
        {
            return _transformationPool.Scale.ExtractScale();
        }

        public override void OnUnload()
        {
            base.OnUnload();

            _transformationPool = null;
            ParentTransform = null;
        }

    }
}


/*namespace PotatoEngine
{
    public class TransformComponent : Component
    {
        private Matrix4 _transform;
        public Matrix4 Transform
        {
            get
            {
                return _transform;
            }
            set
            {
                _transform = value;
            }
        }
        private TransformComponent _parentTransform = null;
        public TransformComponent ParentTransform
        {
            get
            {
                return _parentTransform;
            }
            set
            {
                _parentTransform = value;
                ParentOffset = Transform;
            }
        }
        public Matrix4 ParentOffset { get; set; }
        public Vector3 Pivot { get; set; } = new Vector3(-1f, 0f, 0f);

        public TransformationPool _transformationPool = new TransformationPool();

        public TransformComponent(GameObject gameObject)
            : base(gameObject)
        {
            Transform = Matrix4.Identity;
            _transformationPool = new TransformationPool();
        }

        public TransformComponent(GameObject gameObject, Matrix4 transform, TransformComponent parentTransform, Vector3 pivot)
            : base(gameObject)
        {
            Transform = transform;
            ParentTransform = parentTransform;
            Pivot = pivot;
            _transformationPool = new TransformationPool();
            _transformationPool.WritePool(Transform);
        }

        public override void OnUpdateFrame()
        {
            base.OnUpdateFrame();
            _transformationPool.WritePool(Transform);
        }

        /*public void Translate(Vector3 translation)
        {
            var temp = Transform;
            Transform = Transform.ClearRotation();
            Transform = Transform * Matrix4.CreateTranslation(translation);
            RotateOperation(temp.ExtractRotation().Xyz);
            //SetPosition(GetPosition() + translation);
            //var temp = Transform;
            //Transform = Matrix4.Identity * Matrix4.CreateTranslation(GetPosition() + translation);
            //RotateOperation(temp.ExtractRotation().Xyz);
            //Transform = Transform * Matrix4.CreateScale(temp.ExtractScale());
            //SetScale(temp.ExtractScale());
            //Rotate(temp.ExtractRotation().Xyz);
        }

        public void Rotate(Vector3 rotation)
        {
            var temp = Transform;
            Transform = Transform.ClearTranslation();
            RotateOperation(rotation);           
            Transform = Transform * Matrix4.CreateTranslation(temp.ExtractTranslation());
            //SetRotation(GetRotation().Xyz + Quaternion.FromEulerAngles(rotation).Xyz);
        }

        public void SetScale(Vector3 scale)//++
        {
            var temp = Transform;
            Transform = Matrix4.Identity * Matrix4.CreateScale(scale);
            Translate(temp.ExtractTranslation());
            Rotate(temp.ExtractRotation().Xyz);
        }

        public void SetPosition(Vector3 position)//++
        {
            var temp = Transform;
            Transform = Matrix4.Identity * Matrix4.CreateTranslation(position);
            SetScale(temp.ExtractScale());
            Rotate(temp.ExtractRotation().Xyz);
        }

        public void SetRotation(Vector3 rotation)//++
        {
            var temp = Transform;
            //Transform = Matrix4.Identity;
            Transform = Transform.ClearTranslation();
            Transform = Transform.ClearRotation();
            RotateOperation(rotation);
            Transform = Transform * Matrix4.CreateTranslation(temp.ExtractTranslation());
            //Translate(temp.ExtractTranslation());
            //SetScale(temp.ExtractScale());
        }*/

        /*public void SetPosition(Vector3 position)
        {
            _transformationPool.Translation = Matrix4.Identity * Matrix4.CreateTranslation(position);
        }

        public void SetRotation(Vector3 rotation)
        {
            _transformationPool.Rotation = Matrix4.Identity * Matrix4.CreateRotationX(rotation.X)
                                                            * Matrix4.CreateRotationY(rotation.Y)
                                                            * Matrix4.CreateRotationZ(rotation.Z);
        }

        public void SetScale(Vector3 scale)
        {
            _transformationPool.Scale = Matrix4.Identity * Matrix4.CreateScale(scale);
        }

        public void Translate(Vector3 translation)
        {
            _transformationPool.Translation = Matrix4.Identity * Matrix4.CreateTranslation(_transformationPool.Translation.ExtractTranslation() + translation);
        }

        public void Rotate(Vector3 rotation)
        {
            _transformationPool.Rotation = _transformationPool.Rotation * Matrix4.CreateRotationX(rotation.X)
                                                                        * Matrix4.CreateRotationY(rotation.Y)
                                                                        * Matrix4.CreateRotationZ(rotation.Z);
        }
       
        public Vector3 GetPosition()
        {
            //return Transform.ExtractTranslation();
            return _transformationPool.Translation.ExtractTranslation();
        }

        public Quaternion GetRotation()
        {
            //return Transform.ExtractRotation(false);
            return _transformationPool.Rotation.ExtractRotation(false);
        }

        public Vector3 GetScale()
        {
            //return Transform.ExtractScale();
            return _transformationPool.Scale.ExtractScale();
        }

        public void PoolToTransform()
        {
            Transform = _transformationPool.GetTransformationMatrix();
        }

        /*public void ApplyParentTransform()///---
        {
            if (_parentTransform != null)
            {
                /*var temp = Transform;
                Transform = Matrix4.Identity + _parentTransform.Transform;
                Translate(temp.ExtractTranslation());
                Rotate(temp.ExtractRotation().Xyz);
                SetScale(temp.ExtractScale());

                var temp = Transform;
                Transform = Matrix4.Identity;
                SetPosition(ParentTransform.GetPosition() + ParentOffset.ExtractTranslation());
                SetRotation((ParentTransform.GetRotation() + ParentOffset.ExtractRotation(false)).Xyz);
                SetScale(ParentOffset.ExtractScale());
                

            }

        }*/
        /*private void RotateOperation(Vector3 rotation)
        {
            Transform = Transform * Matrix4.CreateRotationX(rotation.X);
            Transform = Transform * Matrix4.CreateRotationY(rotation.Y);
            Transform = Transform * Matrix4.CreateRotationZ(rotation.Z);
        }*/

       
  //  }
//}
