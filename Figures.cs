﻿using System;
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
