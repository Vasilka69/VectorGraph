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
        //private protected int Color;
        //private protected Color Color;
        public Color Color;

        //public abstract void Apply(GraphSystem gs);
    }

    internal class ContourProps : Property
    {
        // Условно
        public int LineWidth;

        public ContourProps(Color Color, int lineWidth)
        {
            this.Color = System.Drawing.Color.FromArgb(Color.ToArgb());
            LineWidth = lineWidth;
        }
        /*
        public override void Apply(GraphSystem gs)
        {
            gs.ApplyProps();
        }*/
    }

    internal class FillProps : Property
    {

        public FillProps(Color Color)
        {
            this.Color = Color;
        }
        /*
        public override void Apply(GraphSystem gs)
        {
            gs.Update();
        }*/
    }
    
}