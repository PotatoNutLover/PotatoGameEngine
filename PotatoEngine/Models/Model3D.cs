using System;
using System.Collections.Generic;
using System.Text;

namespace PotatoEngine
{
    public class Model3D
    {
        private float[] _vertices = new float[0];
        public float[] Vertices
        {
            get
            {
                return _vertices;
            }
            private set
            {
                _vertices = value;
            }
        }
        private uint[] _indices = new uint[0];
        public uint[] Indices
        {
            get
            {
                return _indices;
            }
            private set
            {
                _indices = value;
            }
        }
        private float[] _uvCoords = new float[0];
        public float[] UVCoords
        {
            get
            {
                return _uvCoords;
            }
            private set
            {
                _uvCoords = value;
            }
        }

        public Model3D(float[] Vertices, uint[] Indices, float[] Uvs)
        {
            this.Vertices = Vertices;
            this.Indices = Indices;
            this.UVCoords = Uvs;
        }

        public Model3D Clone()
        {
            return new Model3D(
                this.Vertices.Clone() as float[],
                this.Indices.Clone() as uint[],
                this.UVCoords.Clone() as float[]);
        }

        public void ScaleUV(float value)
        {
            for(int i = 0; i < _uvCoords.Length; i++)
            {
                _uvCoords[i] = _uvCoords[i] * value;
            }
        }

        public void Unload()
        {
            _vertices = null;
            _indices = null;
            _uvCoords = null;
        }
    }
}
