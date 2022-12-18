using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorGraph
{
    internal class Frame
    {
        public List<int> coords;

        public Frame(int x1, int y1, int x2, int y2)
        {
            coords = new List<int>() { x1, y1, x2, y2 };
        }

        public static Frame FrameSum(List<Frame> frames) // работает
        {
            int minX = frames[0].coords[0];
            int maxX = frames[0].coords[2];
            int minY = frames[0].coords[1];
            int maxY = frames[0].coords[3];
            foreach (Frame frame in frames)
                for (int coord = 0; coord < frame.coords.Count; coord++) {
                    if (coord % 2 == 0) // X
                    {
                        minX = Math.Min(minX, frame.coords[coord]);
                        maxX = Math.Max(maxX, frame.coords[coord]);
                    }
                    if (coord % 2 == 1) // Y
                    {
                        minY = Math.Min(minY, frame.coords[coord]);
                        maxY = Math.Max(maxY, frame.coords[coord]);
                    }
                }
            Frame fr = new Frame(minX, minY, maxX, maxY);
            return fr;
        }

        public Frame Clone()
        {
            Frame frame = new Frame(this.coords[0], this.coords[1], this.coords[2], this.coords[3]);
            return frame;
        }
    }
}
