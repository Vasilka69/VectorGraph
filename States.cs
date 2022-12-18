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
        public abstract void Group();
        public abstract void Ungroup();

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
            EH.SetState(EH.DS);

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
        public override void Group() { }
        public override void Ungroup() { }
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

            EH.SetState(EH.SSS);

        }

        public override void Delete() { }

        public override void Esc() { }
        public override void Group() { }
        public override void Ungroup() { }
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
            bool isHit = Model.Factory.selController.TryGrab(x, y, EH.isCtrl);
            if (EH.isCtrl)
            { 
                if (isHit)
                    EH.SetState(EH.MSS);
            }
            else
                if (!isHit)
                {
                    Model.Factory.selController.Release();
                    EH.SetState(EH.ES);
                }
            Model.GrController.Repaint();
            /*
            SelectionStore selStore = Model.Factory.selController.selStore;
            Selection sel = selStore.TryGrab(x, y, EH.isCtrl);
            if (sel == null) // Не попал
            {
                if (!EH.isCtrl)
                {
                    selStore.Release();
                    EH.SetState(EH.ES);
                }
            }
            else if (EH.isCtrl)
            {
                EH.SetState(EH.MSS);

            }
            Model.GrController.Repaint();
            */
            /*
            SelectionStore selStore = Model.Factory.selController.selStore;
            if (!EH.isCtrl)
                selStore.Release();
            Selection sel = selStore.TryGrab(x, y, EH.isCtrl);
            if (sel == null) // Не попал
            {
                if (!EH.isCtrl)
                {
                    selStore.Release();
                    EH.SetState(EH.ES);
                }
            }
            else if (EH.isCtrl)
            {
                EH.SetState(EH.MSS);

            }
            Model.GrController.Repaint();
            */
        }

        public override void Delete()
        {
            SelectionStore selStore = Model.Factory.selController.selStore;
            selStore.Release();
            Model.GrController.Repaint();

            EH.SetState(EH.ES);
        }

        public override void Esc()
        {
            SelectionStore selStore = Model.Factory.selController.selStore;
            selStore.Release();
            Model.GrController.Repaint();

            EH.SetState(EH.ES);
        }
        public override void Group() { }
        public override void Ungroup()
        {
            if (Model.Factory.selController.Ungrouping())
                EH.SetState(EH.MSS);
            Model.GrController.Repaint();
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
            bool isHit = Model.Factory.selController.TryGrab(x, y, EH.isCtrl);
            if (isHit && EH.isCtrl)
            {
                EH.SetState(EH.MSS);

            }
            Model.GrController.Repaint();

        }

        public override void Delete()
        {
            SelectionStore selStore = Model.Factory.selController.selStore;
            selStore.Release();
            Model.GrController.Repaint();

            EH.SetState(EH.ES);
        }

        public override void Esc()
        {
            SelectionStore selStore = Model.Factory.selController.selStore;
            selStore.Release();
            Model.GrController.Repaint();

            EH.SetState(EH.ES);
        }
        public override void Group() // Допилить
        {
            if (Model.Factory.selController.Grouping())
                EH.SetState(EH.SSS);
            Model.GrController.Repaint();
        }
        public override void Ungroup() { }
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
            Model.Factory.selController.Release();
            bool isHit = Model.Factory.selController.TryGrab(x, y, EH.isCtrl);
            if (isHit) // Попал
            {
                Model.GrController.Repaint();
                EH.SetState(EH.SSS);
            }
        }

        public override void Delete() { }

        public override void Esc() { }
        public override void Group() { }
        public override void Ungroup() { }
    }
}
