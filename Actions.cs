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
                //MessageBox.Show("Redo");
                //CurrAction.isEnabled = true;

                CurrIndex++;
                CurrAction = this[CurrIndex];

                CurrAction.Redo(model);

                ActionListUpdated.Invoke(this.Count.ToString() + " " + CurrIndex.ToString());
                return true;
            }
            return false;
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
        SelectionStore selStore;
        //Store Store;
        GraphItem Item;


        public AddItemAction(SelectionStore selStore, /*Store Store, */GraphItem Item)
        {

            this.selStore = selStore;
            this.Item = (Item as Figure).Clone();
            //isEnabled = true;
            //this.Store = Store; Есть в model

            // DeepCopy Item:
            // Пока без групп, тока фигуры
            //Figure figure = Item as Figure;
            //figure
            //this.Item = new GraphItem(Item.frame, (Item as Figure).pl.Clone());
            /*
            Frame frame = Item.frame;
            PropList pl = (Item as Figure).pl;
            switch ((Item as Figure).type)
            {
                case FigureType.Line:
                    this.Item = new Line(frame, pl.Clone());
                    break;
                case FigureType.Rect:
                    this.Item = new Rect(frame, pl.Clone());
                    break;
                case FigureType.Ellipse:
                    this.Item = new Ellipse(frame, pl.Clone());
                    break;
                default:
                    this.Item = null;
                    break;
            }
            */
        }

        public override void Undo(IModel model)
        {
            // Удалить фигуру
            if (model.st.Count != 0)
                model.st.RemoveAt(model.st.Count - 1);
            /*
            List<GraphItem> Items = new List<GraphItem>();
            foreach (Selection sel in selStore.Selected)
            {
                Items.Add(sel.GetItem());
                model.st.Remove(sel.GetItem());
                selStore.Remove(sel);
            }
            while (selStore.Selected.Count != 0)
                selStore.DeleteSelection(selStore.Selected[0]);
            */
            model.GrController.Repaint(); // Потом убрать
        }

        public override void Redo(IModel model)
        {
            // Добавить фигуру
            //MessageBox.Show(this.Item.ToString());
            /*
            model.GrProperties.Contour.Color = (Item as Figure).pl.ContourProps.Color;
            model.GrProperties.Contour.LineWidth = (Item as Figure).pl.ContourProps.LineWidth;

            model.GrProperties.Fill.Color = (Item as Figure).pl.FillProps.Color;

            model.Factory.AddFigure(Item.frame.coords[0], Item.frame.coords[1], Item.frame.coords[2], Item.frame.coords[3]);
            */

            model.Factory.AddFromItem(this.Item);
            model.GrController.Repaint();
        }


        /*
        public override void Apply(IModel model) // Пока без групп
        {
            if (isEnabled) // Undo (Удаляем фигуру)
            {
                //MessageBox.Show(Item.ToString());


                /*
                //MessageBox.Show("");
                // Не оч хорошо
                List<GraphItem> Items = new List<GraphItem>();
                foreach (Selection sel in selStore.Selected)
                {
                    Items.Add(sel.GetItem());
                    model.st.Remove(sel.GetItem());
                    selStore.Remove(sel);
                }
                while (selStore.Selected.Count != 0)
                    selStore.DeleteSelection(selStore.Selected[0]);
                */
            /*
            }
            else // Redo (Возвращаем фигуру)
            {

                /*
                model.GrProperties.Contour.Color = (Item as Figure).pl.ContourProps.Color;
                model.GrProperties.Contour.LineWidth = (Item as Figure).pl.ContourProps.LineWidth;

                model.GrProperties.Fill.Color = (Item as Figure).pl.FillProps.Color;

                model.Factory.AddFigure(Item.frame.coords[0], Item.frame.coords[1], Item.frame.coords[2], Item.frame.coords[3]);
                */
                /*
            }
        }
        */
    }

    /*
    internal class PropAction : Action
    {
        PropList pl;

        public PropAction(PropList pl)
        {
            this.pl = pl;
        }

        public override void Apply(IModel model)
        {
            model.GrProperties.Contour.Color = pl.ContourProps.Color;
            model.GrProperties.Contour.LineWidth = pl.ContourProps.LineWidth;

            model.GrProperties.Fill.Color = pl.FillProps.Color;
        }
    }
    */
}
