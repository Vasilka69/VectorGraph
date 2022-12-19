using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VectorGraph
{
    internal abstract class GraphItem
    {
        public List<double> Multipliers { set; get; }
        public Selection selection;
        public Frame frame { get; }

        public GraphItem(Frame fr)
        {
            frame = fr;
            Multipliers = new List<double> { -1, -1, -1, -1};
        }

        public abstract void Draw(GraphSystem gs);
        public abstract Selection CreateSelection();
    }

    internal class Group : GraphItem
    {
        public List<GraphItem> items { get; }
        public Group(List<GraphItem> Items, Frame fr) : base(fr)
        {
            items = Items;
        }
        public override void Draw(GraphSystem gs)
        {
            foreach (GraphItem item in items)
                item.Draw(gs);
        }

        public override Selection CreateSelection()
        {
            selection =  new GroupSelection(this);
            return selection;
        }
    }

    enum FigureType 
    { 
        Line = 0, 
        Rect = 1,
        Ellipse = 2
    }

    internal abstract class Figure : GraphItem
    {
        public PropList pl;
        public FigureType type;

        public Figure(Frame fr, PropList pl) : base(fr)
        {
            this.pl = pl;
        }

        public override void Draw(GraphSystem gs)
        {
            ApplyProps(gs);
            DrawGeometry(gs);
        }

        public abstract void ApplyProps(GraphSystem gs);
        public abstract void DrawGeometry(GraphSystem gs);
    }

    internal class Line : Figure
    {
        public Line(Frame fr, PropList pl) : base(fr, pl)
        {
            type = FigureType.Line;

        }

        public override void ApplyProps(GraphSystem gs)
        {
            //this.pl = pl;
            pl.ContourProps.Apply(gs);
            pl.FillProps.Apply(gs);
        }

        public override void DrawGeometry(GraphSystem gs)
        {
            gs.DrawFigure(this);
        }
        public override Selection CreateSelection()
        {
            selection = new LineSelection(this);
            return selection;
        }
    }

    internal class Rect : Figure
    {
        public Rect(Frame fr, PropList pl) : base(fr, pl)
        {
            type = FigureType.Rect;

        }

        public override void ApplyProps(GraphSystem gs)
        {
            //this.pl = pl;
            pl.ContourProps.Apply(gs);
            pl.FillProps.Apply(gs);
        }

        public override void DrawGeometry(GraphSystem gs)
        {
            gs.DrawFigure(this);
        }
        public override Selection CreateSelection()
        {
            selection = new RectSelection(this);
            return selection;
        }
    }

    internal class Ellipse : Figure
    {
        public Ellipse(Frame fr, PropList pl) : base(fr, pl)
        {
            type = FigureType.Ellipse;

        }

        public override void ApplyProps(GraphSystem gs)
        {
            //this.pl = pl;
            pl.ContourProps.Apply(gs);
            pl.FillProps.Apply(gs);
        }

        public override void DrawGeometry(GraphSystem gs)
        {
            gs.DrawFigure(this);
        }
        public override Selection CreateSelection()
        {
            selection = new EllipseSelection(this);
            return selection;
        }
    }
}
