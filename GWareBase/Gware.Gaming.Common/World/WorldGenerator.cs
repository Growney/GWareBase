using Gware.Common.DataStructures;
using Gware.Common.Threading;
using Gware.Gaming.Common.Noise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Gaming.Common.World
{
    public class WorldGenerator
    {
        private Random m_random;

        public int ChunkSize { get; private set; }

        public long Width { get; private set; }
        public long Height { get; private set; }

        public int XChunks { get; private set; }
        public int YChunks { get; private set; }

        public event Action<WorldChunk> OnChunkCompleted;

        public bool IsRunning
        {
            get
            {
                return m_bottomLeftGenerator.IsRunning || m_bottomRightGenerator.IsRunning || m_topLeftGenerator.IsRunning || m_topRightGenerator.IsRunning;
            }
        }

        private WorldSegmentGenerator m_bottomLeftGenerator;
        private WorldSegmentGenerator m_topLeftGenerator;
        private WorldSegmentGenerator m_bottomRightGenerator;
        private WorldSegmentGenerator m_topRightGenerator;


        public WorldGenerator(int seed, int chunkSize,long minSize, long maxSize)
        {
            ChunkSize = chunkSize;

            m_random = new Random(seed);

            Width = (long)(m_random.NextDouble() * (maxSize - minSize) + minSize);
            Height = (long)(m_random.NextDouble() * (maxSize - minSize) + minSize);

            XChunks = (int)(Width / ChunkSize);
            YChunks = (int)(Height / ChunkSize);

            XChunks += XChunks % 2;
            YChunks += YChunks % 2;

            Width = XChunks * ChunkSize;
            Height = YChunks * ChunkSize;


            NoiseGenerator heightNoiseGenerator = new NoiseGenerator(m_random.Next(), FractalType.RidgedMulti, BasisType.Simplex, InterpolationType.Cubic, 25, 1);
            NoiseGenerator moistureNoiseGenerator = new NoiseGenerator(m_random.Next(), FractalType.RidgedMulti, BasisType.Simplex, InterpolationType.Cubic, 25, 1);
            NoiseGenerator temperatureNoiseGenerator = new NoiseGenerator(m_random.Next(), FractalType.RidgedMulti, BasisType.Simplex, InterpolationType.Cubic, 25, 1);

            m_bottomLeftGenerator = new WorldSegmentGenerator(ChunkSize,new Range<int>(0, (XChunks / 2) - 1), new Range<int>(0, (YChunks / 2) - 1), Width, Height, heightNoiseGenerator, moistureNoiseGenerator, temperatureNoiseGenerator);
            m_topLeftGenerator = new WorldSegmentGenerator(ChunkSize,new Range<int>(0, (XChunks / 2) -1), new Range<int>(YChunks / 2, YChunks - 1), Width, Height, heightNoiseGenerator, moistureNoiseGenerator, temperatureNoiseGenerator);
            m_bottomRightGenerator = new WorldSegmentGenerator(ChunkSize,new Range<int>(XChunks / 2, XChunks - 1), new Range<int>(0, (YChunks / 2) - 1), Width, Height, heightNoiseGenerator, moistureNoiseGenerator, temperatureNoiseGenerator);
            m_topRightGenerator = new WorldSegmentGenerator(ChunkSize,new Range<int>(XChunks / 2, XChunks -1), new Range<int>(YChunks / 2, YChunks -1), Width, Height, heightNoiseGenerator, moistureNoiseGenerator, temperatureNoiseGenerator);

            m_bottomLeftGenerator.OnChunkCompleted += InternalChunkCompleted;
            m_topLeftGenerator.OnChunkCompleted += InternalChunkCompleted;
            m_bottomRightGenerator.OnChunkCompleted += InternalChunkCompleted;
            m_topRightGenerator.OnChunkCompleted += InternalChunkCompleted;
        }

        private void InternalChunkCompleted(WorldChunk obj)
        {
            OnChunkCompleted?.Invoke(obj);
        }

        public void Start()
        {
            m_bottomLeftGenerator.Start();
            m_topLeftGenerator.Start();
            m_bottomRightGenerator.Start();
            m_topRightGenerator.Start();
        }

        public void Stop()
        {
            m_bottomLeftGenerator.Stop();
            m_topLeftGenerator.Stop();
            m_bottomRightGenerator.Stop();
            m_topRightGenerator.Stop();
        }
        
    }
}
