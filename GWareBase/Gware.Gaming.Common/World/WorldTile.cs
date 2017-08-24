using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Gaming.Common.World
{
    public class WorldTile
    {
        public double Height { get; set; }
        public double Temperature { get; set; }
        public double Moisture { get; set; }
        public LargeVector2 Position { get; set; }

        public WorldTile(LargeVector2 position,double height, double temperature, double moisture)
        {
            Position = position;
            Height = height;
            Temperature = temperature;
            Moisture = moisture;
        }
    }
}
