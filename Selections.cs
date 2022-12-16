using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VectorGraph
{
    internal abstract class Selection
    {
        public List<Point> points;
        public abstract bool TryGrab(int x, int y);
        public abstract bool TryDragTo(int x, int y);
        public abstract void ReleaseGrab();
        public abstract void Draw(GraphSystem gs);
    }

    internal class LineSelection : Selection
    {
        int delta = 5;
        Line line;

        public LineSelection(Line line)
        {
            this.line = line;
            points = new List<Point>();
            points.Add(new Point(line.frame.coords[0], line.frame.coords[1]));
            points.Add(new Point(line.frame.coords[2], line.frame.coords[3]));
        }

        public override bool TryGrab(int x, int y) // не работает
        {
            foreach (Point p in points)
            {
                if (x > p.X - delta && x < p.X + delta &&
                    y > p.Y - delta && y < p.Y + delta)
                {
                    //MessageBox.Show("popal");
                    return true;
                }
            }
            return false;
        }

        public override bool TryDragTo(int x, int y) // не работает
        {
            return false;
        }

        public override void ReleaseGrab()  // не работает
        {

        }

        public override void Draw(GraphSystem gs)
        {
            ContourProps cp = new ContourProps(Color.Gray, 1);
            FillProps fp = new FillProps(Color.Gray);
            PropList pl = new PropList(cp, fp);
            foreach (Point p in points)
            {
                Frame frame = new Frame(p.X - delta, p.Y - delta, p.X + delta, p.Y + delta);
                Figure marker = new Rect(frame, pl);
                marker.Draw(gs);
            }
        }
    }
    
    internal class RectSelection : Selection // заполнить бы
    {
        int delta = 5;
        Rect rect;

        public RectSelection(Rect rect)
        {
            this.rect = rect;
            /*
            points = new List<Point>();
            points.Add(new Point(rect.frame.coords[0], rect.frame.coords[1]));
            points.Add(new Point(rect.frame.coords[2], rect.frame.coords[1]));
            points.Add(new Point(rect.frame.coords[0], rect.frame.coords[3]));
            points.Add(new Point(rect.frame.coords[2], rect.frame.coords[3]));
            MessageBox.Show("AS");
            */
        }

        public override bool TryGrab(int x, int y) // не работает
        {

            return false;
        }

        public override bool TryDragTo(int x, int y) // не работает
        {
            return false;
        }

        public override void ReleaseGrab()  // не работает
        {

        }

        public override void Draw(GraphSystem gs)
        {
            Frame linefr = rect.frame;
            Frame frame = new Frame(0, 0, 0, 0);
            ContourProps cp = new ContourProps(Color.Gray, 1);
            FillProps fp = new FillProps(Color.Gray);
            PropList pl = new PropList(cp, fp);

            Figure marker = new Rect(frame, pl);
        }

    }
    

    internal class SelectionStore : List<Selection>
    {
        public Selection grabbedSelection { set; get; }

        public IGrController GrController; // Странно

        public Selection TryGrab(int x, int y)
        {
            foreach (Selection sel in this)
            {
                if (sel.TryGrab(x, y))
                {
                    grabbedSelection = sel;
                    return sel;
                }
            }
            return null;

        }

        public void Release()
        {
            grabbedSelection = null;
        }

        public void Draw(GraphSystem gs)
        {
            //grabbedSelection.Draw(gs);

            foreach (Selection sel in this)
            {
                sel.Draw(gs);
            }
        }

        public void DragSelectionTo(int x, int y)
        {

        }

        public void ReleaseSelection()
        {

        }

        public void DeleteSelection(Selection sel)
        {
            foreach (Selection s in this)
                if (s == sel)
                    this.Remove(s);
        }
    }

    internal class SelectionController
    {
        public SelectionStore selStore;

        public SelectionController()//IGrController GrController)
        {
            selStore = new SelectionStore();
        }

        public void SelectAndGrab(GraphItem item, int x, int y)
        {
            Selection sel = item.CreateSelection();
            selStore.Add(sel);
            selStore.TryGrab(x, y);
        }
    }
}
