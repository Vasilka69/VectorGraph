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
        /*
        Factory factory { get; }
        Scene scene { get; }
        Store st { get; }
        GrPropChannel gpc { get; }
        */
    }

    internal class Model : IModel
    {
        public IGrProperties GrProperties { get; }
        public IGrController GrController { get; }
        public IFactory Factory { get; }
        public IEventHandler EventHandler { get; }
        /*
        public Factory factory { get; }
        public Scene scene { get; }
        */
        //GraphSystem graphSystem;
        public GrPropChannel gpc { get; }
        public Store st { get; }

        public Model (Graphics gr, PropList pl)
        {
            GrProperties = new GrPropChannel(pl);
            /*
            GrProperties.Contour.ContourProps = pl.ContourProps;
            GrProperties.Fill.FillProps = pl.FillProps;
            */
            st = new Store();
            GrController = new Scene(gr, st);
            Factory = new Factory(st, pl);
            Factory.RepaintEvent += GrController.Repaint;
            EventHandler = new EventHandler(this);
            //// EVENT HANDLER
            /*
            st = new Store();
            scene = new Scene(gr, st);
            factory = new Factory(st, pl);
            factory.RepaintEvent += scene.Repaint;
            */
        }

        public void StoreClear()
        {
            st.Clear();
        }
    }
}
