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
                CurrIndex--;
                CurrAction = this[CurrIndex - 1];
                return true;
            }
            return false;
        }

        public bool Redo()
        {
            if (CurrIndex + 1 < this.Count)
            {
                CurrIndex++;
                CurrAction = this[CurrIndex + 1];
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

        public abstract void Apply(IModel model);
    }

    internal class PropAction : Action
    {
        PropList pl;

        public override void Apply(IModel model)
        {

        }
    }
}
