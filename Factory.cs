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

        //Frame frame { set; get; } // по идее щас не нужен

        event Repaint RepaintEvent;
        FigureType ChoosenFigure { get; set; }
        GraphItem AddFigure(int x, int y);
        void CreateAndGrabItem(int x, int y);
        //void AddCurrFigure();
        SelectionController selController { get; set; } // Потом убрать
    }

    internal class Factory : IFactory
    {
        public PropList pl { set; get; }

        //public Frame frame { set; get; } // по идее щас не нужен

        public event Repaint RepaintEvent;
        public FigureType ChoosenFigure { get; set; }

        Store st;

        public SelectionController selController { get; set; } // потом убрать

        public Factory(Store st, PropList pl)
        {
            // frame = new Frame(0, 0, 0, 0); // по идее щас не нужен
            ChoosenFigure = FigureType.Line;
            this.st = st;
            this.pl = pl;
            selController = new SelectionController();
            // 
            //AddFigure();
            //
            //this.scene = scene;
        }

        public GraphItem AddFigure(int x, int y)
        {
            Figure f;
            Frame frame = new Frame(x, y, x, y);
            switch (ChoosenFigure)
            {
                case FigureType.Line:
                    f = new Line(frame, pl.Clone());
                    break;
                case FigureType.Rect:
                    f = new Rect(frame, pl.Clone());
                    break;
                case FigureType.Ellipse:
                    f = new Ellipse(frame, pl.Clone());
                    break;
                default:
                    f = null;
                    break;
            }
            //st.Add(f);
            RepaintEvent?.Invoke();
            return f;
        }

        public void CreateAndGrabItem(int x, int y)
        {
            GraphItem item = AddFigure(x, y);
            st.Add(item);
            selController.SelectAndGrab(item, x, y);
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
