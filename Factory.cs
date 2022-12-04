using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorGraph
{
    delegate void Repaint();

    interface IFactory
    {
        PropList pl { set; get; }

        Frame frame { set; get; }

        event Repaint RepaintEvent;
        FigureType ChoosenFigure { get; set; }
        void AddFigure();
    }

    internal class Factory : IFactory
    {
        public PropList pl { set; get; }

        public Frame frame { set; get; }

        public event Repaint RepaintEvent;
        public FigureType ChoosenFigure { get; set; }

        Store st;

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
            RepaintEvent?.Invoke();
            //scene.Repaint();
            //DrawFigures();
        }
    }
}
