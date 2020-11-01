using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{

    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class RangeExAttribute : PropertyAttribute
    {
        public readonly int min;
        public readonly int max;
        public readonly int step;

        public RangeExAttribute(int min, int max, int step)
        {
            this.min = min;
            this.max = max;
            this.step = step;
        }
    }
}
