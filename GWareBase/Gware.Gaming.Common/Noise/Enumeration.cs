using System;

namespace Gware.Gaming.Common.Noise
{
    public enum BasisType
    {
        Value,
        Gradient,
        GradientValue,
        Simplex,
        White
    }

    internal class Cache
    {
        public Double X = 0.00;

        public Double Y = 0.00;

        public Double Z = 0.00;

        public Double W = 0.00;

        public Double U = 0.00;

        public Double V = 0.00;

        public Boolean IsValid = false;

        public Double Value = 0.00;
    }

    internal class CellularCache
    {
        public Double X = 0.00;

        public Double Y = 0.00;

        public Double Z = 0.00;

        public Double W = 0.00;

        public Double U = 0.00;

        public Double V = 0.00;

        public Boolean IsValid = false;

        public Double[] F = new Double[4];

        public Double[] D = new Double[4];
    }

    public enum CombinerType
    {
        Add,
        Multiply,
        Max,
        Min,
        Average
    }
    public enum FractalType
    {
        FractionalBrownianMotion,
        RidgedMulti,
        Billow,
        Multi,
        HybridMulti
    }

    public enum InterpolationType
    {
        None,
        Linear,
        Cubic,
        Quintic
    }

    public enum MappingMode
    {
        SeamlessNone,
        SeamlessX,
        SeamlessY,
        SeamlessZ,
        SeamlessXY,
        SeamlessXZ,
        SeamlessYZ,
        SeamlessXYZ
    }
}
