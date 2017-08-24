using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Gaming.Common.World
{
    public class WorldChunk : IEnumerable<WorldTile>
    {
        public int ChunkSize { get; private set; }
        public WorldTile[,] Tiles { get; }
        public LargeVector2 Position { get; set; }

        public WorldChunk(LargeVector2 position,int chunkSize)
        {
            ChunkSize = chunkSize;
            Position = position;
            Tiles = new WorldTile[chunkSize, chunkSize];
        }

        public void SetTile(int x,int y,WorldTile worldTile)
        {
            Tiles[x, y] = worldTile;
        }

        public WorldTile GetTile(int x,int y)
        {
            return Tiles[x, y];
        }

        public IEnumerator<WorldTile> GetEnumerator()
        {
            for (int x = 0; x < ChunkSize; x++)
            {
                for (int y = 0; y < ChunkSize; y++)
                {
                    yield return Tiles[x, y];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
