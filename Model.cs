using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorGraph
{
    internal class Model
    {
        public Factory factory;
        public Scene scene;
        public Store st;
        //GraphSystem graphSystem;
        // GrPropChannel gpc;

        public Model (Graphics gr, PropList pl)
        {
            st = new Store();
            scene = new Scene(gr, st);
            factory = new Factory(st, pl, scene);
        }
    }
}
