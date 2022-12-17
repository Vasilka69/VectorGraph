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
            EventHandler = new EventHandler(Model); ;
        }

        public string f5()
        {
            /*
            int delta = 5;
            ///////
            List<Point> points = new List<Point>();
            List<Frame> frames = new List<Frame>();
            foreach (Figure f in Model.st)
                frames.Add(f.frame);
            Frame sumFr = Frame.FrameSum(frames);

            points.Add(new Point(sumFr.coords[0], sumFr.coords[1]));
            points.Add(new Point(sumFr.coords[2], sumFr.coords[3]));


            ContourProps cp = new ContourProps(Color.Gray, 1);
            FillProps fp = new FillProps(Color.Gray);
            PropList pl = new PropList(cp, fp);
            foreach (Point p in points)
            {
                Frame frame = new Frame(p.X - delta, p.Y - delta, p.X + delta, p.Y + delta);
                Figure marker = new Rect(frame, pl);
                marker.Draw(Model.GrController.gs);
            }
            //////
            */
            return "123";
        }

    }
}
