﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace VectorGraph
{
    public delegate void curstate(string state);
    interface IEventHandler
    {
        //State CurrState { get; set; }
        void MouseMove(object sender, MouseEventArgs e);
        void LeftMouseDown(object sender, MouseEventArgs e);
        void LeftMouseUp(object sender, MouseEventArgs e);
        void KeyDown(object sender, KeyEventArgs e);
        void KeyUp(object sender, KeyEventArgs e);
        void ToCreateState(object sender, EventArgs e);
        void Group(object sender, EventArgs e);
        void Ungroup(object sender, EventArgs e);

        event curstate CurrStateUpdated;
        event curstate CtrlUpdated;
        event curstate AltUpdated;
    }
    internal class EventHandler : IEventHandler
    {
        public event curstate CurrStateUpdated;
        public event curstate CtrlUpdated;
        public event curstate AltUpdated;

        public State prevState;
        public State currState;

        IModel Model;

        public CreateState CS;
        public DragState DS;
        public SingleSelectState SSS;
        public MultiSelectState MSS;
        public EmptyState ES;
        //private StateStore states;

        public bool isCtrl;
        public bool isAlt;
        public Selection sel; // не было

        public EventHandler(IModel Model)
        {
            isCtrl = false;
            isAlt = false;
            this.Model = Model;
            CS = new CreateState(Model, this);
            DS = new DragState(Model, this);
            SSS = new SingleSelectState(Model, this);
            MSS = new MultiSelectState(Model, this);
            ES = new EmptyState(Model, this);
            prevState = ES;
            currState = ES;
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
            isCtrl = e.Control;
            isAlt = e.Alt;

            switch (e.KeyCode)
            {
                case Keys.Control:
                    isCtrl = true;
                    //MessageBox.Show(isCtrl.ToString());
                    break;
                case Keys.Delete:
                    currState.Delete();
                    //MessageBox.Show("Delete");
                    break;
                case Keys.Escape:
                    currState.Esc();
                    //MessageBox.Show("Escape");
                    break;
                case Keys.Z:
                    if (isCtrl && isAlt)
                    {
                        //MessageBox.Show("Redo");
                        if (Model.ActionList.Redo()) // Redo ctrl alt z
                            SetState(ES);
                    }
                    else if (isCtrl)
                    {
                        //MessageBox.Show("Undo");
                        if (Model.ActionList.Undo()) // Undo ctrl z
                            SetState(ES);
                    }
                    //MessageBox.Show("Escape");
                    break;
            }
            CtrlUpdated.Invoke(isCtrl.ToString());
            AltUpdated.Invoke(isAlt.ToString());
            Model.GrController.Repaint();

        }
        public void KeyUp(object sender, KeyEventArgs e)
        {
            isCtrl = e.Control;
            isAlt = e.Alt;

            switch (e.KeyCode)
            {
                case Keys.Control:
                    isCtrl = false;
                    break;
            }
            CtrlUpdated.Invoke(isCtrl.ToString());
            AltUpdated.Invoke(isAlt.ToString());
        }
        public void Group(object sender, EventArgs e)
        {
            currState.Group();
        }
        public void Ungroup(object sender, EventArgs e)
        {
            currState.Ungroup();
        }

        public void ToCreateState(object sender, EventArgs e)
        {
            SelectionStore selStore = Model.Factory.selController.selStore;
            selStore.Release();
            Model.GrController.Repaint();

            SetState(CS);
        }

        public void SetState(State state)
        {
            prevState = currState;
            currState = state;
            CurrStateUpdated.Invoke(state.ToString().Split('.')[1]);
        }
    }
}
