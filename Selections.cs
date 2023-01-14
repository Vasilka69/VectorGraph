using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace VectorGraph
{
    internal abstract class Selection
    {
        private protected int delta = 5;
        private protected GraphItem item;
        private protected List<Point> points;
        private protected Point DragPoint { get; set; }

        public List<Point> BeforeGrabPoints;
        public Point GrabPoint { get; set; }
        public bool isDrag { get; set; }
        public bool isGrab { get; set; }

        public virtual bool TryDrag(int x, int y)
        {
            int delta = this.delta;
            ActualPoints();
            foreach (Point p in points)
            {
                if (x >= p.X - delta && x <= p.X + delta &&
                    y >= p.Y - delta && y <= p.Y + delta)
                {
                    DragPoint = p;
                    isDrag = true;
                    return true;
                }
            }
            isDrag = false;
            return false;
        }

        public virtual void ReleaseDrag(int x, int y)
        {
            if (!isDrag)
                return;
            int coordX = 0;
            int coordY = 1;
            for (int coord = 0; coord < item.frame.coords.Count; coord += 2)
                if (DragPoint.X == item.frame.coords[coord])
                {
                    coordX = coord;
                }
            for (int coord = 1; coord < item.frame.coords.Count; coord += 2)
                if (DragPoint.Y == item.frame.coords[coord])
                {
                    coordY = coord;
                }
            item.frame.coords[coordX] = x;
            item.frame.coords[coordY] = y;
            DragPoint = new Point(x, y);
        }
        public abstract bool TryGrab(int x, int y, bool multi);
        public virtual void ReleaseGrab(int x, int y)
        {

            if (!isGrab)
                return;
            int diffX = x - GrabPoint.X;
            int diffY = y - GrabPoint.Y;
            for (int coord = 0; coord < item.frame.coords.Count; coord += 2)
            {
                item.frame.coords[coord] = BeforeGrabPoints[coord / 2].X + diffX;
                item.frame.coords[coord + 1] = BeforeGrabPoints[coord / 2].Y + diffY;
            }

        }

        public virtual void Draw(GraphSystem gs)
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

        protected virtual void ActualPoints()
        {
            List<Point> points = new List<Point>();
            points.Add(new Point(item.frame.coords[0], item.frame.coords[1]));
            points.Add(new Point(item.frame.coords[0], item.frame.coords[3]));
            points.Add(new Point(item.frame.coords[2], item.frame.coords[1]));
            points.Add(new Point(item.frame.coords[2], item.frame.coords[3]));

            this.points = points;
        }

        public virtual GraphItem GetItem() { return item; }
    }

    internal class LineSelection : Selection
    {

        public LineSelection(Line line)
        {
            BeforeGrabPoints = new List<Point>();
            isDrag = false;
            isGrab = false;
            item = line;
            ActualPoints();
        }
        public override bool TryGrab(int x, int y, bool multi)
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
            int delta = (item as Figure).pl.ContourProps.LineWidth / 2 + 2;
            if ((distance < delta &&
                x >= Math.Min(points[0].X, points[1].X) &&
                x <= Math.Max(points[0].X, points[1].X) &&
                y >= Math.Min(points[0].Y, points[1].Y) &&
                y <= Math.Max(points[0].Y, points[1].Y)) || multi)
            {
                isGrab = true;
                GrabPoint = new Point(x, y);
                BeforeGrabPoints.Clear();
                for (int coord = 0; coord < item.frame.coords.Count; coord += 2)
                    BeforeGrabPoints.Add(new Point(item.frame.coords[coord], item.frame.coords[coord + 1]));
                return true;
            }
            isGrab = false;
            return false;

        }
        
        public override void ReleaseDrag(int x, int y)
        {
            if (!isDrag)
                return;
            for (int coord = 0; coord < item.frame.coords.Count; coord += 2)
            {
                if (item.frame.coords[coord] == DragPoint.X &&
                    item.frame.coords[coord + 1] == DragPoint.Y)
                {
                    item.frame.coords[coord] = x;
                    item.frame.coords[coord + 1] = y;
                    break;
                }
            }
            DragPoint = new Point(x, y);

        }
        
        protected override void ActualPoints()
        {
            List<Point> points = new List<Point>();
            points.Add(new Point(item.frame.coords[0], item.frame.coords[1]));
            points.Add(new Point(item.frame.coords[2], item.frame.coords[3]));

            this.points = points;
        }
    }

    internal class RectSelection : Selection
    {
        public RectSelection(Rect rect)
        {
            BeforeGrabPoints = new List<Point>();
            isDrag = false;
            isGrab = false;
            item = rect;
            ActualPoints();
        }

        public override bool TryGrab(int x, int y, bool multi)
        {
            ActualPoints();

            int delta = (item as Figure).pl.ContourProps.LineWidth / 2;
            int minX = Math.Min(item.frame.coords[0], item.frame.coords[2]);
            int maxX = Math.Max(item.frame.coords[0], item.frame.coords[2]);
            int minY = Math.Min(item.frame.coords[1], item.frame.coords[3]);
            int maxY = Math.Max(item.frame.coords[1], item.frame.coords[3]);
            if ((x >= minX - delta &&
                x <= maxX + delta &&
                y >= minY - delta &&
                y <= maxY + delta) || multi)
            {
                isGrab = true;
                GrabPoint = new Point(x, y);
                BeforeGrabPoints.Clear();
                for (int coord = 0; coord < item.frame.coords.Count; coord += 2)
                    BeforeGrabPoints.Add(new Point(item.frame.coords[coord], item.frame.coords[coord + 1]));
                return true;
            }
            isGrab = false;
            return false;
        }

    }
    internal class EllipseSelection : Selection
    {

        public EllipseSelection(Ellipse ellipse)
        {
            BeforeGrabPoints = new List<Point>();
            isDrag = false;
            isGrab = false;
            this.item = ellipse;
            ActualPoints();
        }

        public override bool TryGrab(int x, int y, bool multi)
        {
            ActualPoints();
            int delta = (item as Figure).pl.ContourProps.LineWidth / 2;
            Frame frame = item.frame;
            double a = Math.Abs(frame.coords[0] - frame.coords[2]) / 2 + delta;
            double b = Math.Abs(frame.coords[1] - frame.coords[3]) / 2 + delta;
            int x0 = (frame.coords[0] + frame.coords[2]) / 2;
            int y0 = (frame.coords[1] + frame.coords[3]) / 2;
            double ellipseEq = (Math.Pow((x - x0), 2)) / (Math.Pow(a, 2)) + (Math.Pow((y - y0), 2)) / (Math.Pow(b, 2));
            if (ellipseEq <= 1 || multi)
            {
                isGrab = true;
                GrabPoint = new Point(x, y);
                BeforeGrabPoints.Clear();
                for (int coord = 0; coord < item.frame.coords.Count; coord += 2)
                    BeforeGrabPoints.Add(new Point(item.frame.coords[coord], item.frame.coords[coord + 1]));
                return true;
            }
            isGrab = false;
            return false;
        }

    }

    internal class GroupSelection : Selection
    {

        public GroupSelection(Group group)
        {
            BeforeGrabPoints = new List<Point>();
            isDrag = false;
            isGrab = false;
            this.item = group;
            ActualPoints();
        }

        public override bool TryGrab(int x, int y, bool multi)
        {
            bool check = false;
            (item as Group).GetFrame();
            ActualPoints();
            foreach (GraphItem item in (item as Group).items)
            {
                if (item.selection.TryGrab(x, y, multi))
                {
                    check = true;
                    isGrab = true;
                    GrabPoint = new Point(x, y);
                }
            }
            if (!check)
            {
                isGrab = false;
                return false;
            }

            for (int coord = 0; coord < item.frame.coords.Count; coord += 2)
                BeforeGrabPoints.Add(new Point(item.frame.coords[coord], item.frame.coords[coord + 1]));

            foreach (GraphItem item in (item as Group).items)
                item.selection.TryGrab(x, y, true);

            return true;
        }

        public override void ReleaseGrab(int x, int y)
        {
            if (!isGrab)
                return;
            
            
            int diffX = x - GrabPoint.X;
            int diffY = y - GrabPoint.Y;

            (item as Group).ApplyBeforeGrabPoints(x, y, diffX, diffY);
            ActualPoints();
        }

        public override bool TryDrag(int x, int y)
        {
            int delta = this.delta;
            //(item as Group).GetFrame();
            ActualPoints();
            foreach (Point p in points)
            {
                if (x >= p.X - delta && x <= p.X + delta &&
                    y >= p.Y - delta && y <= p.Y + delta)
                {
                    DragPoint = p;
                    isDrag = true;
                    (item as Group).SetMultipliers();
                    return true;
                }
            }
            isDrag = false;
            return false;
        }

        public override void ReleaseDrag(int x, int y)
        {
            ActualPoints();
            int delta = 15;

            if (!isDrag)
                return;
            if ((x < item.frame.coords[0] + delta || y < item.frame.coords[1] + delta) &&
                (DragPoint.X != item.frame.coords[0] && DragPoint.Y != item.frame.coords[1]))
                return;
            if ((x > item.frame.coords[2] - delta || y > item.frame.coords[3] - delta) &&
                (DragPoint.X != item.frame.coords[2] && DragPoint.Y != item.frame.coords[3]))
                return;
            if ((x < item.frame.coords[0] + delta || y > item.frame.coords[3] - delta) &&
                (DragPoint.X != item.frame.coords[0] && DragPoint.Y != item.frame.coords[3]))
                return;
            if ((x > item.frame.coords[2] - delta || y < item.frame.coords[1] + delta) &&
                (DragPoint.X != item.frame.coords[2] && DragPoint.Y != item.frame.coords[1]))
                return;

            int coordX = 0;
            int coordY = 1;
            for (int coord = 0; coord < item.frame.coords.Count; coord += 2)
                if (DragPoint.X == item.frame.coords[coord])
                {
                    coordX = coord;
                }
            for (int coord = 1; coord < item.frame.coords.Count; coord += 2)
                if (DragPoint.Y == item.frame.coords[coord])
                {
                    coordY = coord;
                }
            DragPoint = new Point(x, y);

            item.frame.coords[coordX] = x;
            item.frame.coords[coordY] = y;

            (item as Group).ApplyMultipliers();
        }
    }



    internal class SelectionStore : List<Selection>
    {
        public List<Selection> Selected { set; get; }
        public Selection GrabbedSelection { set; get; }


        public SelectionStore()
        {
            Selected = new List<Selection>();

        }

        public Selection TryGrab(int x, int y)
        {
            List<Selection> rev = this;
            rev.Reverse();
            foreach (Selection sel in rev)
            {
                if (sel.TryGrab(x, y, false))
                {
                    return sel;
                }
            }
            return null;
        }

        public void Release()
        {
            Selected.Clear();
        }

        public void Draw(GraphSystem gs)
        {
            if (Selected != null)
                foreach (Selection sel in Selected)
                    sel.Draw(gs);
        }

        public void DeleteSelections(Group group) // Удалить селекшены элементов группы
        {
            foreach (GraphItem item in group.items)
                DeleteSelection(item.selection);
            Selected.Add(group.selection);
        }

        public void DeleteSelection(Selection sel) // Удалить селекшены элементов группы
        {
            this.Remove(sel);
            Selected.Remove(sel);
        }

        public List<GraphItem> GetSelItems()
        {
            List<GraphItem> items = new List<GraphItem>();
            foreach (Selection sel in Selected)
                items.Add(sel.GetItem());
            return items;
        }
    }

    internal class SelectionController : ISelections
    {
        public Factory factory;
        public SelectionStore selStore { get; }
        public Store Store { get; }

        public event DeleteDelegate DeleteEvent;

        public SelectionController(Store store, Factory factory)
        {
            selStore = new SelectionStore();
            Store = store;
            this.factory = factory;
        }

        public void AddSelection(GraphItem item)
        {
            Selection sel = item.CreateSelection();
            selStore.Add(sel);
        }

        public void SelectAndDrag(GraphItem item, int x, int y)
        {
            Selection sel = item.CreateSelection();
            selStore.Add(sel);
            TryGrab(x, y, false);
            TryDragGrabbed(x, y);
        }

        public bool TryGrab(int x, int y, bool isCtrl)
        {
            Selection sel = selStore.TryGrab(x, y);
            if (sel != null)
            {
                if (!isCtrl)
                {
                    selStore.Release();
                    selStore.GrabbedSelection = sel;
                }
                if (selStore.Selected.Find(selection => selection == sel) == null)
                    selStore.Selected.Add(sel);
                return true;
            }
            return false;
        }

        public bool TryDragGrabbed(int x, int y)
        {
            List<Selection> selection = selStore.Selected;
            foreach (Selection sel in selection)
            {
                if (sel == null)
                    return false;
                if (sel.TryDrag(x, y))
                    return true;
            }
            return false;
            /*
            Selection selection = selStore.GrabbedSelection;
            if (selection == null)
                return false;
            if (selection.TryDrag(x, y))
                return true;
            return false;
            */
        }

        public bool TryGrabSelected(int x, int y, bool multi)
        {
            List<Selection> selections = selStore.Selected;
            if (selections.Count == 0)
                return false;
            bool check = false;
            foreach (Selection selection in selections)
                if (selection.TryGrab(x, y, multi))
                    check = true;
            if (check)
                return true;
            return false;
        }

        public bool Release(int x, int y)
        {
            List<Selection> selections = selStore.Selected;
            if (selections == null)
                return false;
            foreach (Selection sel in selections)
            {
                if (sel.isDrag)
                    sel.ReleaseDrag(x, y);
                else if(sel.isGrab)
                    sel.ReleaseGrab(x, y);
            }
            return true;
        }

        public void CancelDragAndGrab()
        {
            foreach (Selection sel in selStore)
            {
                sel.isDrag = false;
                sel.isGrab = false;
            }
        }

        public void SelClear()
        {
            selStore.Release();
        }

        public int Count()
        {
            return selStore.Count;
        }

        public void DelSelectedItems()
        {
            List<GraphItem> Items = new List<GraphItem>(); // мусор?
            foreach (Selection sel in selStore.Selected) {
                Items.Add(sel.GetItem()); //
                DeleteEvent?.Invoke(sel.GetItem());
                Store.Remove(sel.GetItem());
                selStore.Remove(sel);
            }
            while (selStore.Selected.Count != 0)
                selStore.DeleteSelection(selStore.Selected[0]);
        }

        public bool Grouping()
        {
            Group group = factory.CreateGroup(selStore.GetSelItems());
            Selection sel = group.CreateSelection();

            selStore.Add(sel);
            selStore.Selected.Clear();
            selStore.Selected.Add(sel);

            selStore.GrabbedSelection = sel;

            selStore.DeleteSelections(group);
            if (group != null)
                return true;
            return false;
        }

        public bool Ungrouping()
        {
            
            if (selStore.GrabbedSelection is GroupSelection)
            {
                GroupSelection groupSel = selStore.GrabbedSelection as GroupSelection;
                List<GraphItem> items = factory.Ungroup(groupSel.GetItem() as Group);
                selStore.Selected.Clear();
                foreach (GraphItem item in items)
                {
                    Selection sel = item.CreateSelection();
                    selStore.Add(sel);
                    selStore.Selected.Add(sel);
                }
                selStore.Remove(groupSel);
                return true;
            }
            return false;
        }
    }

    delegate void DeleteDelegate(GraphItem item);
    internal interface ISelections
    {
        SelectionStore selStore { get; }

        event DeleteDelegate DeleteEvent;
        void AddSelection(GraphItem item);
        void SelectAndDrag(GraphItem item, int x, int y);
        bool TryDragGrabbed(int x, int y);
        bool Release(int x, int y);
        bool TryGrab(int x, int y, bool isCtrl);
        bool TryGrabSelected(int x, int y, bool multi);
        void CancelDragAndGrab();
        void SelClear();
        int Count();
        void DelSelectedItems();
        bool Grouping();
        bool Ungrouping();
    }
}
