using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorGraph
{
    internal abstract class Property
    {
        public abstract void Apply(GraphSystem gs);
    }

    internal class ContourProps : Property, IContourProps
    {
        public Color Color { set; get; }

        public int LineWidth { set; get; }

        public ContourProps(Color Color, int lineWidth)
        {
            //this.Color = System.Drawing.Color.FromArgb(Color.ToArgb());
            this.Color = Color;
            LineWidth = lineWidth;
        }
        
        public override void Apply(GraphSystem gs)
        {
            gs.GrProperties.Contour.Color = Color;
            gs.GrProperties.Contour.LineWidth = LineWidth;
        }
    }

    internal class FillProps : Property, IFillProps
    {
        public Color Color { set; get; }

        public FillProps(Color Color)
        {
            this.Color = Color;
        }
        
        public override void Apply(GraphSystem gs)
        {
            gs.GrProperties.Fill.Color = Color;
        }
    }
    
}