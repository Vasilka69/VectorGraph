using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VectorGraph
{

    internal class ActionList : List<Action>
    {
        /*
         * Добаляет действия, когда они происходят CurrAction = *Текущее действие (мб через DoAction)* ; CurrIndex++;
         * Undo приравнивает CurrAction = this[CurrIndex - 1]; CurrIndex--;
         * Redo приравнивает CurrAction = this[CurrIndex + 1]; CurrIndex++;
         * При совершении действия, обрубает то, что > CurrIndex
        */

        Action CurrAction;
        int CurrIndex; // Мб не нужно, но так удобнее, мб мастхев
        IModel model;

        public delegate void count(string count);
        public event count ActionListUpdated;

        public ActionList(IModel model)
        {
            this.model = model;

            CurrIndex = -1;
            this.AddAction(new Action());
        }

        public void DoAction()
        {

        }

        public bool Undo()
        {
            if (CurrIndex - 1 >= 0)
            {
                //MessageBox.Show("Undo");

                CurrAction.Undo(model);
                //CurrAction.isEnabled = false;

                CurrIndex--;
                CurrAction = this[CurrIndex];

                ActionListUpdated.Invoke(this.Count.ToString() + " " + CurrIndex.ToString());
                return true;
            }
            return false;
        }

        public bool Redo()
        {
            if (CurrIndex + 1 < this.Count)
            {

                CurrIndex++;
                CurrAction = this[CurrIndex];

                CurrAction.Redo(model);

                ActionListUpdated.Invoke(this.Count.ToString() + " " + CurrIndex.ToString());
                return true;
            }
            return false;
        }

        public void DoAddItemAction(GraphItem Item)
        {
            AddAction(new AddItemAction(Item));
        }

        public void DoDelItemAction(GraphItem Item)
        {
            AddAction(new DelItemAction(Item));
        }

        public void AddAction(Action action)
        {
            if (CurrIndex < this.Count - 1)
                this.RemoveRange(CurrIndex + 1, this.Count - CurrIndex - 1);
            this.Add(action);
            CurrAction = action;
            CurrIndex++;

            ActionListUpdated?.Invoke(this.Count.ToString() + " " + CurrIndex.ToString());
        }
    }

    internal class Action
    {
        // Единица действия
        //public bool isEnabled;
        //public abstract void Apply(IModel model);
        public virtual void Undo(IModel model) { }
        public virtual void Redo(IModel model) { }
    }

    internal class AddItemAction : Action
    {
        GraphItem RefItem;
        //GraphItem CloneItem;

        public AddItemAction(GraphItem Item)
        {
            this.RefItem = Item;
            //this.CloneItem = (Item as Figure).Clone();
        }

        public override void Undo(IModel model)
        {
            // Удалить фигуру
            model.st.Remove(RefItem);
            model.Factory.selController.selStore.Remove(RefItem.selection);
            model.Factory.selController.selStore.Selected.Clear();
            model.Factory.selController.selStore.GrabbedSelection = null;

            
        }

        public override void Redo(IModel model)
        {
            // Добавить фигуру
            model.st.Add(this.RefItem);
            //RefItem = model.st[model.st.Count - 1];
            model.Factory.selController.AddSelection(RefItem);
            model.GrController.Repaint();

            /*
            model.Factory.AddFromItem(this.CloneItem);
            RefItem = model.st[model.st.Count - 1];
            model.Factory.selController.AddSelection(RefItem);
            model.GrController.Repaint();
            */
        }
    }

    internal class DelItemAction : Action
    {
        GraphItem RefItem;
        //GraphItem CloneItem;

        public DelItemAction(GraphItem Item)
        {
            this.RefItem = Item;
            //this.CloneItem = (Item as Figure).Clone();
        }

        public override void Undo(IModel model)
        {
            // Вернуть фигуру
            model.st.Add(this.RefItem);

            model.Factory.selController.AddSelection(RefItem);
            model.GrController.Repaint();

            /*
            model.Factory.AddFromItem(this.CloneItem);

            RefItem = model.st[model.st.Count - 1];
            model.Factory.selController.AddSelection(RefItem);
            model.GrController.Repaint();
            */
        }

        public override void Redo(IModel model)
        {
            // Удалить фигуру
            model.st.Remove(RefItem);
            model.Factory.selController.selStore.Remove(RefItem.selection);
            model.Factory.selController.selStore.Selected.Clear();
            model.Factory.selController.selStore.GrabbedSelection = null;
        }
    }

}
