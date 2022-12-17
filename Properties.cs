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
    enum LineType
    {
        SolidColor = 0,
        HatchFill = 1
    }
    enum FillType
    {
        SolidColor = 0,
        HatchBrushHor = 1,
        HatchBrushDiagCross = 2,
        HatchBrushBackDiag = 3
    }
    internal abstract class Property
    {
        public abstract void Apply(GraphSystem gs);
    }

    internal class ContourProps : Property, IContourProps
    {
        public Color Color { set; get; }
        public LineType Type { set; get; }


        public int LineWidth { set; get; }

        public ContourProps(Color Color, int lineWidth, LineType type)
        {
            //this.Color = System.Drawing.Color.FromArgb(Color.ToArgb());
            this.Color = Color;
            LineWidth = lineWidth;
            this.Type = type;
        }
        
        public override void Apply(GraphSystem gs)
        {
            gs.GrProperties.Contour.Color = Color;
            gs.GrProperties.Contour.LineWidth = LineWidth;
            gs.GrProperties.Contour.Type = Type;
        }
    }

    internal class FillProps : Property, IFillProps
    {
        public Color Color { set; get; }
        public FillType Type { set; get; }

        public FillProps(Color Color, FillType type)
        {
            this.Color = Color;
            this.Type = type;
        }
        
        public override void Apply(GraphSystem gs)
        {
            gs.GrProperties.Fill.Color = Color;
            gs.GrProperties.Fill.Type = Type;
        }
    }
    
}