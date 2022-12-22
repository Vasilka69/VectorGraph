using System;
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

            switch (f.type)
            {
                case (FigureType.Rect): // Прямоугольник
                    int rectx;
                    int recty;
                    if (x1 - x2 < 0)
                        rectx = x1;
                    else
                        rectx = x2;
                    if (y1 - y2 < 0)
                        recty = y1;
                    else
                        recty = y2;
                    Rectangle rect = new Rectangle(rectx, recty, Math.Abs(width), Math.Abs(height));
                    gr.DrawRectangle(pen, rect);
                    if (fp != null)
                    {
                        int contourWidth = cp.LineWidth / 2;
                        if (contourWidth % 2 != 0)
                            contourWidth--;
                        gr.FillRectangle(new SolidBrush(fp.Color),
                        new Rectangle(rectx + contourWidth + 1, recty + contourWidth + 1,
                                        width - cp.LineWidth, height - cp.LineWidth));
                    }
                    break;
                case (FigureType.Line): // Линия
                    gr.DrawLine(pen, x1, y1, x2, y2);
                    break;
            }
        }
    }
}
