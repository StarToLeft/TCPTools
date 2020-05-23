using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace TCPTools
{
    public class Vector3D
    {
        public Vector3D(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3D()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
        }

        public Vector3D vectorFromString(string vector)
        {
            Vector3D vector3D = new Vector3D();

            vector = vector.Replace("3D<", "");
            vector = vector.Replace(">", "");

            string[] vector3DString = vector.Split(",");
            if (vector3DString.Length < 3) return null;

            vector3D.x = float.Parse(vector3DString[0]);
            vector3D.y = float.Parse(vector3DString[1]);
            vector3D.z = float.Parse(vector3DString[2]);

            return vector3D;
        }

        public string vectorToString()
        {
            string vector = "3D<";

            vector += this.x + ",";
            vector += this.y + ",";
            vector += this.z;

            vector += ">";
            return vector;
        }

        public float x;
        public float y;
        public float z;
    }

    public class Vector2D
    {
        public Vector2D(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2D()
        {
            this.x = 0;
            this.y = 0;
        }

        public Vector2D vectorFromString(string vector)
        {
            Vector2D vector2D = new Vector2D();

            vector = vector.Replace("2D<", "");
            vector = vector.Replace(">", "");

            string[] vector3DString = vector.Split(",");
            if (vector3DString.Length < 2) return null;

            vector2D.x = float.Parse(vector3DString[0]);
            vector2D.y = float.Parse(vector3DString[1]);

            return vector2D;
        }

        public string vectorToString()
        {
            string vector = "2D<";

            vector += this.x + ",";
            vector += this.y;

            vector += ">";
            return vector;
        }

        public float x;
        public float y;
    }
}
