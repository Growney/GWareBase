using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Gaming.Common
{
    public class LargeVector2
    {
        public double X { get; set; }
        public double Y { get; set; }


        public LargeVector2(double x, double y)
        {
            X = x;
            Y = y;
        }
        
        public static double VerticalWrappedDistance(LargeVector2 p1, LargeVector2 p2, double height)
        {
            if (p1.Y > p2.Y)
            {
                LargeVector2 temp = p1;
                p1 = p2;
                p2 = temp;
            }
            return Math.Sqrt(Math.Pow(p1.Y + (height - p2.Y), 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        public static double HorizontalWrappedDistance(LargeVector2 p1, LargeVector2 p2, double width)
        {
            if (p1.X > p2.X)
            {
                LargeVector2 temp = p1;
                p1 = p2;
                p2 = temp;
            }
            return Math.Sqrt(Math.Pow(p1.X + (width - p2.X), 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        public static double BothWrappedDistance(LargeVector2 p1, LargeVector2 p2, double width, double height)
        {
            return Math.Sqrt(Math.Pow(p1.X + (width - p2.X), 2) + Math.Pow(p1.Y + (height - p2.Y), 2));
        }

        public static double Distance(LargeVector2 p1, LargeVector2 p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

        public static double WrappedMinimumDistance(LargeVector2 p1, LargeVector2 p2, double width, double height)
        {
            return Math.Min(Math.Min(VerticalWrappedDistance(p1, p2, height), HorizontalWrappedDistance(p1, p2, width)), BothWrappedDistance(p1, p2, width, height));
        }

        public static double MinimumDistance(LargeVector2 p1, LargeVector2 p2, double width, double height)
        {
            return Math.Min(Distance(p1, p2), WrappedMinimumDistance(p1, p2, width, height));
        }
    }
}
