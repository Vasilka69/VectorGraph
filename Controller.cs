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

        IEventHandler EventHandler { set; get; }

        string f5();
    }
    internal class Controller : IController
    {
        public IModel Model { set; get; }
        public IEventHandler EventHandler { set; get; }

        public Controller(IModel model)
        {
            Model = model;
            EventHandler = new EventHandler(Model);
        }

        public string f5() // Проверка FrameSum (уже работает)
        {
            
            int delta = 5;
            ///////
            List<Point> points = new List<Point>();
            List<Frame> frames = new List<Frame>();
            foreach (Selection sel in Model.Factory.selController.selStore.grabbedSelection)
                frames.Add(sel.GetItem().frame);
            Frame sumFr = Frame.FrameSum(frames);

            points.Add(new Point(sumFr.coords[0], sumFr.coords[1]));
            points.Add(new Point(sumFr.coords[2], sumFr.coords[3]));


            ContourProps cp = new ContourProps(Color.Gray, 1, LineType.SolidColor);
            FillProps fp = new FillProps(Color.Gray, FillType.SolidColor);
            PropList pl = new PropList(cp, fp);
            foreach (Point p in points)
            {
                Frame frame = new Frame(p.X - delta, p.Y - delta, p.X + delta, p.Y + delta);
                Figure marker = new Rect(frame, pl);
                marker.Draw(Model.GrController.gs);
            }
            //////
            
            /*
            List<Selection> sels = Model.Factory.selController.selStore;
            foreach (Selection s in sels)
                s.Draw(Model.GrController.gs);
            */

            return "123";
        }

    }
}
