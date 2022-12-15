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
        //void AddCurrFigure();
         SelectionController selController { get; set; } // Потом убрать
    }

    internal class Factory : IFactory
    {
        public PropList pl { set; get; }

        public Frame frame { set; get; }

        public event Repaint RepaintEvent;
        public FigureType ChoosenFigure { get; set; }

        Store st;

        public SelectionController selController { get; set; } // потом убрать

        public Factory(Store st, PropList pl)
        {
            frame = new Frame(0, 0, 0, 0);
            ChoosenFigure = FigureType.Line;
            this.st = st;
            this.pl = pl;
            // 
            AddFigure();
            //
            //this.scene = scene;
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
        }
        /*
        public void AddCurrFigure()
        {
            switch (ChoosenFigure)
            {
                case FigureType.Line:
                    if (st.Count == 0)
                        AddFigure();
                    else
                        st[0] = (new Line(frame.Clone(), pl.Clone()));
                    break;
                case FigureType.Rect:
                    if (st.Count == 0)
                        AddFigure();
                    else
                        st[0] = (new Rect(frame.Clone(), pl.Clone()));
                    break;
            }
            RepaintEvent?.Invoke();
            //scene.Repaint();
            //DrawFigures();
        }
        */
    }
}
