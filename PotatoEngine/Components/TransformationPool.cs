using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;

namespace PotatoEngine
{
    public class TransformationPool
    {
        public Matrix4 Translation;
        public Matrix4 Rotation;
        public Matrix4 Scale;

        public TransformationPool()
        {
            Translation = Matrix4.Identity;
            Rotation = Matrix4.Identity;
            Scale = Matrix4.Identity;
        }

        public void WritePool(Matrix4 transform)
        {
            Translation = Matrix4.Identity * Matrix4.CreateTranslation(transform.ExtractTranslation());
            Rotation = Matrix4.Identity * Matrix4.CreateRotationX(transform.ExtractRotation().X)
                                * Matrix4.CreateRotationY(transform.ExtractRotation().Y)
                                * Matrix4.CreateRotationZ(transform.ExtractRotation().Z);
            Scale = Matrix4.Identity * Matrix4.CreateScale(transform.ExtractScale());
        }

        public Matrix4 GetTransformationMatrix()
        {
            Matrix4 transform;
            //transform = Translation * Rotation * Scale * Matrix4.Identity;
            transform = Matrix4.Identity * Scale * Rotation * Translation;
            return transform;
        }
    }
}
