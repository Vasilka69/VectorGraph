using System;
using System.Collections.Generic;
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
    }
<<<<<<< Updated upstream
=======

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

        public void SetMultipliers()
        {
            int width = Math.Abs(this.frame.coords[0] - this.frame.coords[2]);
            int height = Math.Abs(this.frame.coords[1] - this.frame.coords[3]);
            foreach (GraphItem item in this.items)
                for (int coord = 0; coord < item.frame.coords.Count; coord++)
                {
                    if (coord % 2 == 0) // X
                        item.Multipliers[coord] = (double)(((item.frame.coords[coord] - this.frame.coords[coord])) / (double)width);
                    if (coord % 2 == 1) // Y
                        item.Multipliers[coord] = (double)(((item.frame.coords[coord] - this.frame.coords[coord])) / (double)height);
                    if (item is Group)
                        (item as Group).SetMultipliers();
                }
        }

        public void ApplyMultipliers()
        {
            int width = Math.Abs(this.frame.coords[0] - this.frame.coords[2]);
            int height = Math.Abs(this.frame.coords[1] - this.frame.coords[3]);
            foreach (GraphItem item in this.items)
            {
                item.frame.coords[0] = Math.Min(this.frame.coords[0], this.frame.coords[2]) + (int)(item.Multipliers[0] * width);
                item.frame.coords[1] = Math.Min(this.frame.coords[1], this.frame.coords[3]) + (int)(item.Multipliers[1] * height);
                item.frame.coords[2] = Math.Max(this.frame.coords[0], this.frame.coords[2]) + (int)(item.Multipliers[2] * width);
                item.frame.coords[3] = Math.Max(this.frame.coords[1], this.frame.coords[3]) + (int)(item.Multipliers[3] * height);
                if (item is Group)
                    (item as Group).ApplyMultipliers();
            }
        }

        public void SetBeforeGrabPoints()
        {
            foreach (GraphItem item in items)
            {
                item.selection.isGrab = true;
                for (int coord = 0; coord < item.frame.coords.Count; coord += 2)
                    item.selection.BeforeGrabPoints.Add(new Point(item.frame.coords[coord], item.frame.coords[coord + 1]));
                if (item is Group)
                    (item as Group).SetBeforeGrabPoints();
            }
        }

        public void ApplyBeforeGrabPoints(int x, int y, int diffX, int diffY)
        {
            foreach (GraphItem item in items)
            {
                for (int coord = 0; coord < item.frame.coords.Count; coord += 2)
                {
                    item.frame.coords[coord] = item.selection.BeforeGrabPoints[coord / 2].X + diffX;
                    item.frame.coords[coord + 1] = item.selection.BeforeGrabPoints[coord / 2].Y + diffY;
                    
                    if (item is Group)
                        (item as Group).ApplyBeforeGrabPoints(x, y, diffX, diffY);
                }
            }
        }

        public Frame GetFrame()
        {
            List<Frame> frames = new List<Frame>();
            foreach (GraphItem item in this.items)
                frames.Add(item.frame);
            Frame frame = Frame.FrameSum(frames);
            this.frame = frame;
            return frame;
        }
    }

>>>>>>> Stashed changes
    enum FigureType 
    { 
        Line = 0, 
        Rect = 1
    }

    internal abstract class Figure : GraphItem
    {
        public PropList pl;
        public FigureType type;

        public Figure(Frame fr, PropList pl) : base(fr)
        {
            this.pl = pl;
        }

        public override void Draw(GraphSystem gs) /// уже не пустышка
        {
            ApplyProps(pl);
            DrawGeometry(gs);
        }

        public abstract void ApplyProps(PropList pl);
        public abstract void DrawGeometry(GraphSystem gs);
    }

    internal class Line : Figure
    {
        public Line(Frame fr, PropList pl) : base(fr, pl)
        {
            type = FigureType.Line;

        }

        public override void ApplyProps(PropList pl)
        {
            this.pl = pl;
        }

        public override void DrawGeometry(GraphSystem gs)
        {
            gs.DrawFigure(this);
        }
    }
    
    internal class Rect : Figure
    {
        public Rect(Frame fr, PropList pl) : base(fr, pl)
        {
            type = FigureType.Rect;

        }

        public override void ApplyProps(PropList pl)
        {
            this.pl = pl;
        }

        public override void DrawGeometry(GraphSystem gs)
        {
            gs.DrawFigure(this);
        }
    }
}
