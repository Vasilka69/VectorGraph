using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VectorGraph
{
    interface IModel
    {
        IGrProperties GrProperties { get; }
        IGrController GrController { get; }
        IFactory Factory { get; }
        void StoreClear();
        GraphItem GetLastItem();

    }

    internal class Model : IModel
    {
        public IGrProperties GrProperties { get; }
        public IGrController GrController { get; }
        public IFactory Factory { get; }

        public Store st { get; }

        public Model (Graphics gr, IGrProperties GrProperties)
        {
            st = new Store();

            this.GrProperties = GrProperties;
            GrController = new Scene(gr, st, GrProperties);

            Factory = new Factory(st, GrProperties);//pl);
            GrController.SelStore = Factory.selController.selStore;

            Factory.RepaintEvent += GrController.Repaint;

        }

        public void StoreClear()
        {
            st.Clear();
        }

        public GraphItem GetLastItem()
        {
            return st[st.Count - 1];
        }
    }
}
