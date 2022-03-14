using System;
using System.Collections.Generic;
using System.Text;

namespace PotatoEngine
{
    public struct Collision
    {
        public List<ColliderComponent> ColliderComponents { get; set; }
        public bool Detected { get; set; }
    }
}
