using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorGraph
{
    internal class Store : List<GraphItem>
    {

        public void Delete(List<GraphItem> Items)
        {
            foreach (GraphItem item in Items)
                this.Remove(item);
        }
    }
}
