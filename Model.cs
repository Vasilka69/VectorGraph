using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorGraph
{
    interface IModel
    {
        IGrProperties GrProperties { get; }
        IGrController GrController { get; }
        IFactory Factory { get; }
        IEventHandler EventHandler { get; }
        void StoreClear();
        Store st { get; }

    }

    internal class Model : IModel
    {
        public IGrProperties GrProperties { get; }
        public IGrController GrController { get; }
        public IFactory Factory { get; }
        public IEventHandler EventHandler { get; }

        public GrPropChannel gpc { get; }
        public Store st { get; }

        public Model (Graphics gr, PropList pl)
        {
            GrProperties = new GrPropChannel(pl);
            st = new Store();
            GrController = new Scene(gr, st);
            Factory = new Factory(st, pl);
            GrController.SelStore = Factory.selController.selStore;
            //Factory.selController.selStore.GrController = GrController; // Странно
            Factory.RepaintEvent += GrController.Repaint;
            EventHandler = new EventHandler(this);
        }

        public void StoreClear()
        {
            st.Clear();
        }
    }
}
