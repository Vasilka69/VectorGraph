using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorGraph
{
    interface IContourProps
    {
        
        int LineWidth { set; get; }
        Color Color { set; get; }
        LineType Type { set; get; }

        //ContourProps ContourProps { get; set; }
    }

    interface IFillProps
    {
        Color Color { set; get; }
        FillType Type { set; get; }

        //FillProps FillProps { get; set; }
    }

    interface IGrProperties
    {
        IContourProps Contour { get; }
        IFillProps Fill { get; }
    }

    internal class GrPropChannel : IGrProperties
    {
        public IContourProps Contour { get; }
        public IFillProps Fill { get; }

        public GrPropChannel(PropList pl)
        {
            Contour = new ContourProps(pl.ContourProps.Color, pl.ContourProps.LineWidth, LineType.SolidColor);
            Fill = new FillProps(pl.FillProps.Color, FillType.SolidColor);

        }
    }
}
