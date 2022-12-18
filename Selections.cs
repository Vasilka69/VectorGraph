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

            int x0 = x;
            int y0 = y;
            int x1 = points[0].X;
            int x2 = points[1].X;
            int y1 = points[0].Y;
            int y2 = points[1].Y;
            int distance = (int)( (Math.Abs((y2 - y1) * x0 - (x2 - x1) * y0 + x2 * y1 - y2 * x1))
                / (Math.Sqrt(Math.Pow((y2 - y1), 2) + Math.Pow((x2 - x1), 2))) );
            int delta = line.pl.ContourProps.LineWidth / 2;
            if (distance < delta &&
                x >= Math.Min(points[0].X, points[1].X) &&
                x <= Math.Max(points[0].X, points[1].X) &&
                y >= Math.Min(points[0].Y, points[1].Y) &&
                y <= Math.Max(points[0].Y, points[1].Y))
                return true;
            return false;
            /*
            foreach (Point p in points)
            {
                if (x > p.X - delta && x < p.X + delta &&
                    y > p.Y - delta && y < p.Y + delta)
                {
                    return true;
                }
            }
            */

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

            int delta = rect.pl.ContourProps.LineWidth / 2;
            int minX = Math.Min(rect.frame.coords[0], rect.frame.coords[2]);
            int maxX = Math.Max(rect.frame.coords[0], rect.frame.coords[2]);
            int minY = Math.Min(rect.frame.coords[1], rect.frame.coords[3]);
            int maxY = Math.Max(rect.frame.coords[1], rect.frame.coords[3]);
            if (x >= minX - delta &&
                x <= maxX + delta &&
                y >= minY - delta &&
                y <= maxY + delta)
                return true;
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

            foreach (GraphItem item in group.items)
            {
                if (item.selection.TryGrab(x, y))
                    return true;
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

        public override bool TryGrab(int x, int y)
        {
            int delta = ellipse.pl.ContourProps.LineWidth / 2;
            Frame frame = ellipse.frame;
            double a = Math.Abs(frame.coords[0] - frame.coords[2]) / 2 + delta;
            double b = Math.Abs(frame.coords[1] - frame.coords[3]) / 2 + delta;
            int x0 = (frame.coords[0] + frame.coords[2]) / 2;
            int y0 = (frame.coords[1] + frame.coords[3]) / 2;
            double ellipseEq = (Math.Pow((x - x0), 2)) / (Math.Pow(a, 2)) + (Math.Pow((y - y0), 2)) / (Math.Pow(b, 2));
            if (ellipseEq <= 1)
                return true;
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

        public void DeleteSelections(Group group) // Удалить селекшены элементов группы
        {
            foreach (GraphItem item in group.items)
                DeleteSelection(item.selection);
            grabbedSelection.Add(group.selection);
        }

        public void DeleteSelection(Selection sel) // Удалить селекшены элементов группы
        {
            this.Remove(sel);
            grabbedSelection.Remove(sel);
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

        public bool Grouping() // Вроде работает
        {
            Group group = factory.CreateNewGroup(selStore.SelItems());
            Selection sel = group.CreateSelection();
            selStore.Add(sel);
            selStore.DeleteSelections(group);
            if (group != null)
                return true;
            return false;
        }

        public bool Ungrouping() // Вроде работает
        {
            if (selStore.grabbedSelection.Count == 1 && selStore.grabbedSelection[0] is GroupSelection) {
                GroupSelection groupSel = selStore.grabbedSelection[0] as GroupSelection;
                List<GraphItem> items = factory.Ungroup(groupSel.GetItem() as Group);
                foreach (GraphItem item in items)
                selStore.Add(item.CreateSelection());
                selStore.DeleteSelection(groupSel);
                return false;
            }
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
