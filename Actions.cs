using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorGraph
{

    internal class ActionList : List<Action>
    {
        /*
         * Добаляет действия, когда они происходят CurrAction = *Текущее действие (мб через DoAction)* ;CurrIndex++;
         * Undo приравнивает CurrAction = this[CurrIndex - 1]; CurrIndex--;
         * Redo приравнивает CurrAction = this[CurrIndex + 1]; CurrIndex++;
         * При совершении действия, обрубает то, что > CurrIndex
        */

        Action CurrAction;
        int CurrIndex; // Мб не нужно, но так удобнее, мб мастхев
        IModel model;

        public ActionList(IModel model)
        {
            this.model = model;
            CurrIndex = 0;
        }

        public void DoAction()
        {

        }

        public bool Undo()
        {
            if (CurrIndex - 1 >= 0)
            {
                CurrAction.isEnabled = false;
                CurrIndex--;
                CurrAction = this[CurrIndex];
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
                CurrAction.isEnabled = true;
                return true;
            }
            return false;
        }

        public void ApplyCurrAction()
        {
            CurrAction.Apply(model);
        }
    }

    internal abstract class Action
    {
        // Единица действия
        public bool isEnabled;
        public abstract void Apply(IModel model);
    }
    internal class AddItemAction : Action
    {
        SelectionStore selStore;
        //Store Store;
        GraphItem Item;

        public AddItemAction(SelectionStore selStore, /*Store Store, */GraphItem Item)
        {
            this.selStore = selStore;
            //this.Store = Store; Есть в model
            this.Item = Item;
        }

        public override void Apply(IModel model) // Пока без групп
        {
            if (isEnabled) // Undo (Удаляем фигуру)
            {
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
            }
            else // Redo (Возвращаем фигуру)
            {
                model.GrProperties.Contour.Color = (Item as Figure).pl.ContourProps.Color;
                model.GrProperties.Contour.LineWidth = (Item as Figure).pl.ContourProps.LineWidth;

                model.GrProperties.Fill.Color = (Item as Figure).pl.FillProps.Color;

                model.Factory.AddFigure(Item.frame.coords[0], Item.frame.coords[2], Item.frame.coords[3], Item.frame.coords[4]);
            }
        }
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
