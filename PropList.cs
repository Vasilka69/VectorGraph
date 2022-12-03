using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorGraph
{
    internal class PropList : List<Property>
    {

        public PropList(ContourProps cp, FillProps fp)
        {
            this.AddRange(new List<Property> { cp, fp });
        }

        public PropList(ContourProps cp)
        {
            this.AddRange(new List<Property> { cp, null});
        }

        public PropList(FillProps fp)
        {
            this.AddRange(new List<Property> { null, fp });
        }

        public PropList Clone() ///////
        {
            return new PropList(new ContourProps(((ContourProps)this[0]).Color, ((ContourProps)this[0]).LineWidth),
                (new FillProps(((FillProps)this[1]).Color)));
        }


    }
}
