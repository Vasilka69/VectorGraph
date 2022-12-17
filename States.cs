using System;
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

        public abstract void LeftMouseDown(int x, int y);
        public abstract void LeftMouseUp(int x, int y);
        public abstract void MouseMove(int x, int y);
        public abstract void Esc();
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

        public override void LeftMouseDown(int x, int y) // Временно ctrl
        {
            SelectionStore selStore = Model.Factory.selController.selStore;
            Selection sel = selStore.TryGrab(x, y);
            if (sel == null)
            {
                if (!EH.isCtrl)
                    Model.GrController.SelStore.Release();
                Model.Factory.CreateAndGrabItem(x, y);
                EH.currState = EH.DS;
            }
            else
            {
                Model.GrController.Repaint();
            }
        }

        public override void LeftMouseUp(int x, int y)
        {

        }

        public override void Esc()
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

        }

        public override void LeftMouseDown(int x, int y)
        {

        }

        public override void LeftMouseUp(int x, int y)
        {
            GraphItem f = Model.GetLastItem();
            f.frame.coords[2] = x;
            f.frame.coords[3] = y;

            Model.GrController.Repaint();

            EH.currState = EH.CS;

        }

        public override void Esc()
        {

        }
    }
    /*
    internal class StateStore : List<State>
    {

    }
    */
}
