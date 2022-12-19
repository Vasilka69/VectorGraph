using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VectorGraph
{
    interface IGrController
    {
        void Repaint();
        void SetPort(Graphics gr);//, int width, int height);

        SelectionStore SelStore { set; get; }

        GraphSystem gs { set; get; }
    }


    internal class Scene : IGrController
    {

        public GraphSystem gs { set; get; }

        public Store st;
        public SelectionStore SelStore { set; get; }

        public Scene(Graphics gr, Store st, IGrProperties GrProperties)
        {
            this.gs = new GraphSystem(gr, GrProperties);
            this.st = st;
        }

        public void Repaint()
        {
            gs.gr.Clear(Color.White);
            if (st.Count != 0)
                foreach (GraphItem item in st)
                    item.Draw(gs);
            SelStore.Draw(gs);
            if (SelStore.SelectionRect != null)
                SelStore.SelectionRect.Draw(gs);
        }

        public void SetPort(Graphics gr)//, int width, int height)
        {
            gs.gr = gr;
            // width height
        }
    }
}
