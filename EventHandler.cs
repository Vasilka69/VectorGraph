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
        void LeftMouseDown(object sender, MouseEventArgs e);
        void LeftMouseUp(object sender, MouseEventArgs e);
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
            currState.MouseMove(e.X, e.Y);
        }

        public void LeftMouseDown(object sender, MouseEventArgs e)
        {
            //MessageBox.Show(currState.ToString());
            if ( e.Button == MouseButtons.Left )
                currState.LeftMouseDown(e.X, e.Y);
            /*
            Model.Factory.frame.coords[0] = e.X;
            Model.Factory.frame.coords[1] = e.Y;
            */
        }

        public void LeftMouseUp(object sender, MouseEventArgs e)
        {
            //MessageBox.Show(currState.ToString());
            if (e.Button == MouseButtons.Left)
                currState.LeftMouseUp(e.X, e.Y);
            /*
            Model.Factory.frame.coords[2] = e.X;
            Model.Factory.frame.coords[3] = e.Y;

            Model.Factory.AddFigure();
            */
        }
    }
}
