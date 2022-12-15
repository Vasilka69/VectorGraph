using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VectorGraph
{
    interface IGrController
    {
        void Repaint();
        void SetPort(Graphics gr);//, int width, int height);
    }

    internal class Scene : IGrController
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
            gs.gr.Clear(Color.White);
            if (st.Count != 0)
                foreach (Figure f in st)
                    f.Draw(gs);
                    //gs.DrawFigure(f);
            //DrawFigure(CurrFigure);
        }

        public void SetPort(Graphics gr)//, int width, int height)
        {
            gs.gr = gr;
            // width height
        }
    }
}
