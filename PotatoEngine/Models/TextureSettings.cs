using OpenTK.Graphics.OpenGL4;
using System.Drawing;

namespace PotatoEngine
{
    public class TextureSettings
    {
        public bool RectModified { get; private set; }
        public Rectangle TextureRect
        {
            get
            {
                return TextureRect;
            }
            set
            {
                RectModified = true;
                TextureRect = value;
            }
        }
        public TextureMinFilter TextureMinFilter { get; set; }
        public TextureMagFilter TextureMagFilter { get; set; }
        public TextureWrapMode TextureWrapMode { get; set; }
        public RenderQueue RenderQueue { get; set; }

        public TextureSettings()
        {
            RectModified = false;
            TextureMinFilter = TextureMinFilter.Linear;
            TextureMagFilter = TextureMagFilter.Linear;
            TextureWrapMode = TextureWrapMode.Repeat;
            RenderQueue = RenderQueue.Opaque;
        }
        

    }
}
