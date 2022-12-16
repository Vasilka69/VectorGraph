﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VectorGraph
{
    internal class GraphSystem
    {
        public Graphics gr;

        public GraphSystem(Graphics gr)
        {
            this.gr = gr;
        }

        public void DrawFigure(Figure f)
        {
            if (f == null)
                return;
            ContourProps cp = f.pl.ContourProps;
            FillProps fp = f.pl.FillProps;
            Pen pen = new Pen(cp.Color, cp.LineWidth);

            int x1 = f.frame.coords[0];
            int y1 = f.frame.coords[1];

            int x2 = f.frame.coords[2];
            int y2 = f.frame.coords[3];

            int width = Math.Abs(x1 - x2);
            int height = Math.Abs(y1 - y2);

            int rectx;
            int recty;
            rectx = Math.Min(x1, x2);
            recty = Math.Min(y1, y2);

            Rectangle rect = new Rectangle(rectx, recty, width, height);

            switch (f.type)
            {
                case (FigureType.Rect): // Прямоугольник
                    if (fp != null)
                        gr.FillRectangle(new SolidBrush(fp.Color), rect);
                    gr.DrawRectangle(pen, rect);
                    break;

                case (FigureType.Line): // Линия
                    gr.DrawLine(pen, x1, y1, x2, y2);
                    break;

                case (FigureType.Ellipse): // Эллипс
                    if (fp != null)
                        gr.FillEllipse(new SolidBrush(fp.Color), rect);
                    gr.DrawEllipse(pen, rect);
                    break;
            }
        }
    }
}
