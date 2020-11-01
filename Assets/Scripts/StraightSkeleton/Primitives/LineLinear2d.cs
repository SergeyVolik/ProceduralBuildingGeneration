using System;

namespace StraightSkeleton.Primitives
{
    /// <summary>
    ///     Geometry line in linear form. General form:
    ///     Ax + By + C = 0;
    ///     <see href="http://en.wikipedia.org/wiki/Linear_equation"/>
    /// </summary>
    public struct LineLinear2d
    {
        public static LineLinear2d Empty = new LineLinear2d();
        #region Variables
        public double A;
        public double B;
        public double C;

        public Vector2d direction;
        public Vector2d normal;

        #endregion
        #region Constructos
        /// <summary> Linear line from two points on line. </summary>
        public LineLinear2d(Vector2d pP1, Vector2d pP2)
        {
            A = pP1.Y - pP2.Y;
            B = pP2.X - pP1.X;
            C = pP1.X*pP2.Y - pP2.X*pP1.Y;
            direction = new Vector2d(-B, A);
            normal = new Vector2d(A, B);
        }

        /// <summary> Linear line. </summary>
        public LineLinear2d(double pA, double pB, double pC)
        {
            A = pA;
            B = pB;
            C = pC;
            direction = new Vector2d(-B, A);
            normal = new Vector2d(A, B);
        }
        public static LineLinear2d OrthogonalLineFromOtherLineDirection(Vector2d direction, Vector2d point)
        {
            double A, B, C;

            A = direction.X;
            B = direction.Y;
            C = -point.X * A + -point.Y * B;

            return new LineLinear2d(A, B, C);
        }
        public static LineLinear2d OrthogonalLineFromOtherLineNormal(Vector2d normal, Vector2d point)
        {
            double A, B, C;

            A = -normal.Y;
            B = normal.X;
            C = -point.X * A + -point.Y * B;

            return new LineLinear2d(A, B, C);
        }
        #endregion
        #region Collide specific
        /// <summary> Collision point of two lines. </summary>
        /// <param name="pLine">Line to collision.</param>
        /// <returns>Collision point.</returns>
        public Vector2d Collide(LineLinear2d pLine)
        {
            return Collide(this, pLine);
        }

        /// <summary> Collision point of two lines. </summary>
        public static Vector2d Collide(LineLinear2d pLine1, LineLinear2d pLine2)
        {
            return Collide(pLine1.A, pLine1.B, pLine1.C, pLine2.A, pLine2.B, pLine2.C);
        }
        /// <summary> Collision point of two lines. </summary>
        public static Vector2d Collide(double A1, double B1, double C1, double A2, double B2, double C2)
        {
            var WAB = A1 * B2 - A2 * B1;
            var WBC = B1 * C2 - B2 * C1;
            var WCA = C1 * A2 - C2 * A1;

            return WAB == 0 ? Vector2d.Empty : new Vector2d(WBC / WAB, WCA / WAB);
        }
        #endregion 
        #region Orthogonal specific
        /// <summary> Checking orthogonal of lines. </summary>
        /// <param name="pLine1">Line 1.</param>
        /// <param name="pLine2">Line 2.</param>
        /// <returns>True if line is Orthogonal.</returns>
        public static bool Orthogonal(LineLinear2d pLine1, LineLinear2d pLine2)
        {
            if (Math.Abs(pLine1.direction.X*pLine2.direction.X + pLine1.direction.Y * pLine2.direction.Y) < Epsilon)
                return true;
            return false;

        }
        /// <summary> Checking orthogonal of lines. </summary>
        public bool Orthogonal(LineLinear2d pLine1)
        {
            if (Math.Abs(pLine1.direction.X * direction.X + pLine1.direction.Y * direction.Y) < Epsilon)
                return true;
            return false;

        }
        #endregion
        #region Colinear specific
        /// <summary> Checking colinear of lines. </summary>
        /// <param name="pLine1">Line 1.</param>
        /// <param name="pLine2">Line 2.</param>
        /// <returns>True if line is colinear.</returns>
        public static bool Colinear(LineLinear2d pLine1, LineLinear2d pLine2)
        {
            if (Math.Abs(pLine1.A * pLine2.B - pLine2.A * pLine1.B)< Epsilon)
                return true;

            return false;
        }
        /// <summary> Checking colinear of lines. </summary>
        public bool Colinear(LineLinear2d pLine1)
        {
            if (Math.Abs(pLine1.A * B - A * pLine1.B) < Epsilon)
                return true;

            return false;
        }
        #endregion
        #region Contains specific
        const double Epsilon = 0.1e-4;
        /// <summary> Check whether point belongs to line. </summary>
        public bool Contains(Vector2d point)
        {         
            return Math.Abs((point.X * A + point.Y * B + C)) < Epsilon;
        }
        #endregion

        public double GetX(double Y)
        {           
            if (A != 0)
                return (Y * B + C) / A * -1;
            else return  (Y * B + C) * -1;
           
        }
        public double GetY(double X)
        {
            if(B != 0)
                return (X * A + C) / B * -1;
            else return (X * A + C) * -1;
        }
        public static bool operator ==(LineLinear2d left, LineLinear2d right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(LineLinear2d left, LineLinear2d right)
        {
            return !left.Equals(right);
        }
    }
}