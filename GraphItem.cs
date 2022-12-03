using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorGraph
{
    internal abstract class GraphItem
    {
        //private protected Frame fr { get; }
        public Frame frame { get; }

        public GraphItem(Frame fr)
        {
            frame = fr;
        }

        public abstract void Draw(GraphSystem gs);
    }
}
