using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorGraph
{
    internal class Factory
    {
        Store st;
        public PropList pl;

        public Frame frame;
        public FigureType ChoosenFigure { get; set; }

        public delegate void Repaint();
        public event Repaint RepintEvent;

        public Factory(Store st, PropList pl)
        {
            this.st = st;
            this.pl = pl;
            ChoosenFigure = FigureType.Line;
            //this.scene = scene;
            frame = new Frame(0, 0, 0, 0);
        }

        public void AddFigure()
        {
            switch (ChoosenFigure)
            {
                case FigureType.Line:
                    st.Add(new Line(frame.Clone(), pl.Clone()));
                    break;
                case FigureType.Rect:
                    st.Add(new Rect(frame.Clone(), pl.Clone()));
                    break;
            }
            RepintEvent?.Invoke();
            //scene.Repaint();
            //DrawFigures();
        }
    }
}
