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
        void KeyDown(object sender, KeyEventArgs e);
        void KeyUp(object sender, KeyEventArgs e);
    }
    internal class EventHandler : IEventHandler
    {
        public State currState;

        public CreateState CS;
        public DragState DS;
        //private StateStore states;

        public bool isCtrl;

        public EventHandler(IModel Model)
        {
            isCtrl = false;
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
            if ( e.Button == MouseButtons.Left )
                currState.LeftMouseDown(e.X, e.Y);
        }

        public void LeftMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                currState.LeftMouseUp(e.X, e.Y);
        }
        public void KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Control:
                    isCtrl = true;
                    break;
            }
        }
        public void KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Control:
                    isCtrl = false;
                    break;
            }
        }
    }
}
