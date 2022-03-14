using System;
using System.Collections.Generic;
using System.Text;

namespace PotatoEngine
{
    public class Component
    {
        public GameObject gameObject { get; private set; }

        public Component(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public virtual void OnLoad()
        {

        }

        public virtual void OnRenderFrame()
        {

        }

        public virtual void OnUpdateFrame()
        {

        }

        public virtual void OnDestroy()
        {

        }

        public virtual void OnUnload()
        {
            gameObject = null;
        }

        public virtual Component Clone(GameObject gameObject)
        {
            return new Component(gameObject);
        }
    }
}
