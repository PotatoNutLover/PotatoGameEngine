using System;
using OpenTK.Mathematics;

namespace PotatoEngine
{
    public class Camera
    {
        private Vector3 _front = -Vector3.UnitZ;
        private Vector3 _up = Vector3.UnitY;
        private Vector3 _right = Vector3.UnitX;

        private float _pitch;
        private float _yaw = -MathHelper.PiOver2;
        private float _fov = MathHelper.PiOver2;

        public CameraSettings CameraSettings { get; private set; }

        public Vector3 Position { get; set; }
        public float AspectRatio { get; set; }

        public Vector3 Front
        {
            get
            {
                return _front;
            }
        }
        public Vector3 Up
        {
            get
            {
                return _up;
            }
        }
        public Vector3 Right
        {
            get
            {
                return _right;
            }
        }

        public float Pitch
        {
            get
            {
                return MathHelper.RadiansToDegrees(_pitch);
            }
            set
            {
                var angle = MathHelper.Clamp(value, CameraSettings.PitchLimits.X, CameraSettings.PitchLimits.Y); 
                _pitch = MathHelper.DegreesToRadians(angle);
                UpdateVectors();
            }
        }
        public float Yaw
        {
            get
            {
                return MathHelper.RadiansToDegrees(_yaw);
            }
            set
            {
                _yaw = MathHelper.DegreesToRadians(value);
                UpdateVectors();
            }
        }
        public float Fov
        {
            get
            {
                return MathHelper.RadiansToDegrees(_fov);
            }
            set
            {
                var angle = MathHelper.Clamp(value, CameraSettings.FovLimits.X, CameraSettings.FovLimits.Y); 
                _fov = MathHelper.DegreesToRadians(angle);
            }
        }

        public Camera(Vector3 position, float aspectRatio, CameraSettings cameraSettings)
        {
            CameraSettings = cameraSettings;
            Position = position;
            AspectRatio = aspectRatio;
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Position + _front, _up);
        }

        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(_fov, AspectRatio, CameraSettings.DepthLimits.X, CameraSettings.DepthLimits.Y);
        }

        private void UpdateVectors()
        {
            _front.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
            _front.Y = MathF.Sin(_pitch);
            _front.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);

            _front = Vector3.Normalize(_front);

            _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
            _up = Vector3.Normalize(Vector3.Cross(_right, _front));
        }



    }
}
