using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VectorGraph
{
    delegate void Repaint();

    interface IFactory
    {
        event Repaint RepaintEvent;
        //PropList pl { set; get; }
        IGrProperties GrProperties { set; get; }
        FigureType ChoosenFigure { get; set; }
        GraphItem AddFigure(int x, int y);
        void CreateAndGrabItem(int x, int y);
        ISelections selController { get; set; } // Потом убрать
    }

    internal class Factory : IFactory
    {
        //public PropList pl { set; get; }

        public event Repaint RepaintEvent;
        public IGrProperties GrProperties { set; get; }
        public FigureType ChoosenFigure { get; set; }
        public ISelections selController { get; set; } // потом убрать?

        Store st;

        public Factory(Store st, IGrProperties GrProperties)//PropList pl)
        {
            ChoosenFigure = FigureType.Line;
            this.st = st;

            ContourProps cp = new ContourProps(GrProperties.Contour.Color, GrProperties.Contour.LineWidth);
            FillProps fp = new FillProps(GrProperties.Fill.Color);
            PropList pl = new PropList(cp, fp);
            this.GrProperties = new GrPropChannel(pl);

            //this.pl = pl;
            selController = new SelectionController();
        }

        public GraphItem AddFigure(int x, int y)
        {
            Figure f;
            Frame frame = new Frame(x, y, x, y);
            PropList pl = new PropList(GrProperties.Contour as ContourProps, GrProperties.Fill as FillProps);
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
            RepaintEvent?.Invoke();
            return f;
        }

        public void CreateAndGrabItem(int x, int y)
        {
            GraphItem item = AddFigure(x, y);
            st.Add(item);
            selController.SelectAndGrab(item, x, y);
        }
    }
}
