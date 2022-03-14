using System;
using System.Collections.Generic;
using System.Text;
using PotatoUtils.ObjToJson;
using System.IO;

namespace PotatoEngine
{
    public static class WavefrontParser
    {
        public static Model3D CreateNewPrimitive(string path)
        {
            PrimitiveObject tempObject = WavefrontConverter.WavefrontDeserialize(path);
            Model3D tempPrimitive = new Model3D(tempObject.Vertices, tempObject.Indices, tempObject.UVCoords);
            return tempPrimitive;
        }
        public static Model3D CreateNewPrimitiveFromJson(string path)
        {
            PrimitiveObject tempObject = WavefrontConverter.GetObectFromJson(File.ReadAllText(path));
            Model3D tempPrimitive = new Model3D(tempObject.Vertices, tempObject.Indices, tempObject.UVCoords);
            return tempPrimitive;
        }
    }
}
