﻿using System;
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
        Store st { get; }
        void StoreClear();
        ActionList ActionList{ get; }
    }

    internal class Model : IModel
    {
        public IGrProperties GrProperties { get; }
        public IGrController GrController { get; }
        public IFactory Factory { get; }

        public Store st { get; }
        public ActionList ActionList { get; }

        public Model(Graphics gr)//, PropList pl)
        {
            this.ActionList = new ActionList(this);
            st = new Store();

            Factory = new Factory(st);//, pl);

            GrProperties = new GrPropChannel(Factory);//, pl);

            GrController = new Scene(gr, st);//, GrProperties);
            GrController.SelStore = Factory.selController.selStore;

            Factory.RepaintEvent += GrController.Repaint;
        }

        public void StoreClear()
        {
            st.Clear();
        }
    }
}