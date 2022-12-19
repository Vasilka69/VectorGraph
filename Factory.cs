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
        PropList pl { set; get; }
        FigureType ChoosenFigure { get; set; }
        GraphItem AddFigure(int x, int y);
        void CreateAndGrabItem(int x, int y);
        ISelections selController { get; set; } // Потом убрать
    }

    internal class Factory : IFactory
    {
        public PropList pl { set; get; }

        public event Repaint RepaintEvent;
        public FigureType ChoosenFigure { get; set; }
        public ISelections selController { get; set; } // потом убрать?

        Store st;

        public Factory(Store st)//PropList pl)
        {
            ChoosenFigure = FigureType.Line;
            this.st = st;

            ContourProps cp = new ContourProps(Color.White, 1, LineType.HatchFill);
            FillProps fp = new FillProps(Color.Black, FillType.SolidColor);
            pl = new PropList(cp, fp);

            //this.pl = pl;
            selController = new SelectionController(st, this);
        }

        public GraphItem AddFigure(int x, int y)
        {
            Figure f;
            Frame frame = new Frame(x, y, x, y);
            //PropList pl = new PropList(pl.ContourProps, pl.FillProps);
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

        public Group CreateGroup(List<GraphItem> Items)
        {
            List<Frame> frames = new List<Frame>();
            foreach (GraphItem item in Items)
                frames.Add(item.frame);
            Group group = new Group(Items, Frame.FrameSum(frames));
            st.Add(group);
            st.Delete(Items);
            return group;
        }

        public List<GraphItem> Ungroup(Group group)
        {
            List<GraphItem> items = new List<GraphItem>();
            foreach (GraphItem item in group.items)
            {
                st.Add(item);
                items.Add(item);
            }
            return items;
        }
    }
}
