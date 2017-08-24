using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gware.Gaming.Common.Noise
{
    public class NoiseGenerator
    {
        private ImplicitFractal m_fractal;

        public NoiseGenerator(int seed,FractalType fractalType, BasisType basisType, InterpolationType interpolationType,int octaves, double frequency)
        {
            m_fractal = new ImplicitFractal(fractalType, basisType, interpolationType)
            {
                Octaves = octaves,
                Frequency = frequency,
                Seed = seed
            };
        }
        public double[,] GenerateNoiseChunk(double xChunkOffset,double yChunkOffset, int chunkSize, double mapWidth, double mapHeight)
        {
            return GenerateWrappedNoiseChunk(m_fractal, xChunkOffset,yChunkOffset, chunkSize, mapWidth, mapHeight);
        }
        
        public double GetNoise(double x, double y, double width, double height)
        {
            return GetNoise(m_fractal, x, y, width, height);
        }

        public static double GetNoise(ImplicitFractal fractal,double x, double y,double width,double height)
        {
            //noise range
            double x1 = 0, x2 = 2;
            double y1 = 0, y2 = 2;
            double dx = x2 - x1;
            double dy = y2 - y1;

            // Sample noise at smaller intervals
            double s = x / width;
            double t = y / height;

            // Calculate our 4D coordinates
            double nx = x1 + System.Math.Cos(s * 2 * System.Math.PI) * dx / (2 * System.Math.PI);
            double ny = y1 + System.Math.Cos(t * 2 * System.Math.PI) * dy / (2 * System.Math.PI);
            double nz = x1 + System.Math.Sin(s * 2 * System.Math.PI) * dx / (2 * System.Math.PI);
            double nw = y1 + System.Math.Sin(t * 2 * System.Math.PI) * dy / (2 * System.Math.PI);

            return fractal.Get(nx, ny, nz, nw);
        }

        public static double[,] GenerateWrappedNoiseChunk(ImplicitFractal fractal, double xChunkOffset, double yChunkOffset, int chunkSize, double mapWidth, double mapHeight)
        {
            double[,] retVal = new double[chunkSize, chunkSize];
            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    retVal[x, y] = GetNoise(fractal,x + xChunkOffset,y +yChunkOffset,mapWidth,mapHeight);
                }
            }
            return retVal;
        }
        
    }
}