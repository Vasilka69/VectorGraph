using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorGraph
{
    internal abstract class GraphItem
    {
        public Frame frame { get; }

        public GraphItem(Frame fr)
        {
            frame = fr;
        }

        public abstract void Draw(GraphSystem gs);
        public abstract Selection CreateSelection();
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
            return new LineSelection(this);
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
            return new RectSelection(this);
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
            return new EllipseSelection(this);
        }
    }
}
