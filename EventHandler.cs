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
        public SingleSelectState SSS;
        public MultiSelectState MSS;
        public EmptyState ES;
        //private StateStore states;

        public bool isCtrl;

        public EventHandler(IModel Model)
        {
            isCtrl = false;
            CS = new CreateState(Model, this);
            DS = new DragState(Model, this);
            SSS = new SingleSelectState(Model, this);
            MSS = new MultiSelectState(Model, this);
            ES = new EmptyState(Model, this);
            currState = CS;
        }

        public void MouseMove(object sender, MouseEventArgs e)
        {
            currState.MouseMove(e.X, e.Y);
        }

        public void LeftMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left )
                currState.LeftMouseDown(e.X, e.Y);
        }

        public void LeftMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                currState.LeftMouseUp(e.X, e.Y);
        }
        public void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
                isCtrl = true;

            
            switch (e.KeyCode)
            {
                case Keys.Control:
                    isCtrl = true;
                    MessageBox.Show(isCtrl.ToString());
                    break;
                case Keys.Delete:
                    currState.Delete();
                    //MessageBox.Show("Delete");
                    break;
                case Keys.Escape:
                    currState.Esc();
                    //MessageBox.Show("Escape");
                    break;
            }
            
        }
        public void KeyUp(object sender, KeyEventArgs e)
        {
            if (!e.Control)
                isCtrl = false;
            
            switch (e.KeyCode)
            {
                case Keys.Control:
                    isCtrl = false;
                    break;
            }
            
        }
    }
}
