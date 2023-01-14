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
        public virtual void Undo()
        {
            Model.ActionList.Undo();
        }
        public virtual void Redo()
        {
            Model.ActionList.Redo();
        }

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

        public override void LeftMouseDown(int x, int y)
        {
            Model.Factory.CreateAndGrabItem(x, y);
            List<Selection> sels = Model.Factory.selController.selStore.Selected;
            EH.sel = sels[sels.Count - 1];
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
            /*
            //Model.Factory.selController.DragSelTo(x, y);
            EH.sel.ReleaseGrab(x, y);

            Model.GrController.Repaint();
            */
            //ISelections selController = Model.Factory.selController;
            //if ()

            Model.Factory.selController.Release(x, y);
            Model.GrController.Repaint();

        }

        public override void LeftMouseDown(int x, int y) { }

        public override void LeftMouseUp(int x, int y)
        {
            /*
            GraphItem f = Model.GetLastItem();
            f.frame.coords[2] = x;
            f.frame.coords[3] = y;
            
            Model.GrController.Repaint();
            */

            //Model.Factory.selController.ReleaseDrag(x, y);

            ISelections selController = Model.Factory.selController;
            selController.CancelDragAndGrab();
            /// Пофиксить
            //Model.ActionList.AddAction(new AddItemAction(Model.Factory.selController.selStore, Model.st[Model.st.Count - 1]));
            Model.ActionList.AddAction(new AddItemAction(Model.Factory.selController.selStore, Model.st[Model.st.Count - 1]));
            /*
            bool dragHit = selController.TryDragGrabbed(x, y);
            bool grabHit = selController.TryGrab(x, y, EH.isCtrl);
            */
            Model.GrController.Repaint();


            EH.SetState(EH.SSS);
            /*
            if (selController.selStore.Selected.Count <= 1)
                EH.SetState(EH.SSS);
            else
                EH.SetState(EH.MSS);
            */
        }

        public override void Delete() { }

        public override void Esc() { }
        public override void Group() { }
        public override void Ungroup() { }
    }

    internal class SingleSelectState : State
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
            ISelections selController = Model.Factory.selController;

            if (EH.isCtrl)
            {
                if (selController.TryGrab(x, y, EH.isCtrl))
                    EH.SetState(EH.MSS);
            }
            else
            {
                if (selController.TryDragGrabbed(x, y))
                {
                    EH.SetState(EH.DS);
                }
                else if (selController.TryGrabSelected(x, y, false))
                {
                    EH.SetState(EH.DS);
                }
                else
                {
                    EH.SetState(EH.ES);
                    Model.Factory.selController.SelClear();
                }
            }
            Model.GrController.Repaint();
        }

        public override void LeftMouseUp(int x, int y) { }

        public override void Delete()
        {
            Model.Factory.selController.DelSelectedItems();
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

    internal class MultiSelectState : State
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
            /*
            ISelections selController = Model.Factory.selController;
            bool toDragState = false;
            if (EH.isCtrl)
                selController.TryGrab(x, y, EH.isCtrl);
            else
            {
                foreach (Selection sel in selController.selStore.Selected)
                {
                    if (selController.TryDragGrabbed(x, y))
                    {
                        toDragState = true;
                    }
                    else if (selController.TryGrabSelected(x, y, true))
                    {
                        toDragState = true;
                    }

                }
                if (toDragState)
                    EH.SetState(EH.DS);
                else
                {
                    EH.SetState(EH.ES);
                    Model.Factory.selController.SelClear();
                }
            }
            */
            Model.GrController.Repaint();
        }

        public override void LeftMouseUp(int x, int y) { }

        public override void Delete()
        {
            Model.Factory.selController.DelSelectedItems();
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
        public override void Group()
        {
            if (Model.Factory.selController.Grouping())
                EH.SetState(EH.SSS);
            Model.GrController.Repaint();
        }
        public override void Ungroup() { }
    }

    internal class EmptyState : State
    {
        public override IModel Model { set; get; }
        public override EventHandler EH { set; get; }

        public EmptyState(IModel Model, EventHandler EH)
        {
            this.Model = Model;
            this.EH = EH;
        }

        public override void MouseMove(int x, int y) { }

        public override void LeftMouseDown(int x, int y)
        {
            //EH.SetState(EH.DS);
            Model.Factory.selController.SelClear();
            bool isHit = Model.Factory.selController.TryGrab(x, y, EH.isCtrl);
            if (isHit) // Попал
            {
                Model.GrController.Repaint();
                EH.SetState(EH.SSS);
            }
        }

        public override void LeftMouseUp(int x, int y) { }

        public override void Delete() { }

        public override void Esc() { }
        public override void Group() { }
        public override void Ungroup() { }
    }
}
