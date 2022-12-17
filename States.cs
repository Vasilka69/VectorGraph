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
        public abstract void Delete();
        public abstract void Esc();
    }

    /*
    internal class StateStore : List<State>
    {

    }
    */

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

        public override void LeftMouseDown(int x, int y)
        {
            Model.Factory.CreateAndGrabItem(x, y);
            Model.GrController.Repaint();
            EH.currState = EH.DS;

            /*
            SelectionStore selStore = Model.Factory.selController.selStore;
            if (!EH.isCtrl)
                Model.GrController.SelStore.Release();
            Selection sel = selStore.TryGrab(x, y);
            if (sel != null) // Попал
            {
                Model.GrController.Repaint();

            }
            else // Не попал
            {
                if (!EH.isCtrl)
                {
                    Model.Factory.CreateAndGrabItem(x, y);
                    EH.currState = EH.DS;
                }
            }
            */
        }

        public override void LeftMouseUp(int x, int y) { }

        public override void Delete() {}

        public override void Esc() { }
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
            GraphItem f = Model.GetLastItem();
            f.frame.coords[2] = x;
            f.frame.coords[3] = y;
            Model.GrController.Repaint();

        }

        public override void LeftMouseDown(int x, int y) { }

        public override void LeftMouseUp(int x, int y)
        {
            GraphItem f = Model.GetLastItem();
            f.frame.coords[2] = x;
            f.frame.coords[3] = y;

            Model.GrController.Repaint();

            EH.currState = EH.SSS;

        }

        public override void Delete() { }

        public override void Esc() { }
    }

    internal class SingleSelectState : State // Чек блокнот (Добавить Ungrouping)
    {
        public override IModel Model { set; get; }
        public override EventHandler EH { set; get; }

        public SingleSelectState(IModel Model, EventHandler EH)
        {
            this.Model = Model;
            this.EH = EH;
        }

        public override void MouseMove(int x, int y) { }

        public override void LeftMouseDown(int x, int y)
        {
        }

        public override void LeftMouseUp(int x, int y)
        {
            SelectionStore selStore = Model.Factory.selController.selStore;
            Selection sel = selStore.TryGrab(x, y);
            if (sel == null) // Не попал
                EH.currState = EH.ES;
            else if (EH.isCtrl)
                EH.currState = EH.MSS;

        }

        public override void Delete()
        {
            EH.currState = EH.ES;
        }

        public override void Esc()
        {
            EH.currState = EH.ES;
        }
    }

    internal class MultiSelectState : State // Grouping
    {
        public override IModel Model { set; get; }
        public override EventHandler EH { set; get; }

        public MultiSelectState(IModel Model, EventHandler EH)
        {
            this.Model = Model;
            this.EH = EH;
        }

        public override void MouseMove(int x, int y) { }

        public override void LeftMouseDown(int x, int y)
        {

        }

        public override void LeftMouseUp(int x, int y)
        {

        }

        public override void Delete()
        {
            EH.currState = EH.ES;
        }

        public override void Esc()
        {
            EH.currState = EH.ES;
        }
    }

    internal class EmptyState : State // Готов
    {
        public override IModel Model { set; get; }
        public override EventHandler EH { set; get; }

        public EmptyState(IModel Model, EventHandler EH)
        {
            this.Model = Model;
            this.EH = EH;
        }

        public override void MouseMove(int x, int y) { }

        public override void LeftMouseDown(int x, int y) { }

        public override void LeftMouseUp(int x, int y)
        {
            SelectionStore selStore = Model.Factory.selController.selStore;
            Selection sel = selStore.TryGrab(x, y);
            if (sel != null) // Попал
            {
                Model.GrController.Repaint();
            }
        }

        public override void Delete() { }

        public override void Esc() { }
    }
}
