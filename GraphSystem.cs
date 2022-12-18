using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VectorGraph
{
    internal class GraphSystem
    {
        public Graphics gr;

        public IGrProperties GrProperties;

        public GraphSystem(Graphics gr, IGrProperties GrProperties)
        {
            this.gr = gr;
            this.GrProperties = GrProperties;
        }

        public void DrawFigure(Figure f)
        {
            if (f == null)
                return;

            Brush brush = new SolidBrush(GrProperties.Fill.Color);
            switch (GrProperties.Fill.Type)
            {
                case (FillType.SolidColor):
                    brush = new SolidBrush(GrProperties.Fill.Color);
                    break;
                case (FillType.HatchBrushHor):
                    brush = new HatchBrush(HatchStyle.Horizontal, GrProperties.Fill.Color, Color.White);
                    break;
                case (FillType.HatchBrushDiagCross):
                    brush = new HatchBrush(HatchStyle.DiagonalCross, GrProperties.Fill.Color, Color.White);
                    break;
                case (FillType.HatchBrushBackDiag):
                    brush = new HatchBrush(HatchStyle.BackwardDiagonal, GrProperties.Fill.Color, Color.White);
                    break;
            }

            Pen pen = new Pen(GrProperties.Contour.Color, GrProperties.Contour.LineWidth);
            switch (GrProperties.Contour.Type)
            {
                case (LineType.SolidColor):
                    //penBrush = new SolidBrush(GrProperties.Contour.Color);
                    break;
                case (LineType.HatchFill):
                    float[] dashValues = { 2, 1 };
                    pen.DashPattern = dashValues;
                    break;
            }

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
            /*
            int border = 5;
            Rectangle rect2 = new Rectangle(rectx + border, recty + border, width - border * 2, height - border * 2);
            ControlPaint.DrawSelectionFrame(gr, true, rect, rect2, Color.Black);
            */

            switch (f.type)
            {
                case (FigureType.Rect): // Прямоугольник
                    if (GrProperties.Fill != null)
                        gr.FillRectangle(brush, rect);
                    gr.DrawRectangle(pen, rect);
                    break;
                case (FigureType.Line): // Линия
                    gr.DrawLine(pen, x1, y1, x2, y2);
                    break;

                case (FigureType.Ellipse): // Эллипс
                    if (GrProperties.Fill != null)
                        gr.FillEllipse(brush, rect);
                    gr.DrawEllipse(pen, rect);
                    break;
            }
        }
    }
}
