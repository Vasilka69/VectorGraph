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
        public abstract GraphItem GetItem();
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

        public override GraphItem GetItem()
        {
            return line;
        }
    }

    internal class RectSelection : Selection
    {
        int delta = 5;
        Rect rect;
        List<Point> points;

        public RectSelection(Rect rect)
        {
            this.rect = rect;
            ActualPoints();
        }

        public override bool TryGrab(int x, int y)
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

        public override GraphItem GetItem()
        {
            return rect;
        }

    }

    internal class GroupSelection : Selection
    {
        int delta = 5;
        Group group;
        List<Point> points;

        public GroupSelection(Group group)
        {
            this.group = group;
            ActualPoints();
        }

        public override bool TryGrab(int x, int y)
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
            List<Frame> frames = new List<Frame>();
            foreach (GraphItem item in group.items)
                frames.Add(item.frame);
            Frame frame = Frame.FrameSum(frames);
            points.Add(new Point(frame.coords[0], frame.coords[1]));
            points.Add(new Point(frame.coords[0], frame.coords[3]));
            points.Add(new Point(frame.coords[2], frame.coords[1]));
            points.Add(new Point(frame.coords[2], frame.coords[3]));

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

        public override GraphItem GetItem()
        {
            return group;
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

        public override GraphItem GetItem()
        {
            return ellipse;
        }

    }


    internal class SelectionStore : List<Selection>
    {
        public List<Selection> grabbedSelection { set; get; }

        public SelectionStore()
        {
            grabbedSelection = new List<Selection>();
        }

        public Selection TryGrab(int x, int y)//, bool isCtrl)
        {
            foreach (Selection sel in this)
            {
                if (sel.TryGrab(x, y))
                {
                    /*
                    if (!isCtrl && grabbedSelection.Count != 0)
                        Release();
                    grabbedSelection.Add(sel);
                    */
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

        public void DeleteSelection(Selection sel) // Удалить селекшены элементов группы
        {
            //this.Remove(sel);
        }

        public List<GraphItem> SelItems()
        {
            List<GraphItem> items = new List<GraphItem>();
            foreach (Selection sel in grabbedSelection)
                items.Add(sel.GetItem());
            return items;
        }
    }

    internal class SelectionController : ISelections
    {
        public Factory factory;
        public SelectionStore selStore { get; }
        public Store Store { get; }

        public SelectionController(Store store, Factory factory)
        {
            selStore = new SelectionStore();
            Store = store;
            this.factory = factory;
        }

        public void SelectAndGrab(GraphItem item, int x, int y)
        {
            Selection sel = item.CreateSelection();
            selStore.Add(sel);
            TryGrab(x, y, false);
        }

        public bool TryGrab(int x, int y, bool isCtrl)
        {
            Selection sel = selStore.TryGrab(x, y);
            if (sel != null)
            {
                if (!isCtrl)
                    selStore.Release();
                selStore.grabbedSelection.Add(sel);
                return true;
            }
            return false;
        }

        public void Release()
        {
            selStore.Release();
        }

        public int Count()
        {
            return selStore.Count;
        }

        public bool Grouping() // Пока не работает
        {
            Group group = factory.CreateNewGroup(selStore.SelItems());
            if (group != null)
                return true;
            return false;
        }

        public bool Ungrouping() // Пока не работает
        {
            return true;
        }
    }

    internal interface ISelections
    {
        SelectionStore selStore { get; }
        void SelectAndGrab(GraphItem item, int x, int y);
        bool TryGrab(int x, int y, bool isCtrl);
        void Release();
        int Count();
        bool Grouping();
        bool Ungrouping();
    }
}
