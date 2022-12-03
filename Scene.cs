using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorGraph
{
    internal class Scene
    {
        public GraphSystem gs;

        public Store st;

        public Scene(Graphics gr, Store st)
        {
            this.gs = new GraphSystem(gr);
            this.st = st;
        }

        public void Repaint()
        {
            //gs.gr.Clear(Color.White);
            if (st.Count != 0)
                foreach (Figure f in st)
                    gs.DrawFigure(f);
            //DrawFigure(CurrFigure);
        }
    }
}
