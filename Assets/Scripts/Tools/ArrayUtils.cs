using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Tools
{
    public static class ArrayUtils
    {
        public static int[] FindIndex(this Array haystack, object needle)
        {
            if (haystack.Rank == 1)
                return new[] { Array.IndexOf(haystack, needle) };

            var found = haystack.OfType<object>()
                              .Select((v, i) => new { v, i })
                              .FirstOrDefault(s => s.v.Equals(needle));
            if (found == null)
                throw new Exception("needle not found in set");

            var indexes = new int[haystack.Rank];
            var last = found.i;
            var lastLength = Enumerable.Range(0, haystack.Rank)
                                       .Aggregate(1,
                                           (a, v) => a * haystack.GetLength(v));
            for (var rank = 0; rank < haystack.Rank; rank++)
            {
                lastLength = lastLength / haystack.GetLength(rank);
                var value = last / lastLength;
                last -= value * lastLength;

                var index = value + haystack.GetLowerBound(rank);
                if (index > haystack.GetUpperBound(rank))
                    throw new IndexOutOfRangeException();
                indexes[rank] = index;
            }

            return indexes;
        }
        public static (int i,int j) CoordinatesOf<T>(this T[,] matrix, T value)
        {
            int w = matrix.GetLength(0); // width
            int h = matrix.GetLength(1); // height

            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    if (matrix[x, y].Equals(value))
                        return (x, y);
                }
            }

            return (-1, -1);
        }
    }
}
