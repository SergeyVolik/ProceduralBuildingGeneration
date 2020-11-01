using StraightSkeleton.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Tools
{
    public static class Vector2dExtentions
    {
        public static Vector3 ToVector3(this Vector2d v, float y)
        {
            return new Vector3((float)v.X, y, (float)v.Y);
        }
    }
}
