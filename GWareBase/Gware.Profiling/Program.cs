using Gware.Gaming.Common.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Profiling
{
    class Program
    {
        public static void Main(string[] args)
        {
            WorldGenerator m_generator = new WorldGenerator(22, 16,7000,10000);
            m_generator.OnChunkCompleted += OnChunkGenerated;
            m_generator.Start();

            while (m_generator.IsRunning)
            {

            }
            
        }
        private static void OnChunkGenerated(WorldChunk obj)
        {
            Console.WriteLine(String.Format("Chunk Completed at {0:0},{1:0}", obj.Position.X, obj.Position.Y));
        }
    }
}
