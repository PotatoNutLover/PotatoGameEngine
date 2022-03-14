using OpenTK.Mathematics;

namespace PotatoEngine
{
    public class CameraSettings
    {
        public Vector2 PitchLimits { get; set; }
        public Vector2 FovLimits { get; set; }
        public Vector2 DepthLimits { get; set; }

        public CameraSettings()
        {
            PitchLimits = new Vector2(-89f, 89f);
            FovLimits = new Vector2(1f, 45f);
            DepthLimits = new Vector2(0.01f, 100f);
        }
    }
}
