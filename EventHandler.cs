using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VectorGraph
{
    interface IEventHandler
    {
        //State CurrState { get; set; }
        void MouseMove(object sender, MouseEventArgs e);
        void MouseDown(object sender, MouseEventArgs e);
        void MouseUp(object sender, MouseEventArgs e);
    }
    internal class EventHandler : IEventHandler
    {
        public State currState;

        public CreateState CS;
        public DragState DS;
        //private StateStore states;

        public EventHandler(IModel Model)
        {
            CS = new CreateState(Model, this);
            DS = new DragState(Model, this);
            currState = CS;
        }

        public void MouseMove(object sender, MouseEventArgs e)
        {
        }

        public void MouseDown(object sender, MouseEventArgs e)
        {
            currState.MouseDown(e.X, e.Y);
            /*
            Model.Factory.frame.coords[0] = e.X;
            Model.Factory.frame.coords[1] = e.Y;
            */
        }

        public void MouseUp(object sender, MouseEventArgs e)
        {
            currState.MouseUp(e.X, e.Y);
            /*
            Model.Factory.frame.coords[2] = e.X;
            Model.Factory.frame.coords[3] = e.Y;

            Model.Factory.AddFigure();
            */
        }
    }
}
