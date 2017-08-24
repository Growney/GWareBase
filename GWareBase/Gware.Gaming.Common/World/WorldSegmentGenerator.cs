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
    internal class WorldSegmentGenerator : ThreadBase
    {
        public int CurrentXChunk { get; private set; }
        public int CurrentYChunk { get; private set; }

        public int XChunks { get; private set; }
        public int YChunks { get; private set; }

        public Range<int> XChunkRange { get; private set; }
        public Range<int> YChunkRange { get; private set; }

        public int ChunkSize { get; private set; }

        private NoiseGenerator m_heightNoiseGenerator;
        private NoiseGenerator m_moistureNoiseGenerator;
        private NoiseGenerator m_temperatureNoiseGenerator;

        private double m_totalMapHeight;
        private double m_totalMapWidth;

        public event Action<WorldChunk> OnChunkCompleted;

        public WorldSegmentGenerator(int chunkSize,Range<int> xChunkRange, Range<int> yChunkRange, double totalMapWidth, double totalMapHeight, NoiseGenerator heightNoiseGenerator, NoiseGenerator moistureNoiseGenerator, NoiseGenerator temperatureNoiseGenerator)
            : base(1)
        {
            ChunkSize = chunkSize;

            XChunkRange = xChunkRange;
            YChunkRange = yChunkRange;

            XChunks = (xChunkRange.End - xChunkRange.Start) + 1;// Plus one because they are index values
            YChunks = (yChunkRange.End - yChunkRange.Start) + 1;

            CurrentXChunk = XChunkRange.Start;
            CurrentYChunk = YChunkRange.Start;

            m_totalMapWidth = totalMapWidth;
            m_totalMapHeight = totalMapHeight;

            m_heightNoiseGenerator = heightNoiseGenerator;
            m_moistureNoiseGenerator = moistureNoiseGenerator;
            m_temperatureNoiseGenerator = temperatureNoiseGenerator;
        }

        private WorldTile GenerateTile(int x, int y, int xChunk, int yChunk)
        {
            double xVal = x + (ChunkSize * (xChunk));
            double yVal = y + (ChunkSize * (yChunk));

            double heightValue = m_heightNoiseGenerator.GetNoise(xVal, yVal, m_totalMapWidth, m_totalMapHeight);
            double moistureValue = m_moistureNoiseGenerator.GetNoise(xVal, yVal, m_totalMapWidth, m_totalMapHeight);
            double temperatureValue = m_moistureNoiseGenerator.GetNoise(xVal, yVal, m_totalMapWidth, m_totalMapHeight);

            return new WorldTile(new LargeVector2(x, y), heightValue, moistureValue, temperatureValue);

        }

        private void GenerateChunk(int chunkX, int chunkY)
        {
            WorldChunk currentChunk = new WorldChunk(new LargeVector2(ChunkSize * chunkX, ChunkSize * chunkY), ChunkSize);
            int chunkSizeIndex = ChunkSize - 1;
            for (int x = 0; x < ChunkSize / 2; x++)
            {
                for (int y = 0; y < ChunkSize / 2; y++)
                {
                    //bottomLeft
                    currentChunk.SetTile(x, y, GenerateTile(x, y, chunkX, chunkY));
                    //topleft
                    currentChunk.SetTile(x, chunkSizeIndex - y, GenerateTile(x, chunkSizeIndex - y, chunkX, chunkY));
                    //top right
                    currentChunk.SetTile(chunkSizeIndex - x, chunkSizeIndex - y, GenerateTile(chunkSizeIndex - x, chunkSizeIndex - y, chunkX, chunkY));
                    //bottom right
                    currentChunk.SetTile(chunkSizeIndex - x, y, GenerateTile(chunkSizeIndex - x, y, chunkX, chunkY));
                }
            }
            OnChunkCompleted?.Invoke(currentChunk);
        }

        protected override void ExecuteSingleThreadCycle()
        {
            GenerateChunk(CurrentXChunk, CurrentYChunk); // Bottom Left
            GenerateChunk(XChunkRange.End - (CurrentXChunk - XChunkRange.Start), CurrentYChunk); // Bottom Right
            GenerateChunk(CurrentXChunk, YChunkRange.End - (CurrentYChunk - YChunkRange.Start)); // Top Left
            GenerateChunk(XChunkRange.End - (CurrentXChunk - XChunkRange.Start), YChunkRange.End - (CurrentYChunk - YChunkRange.Start)); //Top Right

            ///Update chunk and stop if completed
            CurrentXChunk++;
            int xRelativeChunkIndex = CurrentXChunk - XChunkRange.Start;
            if (xRelativeChunkIndex > XChunks / 2)
            {
                CurrentYChunk++;
                int yRelativeChunkIndex = CurrentYChunk - YChunkRange.Start;
                if (yRelativeChunkIndex > YChunks / 2)
                {
                    Stop();
                }
                else
                {
                    CurrentXChunk = XChunkRange.Start;
                }
            }
        }

        public override string ToString()
        {
            return String.Format("{0},{1}", XChunkRange, YChunkRange);
        }
    }
}
