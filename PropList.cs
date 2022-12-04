using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorGraph
{
    internal class PropList : List<Property>
    {
        public ContourProps ContourProps { get; set; }
        public FillProps FillProps { get; set; }

        public PropList(ContourProps cp, FillProps fp)
        {
            this.ContourProps = cp;
            this.FillProps = fp;
        }

        public PropList Clone() ///////
        {
            return new PropList(new ContourProps(ContourProps.Color, ContourProps.LineWidth),
                new FillProps(FillProps.Color));
            /*
            return new PropList(new ContourProps(((ContourProps)this[0]).Color, ((ContourProps)this[0]).LineWidth),
                (new FillProps(((FillProps)this[1]).Color)));
            */
        }


    }
}
