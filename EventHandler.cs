using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorGraph
{
    interface IEventHandler
    {
        void LeftMouseUp(int x1, int y1, int x2, int y2);
    }
    internal class EventHandler : IEventHandler
    {
        public void LeftMouseUp(int x1, int y1, int x2, int y2)
        {

        }
    }
}
