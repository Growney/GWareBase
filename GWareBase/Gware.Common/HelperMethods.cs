using System;

namespace Gware.Common
{
    public static class HelperMethods
    {
        public static bool Overlap<T>(T firstStart, T firstEnd, T secondStart, T secondEnd) where T : IComparable<T>
        {
            //Error checking to ensure that the start is not greater than the end for any of the ranges
            if (firstStart.CompareTo(firstEnd) > 0)
                throw new ArgumentException("First start cannot be greater than first end.");
            if (secondStart.CompareTo(secondEnd) > 0)
                throw new ArgumentException("Second start cannot be greater than second end.");
            //All conditions require that the first range starts before the second ends and that it ends before it starts.
            if ((firstStart.CompareTo(secondEnd) <= 0) && (firstEnd.CompareTo(secondStart) > 0))
            {
                int startComparison = firstStart.CompareTo(secondStart);
                int endComparison = firstEnd.CompareTo(secondEnd);
                //Checking when the first range starts after the second and ends after it
                if ((startComparison >= 0) && (endComparison > 0))
                    return true;
                //Checking when the first range starts before the second and ends before the second.
                if ((startComparison < 0) && (endComparison <= 0))
                    return true;
                //Checking when the first range starts before and ends after the second
                if ((startComparison < 0) && (endComparison > 0))
                    return true;
                //Checking when the first range starts after the second and ends before the second. Or the two ranges are equal.k
                if ((startComparison >= 0) && (endComparison <= 0))
                    return true;
            }
            return false;
        }
    }
}
