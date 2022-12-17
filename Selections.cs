using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VectorGraph
{
    internal abstract class Selection
    {
        public abstract bool TryGrab(int x, int y);
        public abstract bool TryDragTo(int x, int y);
        public abstract void ReleaseGrab();
        public abstract void Draw(GraphSystem gs);
    }

    internal class LineSelection : Selection
    {
        int delta = 5;
        public Line line;
        List<Point> points;

        public LineSelection(Line line)
        {
            this.line = line;
            ActualPoints();
        }

        public override bool TryGrab(int x, int y) // не работает
        {
            ActualPoints();

            foreach (Point p in points)
            {
                if (x > p.X - delta && x < p.X + delta &&
                    y > p.Y - delta && y < p.Y + delta)
                {
                    return true;
                }
            }
            return false;
            
        }

        public override bool TryDragTo(int x, int y) // пока не работает
        {
            return true;
        }

        public override void ReleaseGrab()  // не работает
        {

        }

        private void ActualPoints()
        {
            List<Point> points = new List<Point>();
            points.Add(new Point(line.frame.coords[0], line.frame.coords[1]));
            points.Add(new Point(line.frame.coords[2], line.frame.coords[3]));

            this.points = points;
        }

        public override void Draw(GraphSystem gs)
        {
            ActualPoints();

            ContourProps cp = new ContourProps(Color.Gray, 1, LineType.SolidColor);
            FillProps fp = new FillProps(Color.Gray, FillType.SolidColor);
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
        List<Point> points;

        public RectSelection(Rect rect)
        {
            this.rect = rect;
            ActualPoints();
        }

        public override bool TryGrab(int x, int y) // не работает
        {
            ActualPoints();

            foreach (Point p in points)
            {
                if (x > p.X - delta && x < p.X + delta &&
                    y > p.Y - delta && y < p.Y + delta)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool TryDragTo(int x, int y) // пока не работает
        {
            return true;
        }

        public override void ReleaseGrab()  // не работает
        {

        }

        private void ActualPoints()
        {
            List<Point> points = new List<Point>();
            points.Add(new Point(rect.frame.coords[0], rect.frame.coords[1]));
            points.Add(new Point(rect.frame.coords[0], rect.frame.coords[3]));
            points.Add(new Point(rect.frame.coords[2], rect.frame.coords[1]));
            points.Add(new Point(rect.frame.coords[2], rect.frame.coords[3]));

            this.points = points;
        }

        public override void Draw(GraphSystem gs)
        {
            ActualPoints();

            ContourProps cp = new ContourProps(Color.Gray, 1, LineType.SolidColor);
            FillProps fp = new FillProps(Color.Gray, FillType.SolidColor);
            PropList pl = new PropList(cp, fp);
            foreach (Point p in points)
            {
                Frame frame = new Frame(p.X - delta, p.Y - delta, p.X + delta, p.Y + delta);
                Figure marker = new Rect(frame, pl);
                marker.Draw(gs);
            }
        }

    }

    internal class EllipseSelection : Selection // заполнить бы
    {
        int delta = 5;
        Ellipse ellipse;
        List<Point> points;

        public EllipseSelection(Ellipse ellipse)
        {
            this.ellipse = ellipse;
            ActualPoints();
        }

        public override bool TryGrab(int x, int y) // не работает
        {
            foreach (Point p in points)
            {
                if (x > p.X - delta && x < p.X + delta &&
                    y > p.Y - delta && y < p.Y + delta)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool TryDragTo(int x, int y) // пока не работает
        {
            return true;
        }

        public override void ReleaseGrab()  // не работает
        {

        }

        private void ActualPoints()
        {
            List<Point> points = new List<Point>();
            points.Add(new Point(ellipse.frame.coords[0], ellipse.frame.coords[1]));
            points.Add(new Point(ellipse.frame.coords[0], ellipse.frame.coords[3]));
            points.Add(new Point(ellipse.frame.coords[2], ellipse.frame.coords[1]));
            points.Add(new Point(ellipse.frame.coords[2], ellipse.frame.coords[3]));

            points.Add(new Point((ellipse.frame.coords[0] + ellipse.frame.coords[2]) / 2,
                ellipse.frame.coords[1]));
            points.Add(new Point(ellipse.frame.coords[0],
                (ellipse.frame.coords[1] + ellipse.frame.coords[3]) / 2));
            points.Add(new Point((ellipse.frame.coords[0] + ellipse.frame.coords[2]) / 2,
                ellipse.frame.coords[3]));
            points.Add(new Point(ellipse.frame.coords[2],
                (ellipse.frame.coords[1] + ellipse.frame.coords[3]) / 2));
            this.points = points;
        }

        public override void Draw(GraphSystem gs)
        {
            ActualPoints();

            ContourProps cp = new ContourProps(Color.Gray, 1, LineType.SolidColor);
            FillProps fp = new FillProps(Color.Gray, FillType.SolidColor);
            PropList pl = new PropList(cp, fp);
            foreach (Point p in points)
            {
                Frame frame = new Frame(p.X - delta, p.Y - delta, p.X + delta, p.Y + delta);
                Figure marker = new Rect(frame, pl);
                marker.Draw(gs);
            }
        }

    }


    internal class SelectionStore : List<Selection>
    {
        public List<Selection> grabbedSelection { set; get; }

        public SelectionStore()
        {
            grabbedSelection = new List<Selection>();
        }

        public Selection TryGrab(int x, int y)
        {
            foreach (Selection sel in this)
            {
                if (sel.TryGrab(x, y))
                {
                    grabbedSelection.Add(sel);
                    return sel;
                }
            }
            return null;

        }

        public void Release()
        {
            grabbedSelection.Clear();
        }

        public void Draw(GraphSystem gs) // как оно должно быть
        {
            if (grabbedSelection != null)
                foreach (Selection sel in grabbedSelection)
                    sel.Draw(gs);
            /*
            foreach (Selection sel in this)
            {
                sel.Draw(gs);
            }
            */
        }

        public void DragSelectionTo(int x, int y)
        {

        }

        public void ReleaseSelection()
        {
            grabbedSelection.Clear();
        }

        public void DeleteSelection(Selection sel)
        {
            this.Remove(sel);
        }
    }

    internal class SelectionController : ISelections
    {
        public SelectionStore selStore { get; }

        public SelectionController()//SelectionStore selStore)//IGrController GrController)
        {
            selStore = new SelectionStore();// IGrController GrController);
            //this.selStore = selStore;
        }

        public void SelectAndGrab(GraphItem item, int x, int y)
        {
            Selection sel = item.CreateSelection();
            selStore.Add(sel);
            selStore.TryGrab(x, y);
        }
    }

    internal interface ISelections
    {
        SelectionStore selStore { get; }
        void SelectAndGrab(GraphItem item, int x, int y);
    }
}
