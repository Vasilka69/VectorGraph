﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VectorGraph
{
    abstract internal class State
    {
        public abstract IModel Model { set; get; }
        public abstract EventHandler EH { set; get; }

        public abstract void MouseDown(int x, int y);
        public abstract void MouseUp(int x, int y);
        public abstract void MouseMove(int x, int y);
    }

    internal class CreateState : State
    {
        public override IModel Model { set; get; }
        public override EventHandler EH { set; get; }

        public CreateState(IModel Model, EventHandler EH)
        {
            this.Model = Model;
            this.EH = EH;
        }

        public override void MouseMove(int x, int y) { }

        public override void MouseDown(int x, int y)
        {
            SelectionStore selStore = Model.Factory.selController.selStore;
            if (selStore.TryGrab(x, y) == null)
            {
                Model.Factory.CreateAndGrabItem(x, y);

                EH.currState = EH.DS;
                //MessageBox.Show("Sozdau");

            }
            //MessageBox.Show("Sozdau");
        }

        public override void MouseUp(int x, int y)
        {

        }
    }

    internal class DragState : State
    {
        public override IModel Model { set; get; }
        public override EventHandler EH { set; get; }

        public DragState(IModel Model, EventHandler EH)
        {
            this.Model = Model;
            this.EH = EH;
        }

        public override void MouseMove(int x, int y)
        {
            /*
            Model.st[Model.st.Count - 1].frame.coords[2] = x;
            Model.st[Model.st.Count - 1].frame.coords[3] = y;

            Model.GrController.Repaint();
            */

        }

        public override void MouseDown(int x, int y)
        {

        }

        public override void MouseUp(int x, int y)
        {
            GraphItem f = Model.st[Model.st.Count - 1];
            f.frame.coords[2] = x;
            f.frame.coords[3] = y;

            SelectionStore selStore = Model.Factory.selController.selStore;
            //selStore[selStore.Count - 1].points[1] = new System.Drawing.Point(x, y);
            if (selStore.grabbedSelection != null)
                selStore.grabbedSelection.points[1] = new System.Drawing.Point(x, y);

            Model.GrController.Repaint();

            EH.currState = EH.CS;
            //MessageBox.Show("SozdaL");

        }
    }
    /*
    internal class StateStore : List<State>
    {

    }
    */
}
