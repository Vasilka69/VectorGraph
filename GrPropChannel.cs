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
        /*
        int width { set; get; }
        Color color { set; get; }
        */
        ContourProps ContourProps { get; set; }
    }

    interface IFillProps
    {
        //Color color { set; get; }

        FillProps FillProps { get; set; }
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
            Contour = pl.ContourProps as IContourProps;
            Fill = pl.FillProps as IFillProps;
        }
        //IGrController iGPC; /// ?
    }
}
