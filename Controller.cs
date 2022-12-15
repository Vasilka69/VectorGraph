using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VectorGraph
{
    interface IController
    {
        IModel Model { set;  get; }
        //IEventHandler EventHandler { get; }
        string f5();
    }
    internal class Controller : IController
    {
        public IModel Model { set; get; }

        //public IEventHandler EventHandler { get; }

        public Controller(IModel model)//, IEventHandler eventHandler)
        {
            this.Model = model;
            //this.EventHandler = eventHandler;
        }
        public string f5()
        {
            Model.Factory.selController.selStore.Draw(Model.GrController.grs); // Временно
            /*
            if (Model.st.Count <= 0)
                return "";
            //MessageBox.Show("sadfasd");
            Random random = new Random();
            PropList fpl = (Model.st[random.Next(Model.st.Count)] as Figure).pl;
            /*
            int col2 = Color.Black.ToArgb();
            MessageBox.Show(col2.ToString());
            int col = fpl.ContourProps.Color.ToArgb();
            MessageBox.Show(col.ToString());
            fpl.ContourProps.Color = Color.FromArgb(col2 - col);
            */
            /*
            fpl.ContourProps.Color = Color.FromArgb(-random.Next(16777216));
            fpl.FillProps.Color = Color.FromArgb(-random.Next(16777216));
            //(Model.st[random.Next(Model.st.Count)] as Figure).pl.ContourProps.Color = Color.Black;
            Model.GrController.Repaint();
            */
            return "AAAAA";
        }

    }
}
