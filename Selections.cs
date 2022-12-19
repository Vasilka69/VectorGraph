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
        private protected int delta = 5;
        private protected GraphItem item;
        private protected List<Point> points;
        public Point GrabbedPoint { get; set; }
        public abstract bool TryGrab(int x, int y);
        public abstract bool TryDragTo(int x, int y);
        public abstract void ReleaseGrab(int x, int y);
        public abstract void Draw(GraphSystem gs);
        public abstract GraphItem GetItem();
    }

    internal class LineSelection : Selection
    {

        public LineSelection(Line line)
        {
            item = line;
            ActualPoints();
        }
        public override bool TryGrab(int x, int y)
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
            if (distance < delta &&
                x >= Math.Min(points[0].X, points[1].X) &&
                x <= Math.Max(points[0].X, points[1].X) &&
                y >= Math.Min(points[0].Y, points[1].Y) &&
                y <= Math.Max(points[0].Y, points[1].Y))
                return true;
            return false;
            /*
            foreach (Point p in points)
                if (x > p.X - delta && x < p.X + delta &&
                    y > p.Y - delta && y < p.Y + delta)
                    return true;
            */

        }

        public override bool TryDragTo(int x, int y)
        {
            int delta = 5;
            foreach (Point p in points)
            {
                if (x >= p.X - delta && x <= p.X + delta &&
                    y >= p.Y - delta && y <= p.Y + delta)
                {
                    GrabbedPoint = p;
                    return true;
                }
            }
            return false;
        }

        public override void ReleaseGrab(int x, int y)
        {

            if (GrabbedPoint.X == -1 || GrabbedPoint.Y == -1)
                return;
            for (int coord = 0; coord < item.frame.coords.Count; coord += 2)
            {
                if (item.frame.coords[coord] == GrabbedPoint.X &&
                    item.frame.coords[coord + 1] == GrabbedPoint.Y)
                {
                    item.frame.coords[coord] = x;
                    item.frame.coords[coord + 1] = y;
                    GrabbedPoint = new Point(x, y);
                    break;
                }
            }

        }

        private void ActualPoints()
        {
            List<Point> points = new List<Point>();
            points.Add(new Point(item.frame.coords[0], item.frame.coords[1]));
            points.Add(new Point(item.frame.coords[2], item.frame.coords[3]));

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
            return item;
        }
    }

    internal class RectSelection : Selection
    {
        public RectSelection(Rect rect)
        {
            item = rect;
            ActualPoints();
        }

        public override bool TryGrab(int x, int y)
        {
            ActualPoints();

            int delta = (item as Figure).pl.ContourProps.LineWidth / 2;
            int minX = Math.Min(item.frame.coords[0], item.frame.coords[2]);
            int maxX = Math.Max(item.frame.coords[0], item.frame.coords[2]);
            int minY = Math.Min(item.frame.coords[1], item.frame.coords[3]);
            int maxY = Math.Max(item.frame.coords[1], item.frame.coords[3]);
            if (x >= minX - delta &&
                x <= maxX + delta &&
                y >= minY - delta &&
                y <= maxY + delta)
                return true;
            return false;
        }

        public override bool TryDragTo(int x, int y)
        {
            int delta = 5;
            foreach (Point p in points)
            {
                if (x >= p.X - delta && x <= p.X + delta &&
                    y >= p.Y - delta && y <= p.Y + delta)
                {
                    GrabbedPoint = p;
                    return true;
                }
            }
            return false;
        }

        public override void ReleaseGrab(int x, int y)
        {
            if (GrabbedPoint.X == -1 || GrabbedPoint.Y == -1)
                return;
            int coordX = 0;
            int coordY = 1;
            for (int coord = 0; coord < item.frame.coords.Count; coord += 2)
                if (GrabbedPoint.X == item.frame.coords[coord])
                {
                    coordX = coord;
                }
            for (int coord = 1; coord < item.frame.coords.Count; coord += 2)
                if (GrabbedPoint.Y == item.frame.coords[coord])
                {
                    coordY = coord;
                }
            item.frame.coords[coordX] = x;
            item.frame.coords[coordY] = y;
            GrabbedPoint = new Point(x, y);
        }

        private void ActualPoints()
        {
            List<Point> points = new List<Point>();
            points.Add(new Point(item.frame.coords[0], item.frame.coords[1]));
            points.Add(new Point(item.frame.coords[0], item.frame.coords[3]));
            points.Add(new Point(item.frame.coords[2], item.frame.coords[1]));
            points.Add(new Point(item.frame.coords[2], item.frame.coords[3]));

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
            return item;
        }

    }

    internal class GroupSelection : Selection
    {

        public GroupSelection(Group group)
        {
            this.item = group;
            ActualPoints();
        }

        public override bool TryGrab(int x, int y)
        {
            ActualPoints();

            foreach (GraphItem item in (item as Group).items)
            {
                if (item.selection.TryGrab(x, y))
                    return true;
            }
            return false;
        }

        public override bool TryDragTo(int x, int y)
        {
            int delta = 5;
            foreach (Point p in points)
            {
                if (x >= p.X - delta && x <= p.X + delta &&
                    y >= p.Y - delta && y <= p.Y + delta)
                {
                    GrabbedPoint = p;
                    int width = Math.Abs(item.frame.coords[0] - item.frame.coords[2]);
                    int height = Math.Abs(item.frame.coords[1] - item.frame.coords[3]);
                    foreach (GraphItem item in (item as Group).items)
                        for (int coord = 0; coord < item.frame.coords.Count; coord++)
                        {
                            //item.Multipliers[coord] = item.frame.coords[coord] / group.frame.coords[coord];
                            if (coord % 2 == 0) // X
                                item.Multipliers[coord] = (double)(((item.frame.coords[coord] - this.item.frame.coords[coord])) / (double)width);
                            if (coord % 2 == 1) // Y
                                item.Multipliers[coord] = (double)(((item.frame.coords[coord] - this.item.frame.coords[coord])) / (double)height);
                        }
                    string res = "";
                    foreach (double mult in (item as Group).items[0].Multipliers)
                        res += mult + " ";
                    //MessageBox.Show(res);
                    return true;
                }
            }
            return false;
        }

        public override void ReleaseGrab(int x, int y)
        {
            if (GrabbedPoint.X == -1 || GrabbedPoint.Y == -1)
                return;
            int coordX = 0;
            int coordY = 1;
            for (int coord = 0; coord < item.frame.coords.Count; coord += 2)
                if (GrabbedPoint.X == item.frame.coords[coord])
                {
                    coordX = coord;
                }
            for (int coord = 1; coord < item.frame.coords.Count; coord += 2)
                if (GrabbedPoint.Y == item.frame.coords[coord])
                {
                    coordY = coord;
                }
            GrabbedPoint = new Point(x, y);

            item.frame.coords[coordX] = x;
            item.frame.coords[coordY] = y;
            int width = Math.Abs(item.frame.coords[0] - item.frame.coords[2]);
            int height = Math.Abs(item.frame.coords[1] - item.frame.coords[3]);
            /*
            foreach (GraphItem item in group.items)
                for (int coord = 0; coord < item.frame.coords.Count; coord++)
                {
                    //item.frame.coords[coord] = group.frame.coords[coord] + (int)(item.Multipliers[coord] * group.frame.coords[coord]);
                    if (coord % 2 == 0) // X
                        item.frame.coords[coord] = group.frame.coords[coord] + (int)(item.Multipliers[coord] * width);
                    if (coord % 2 == 1) // Y
                        item.frame.coords[coord] = group.frame.coords[coord] + (int)(item.Multipliers[coord] * height);
                }
            */
            
            foreach (GraphItem item in (item as Group).items)
            {
                item.frame.coords[0] = Math.Min(this.item.frame.coords[0], this.item.frame.coords[2]) + (int)(item.Multipliers[0] * width);
                item.frame.coords[1] = Math.Min(this.item.frame.coords[1], this.item.frame.coords[3]) + (int)(item.Multipliers[1] * height);
                item.frame.coords[2] = Math.Max(this.item.frame.coords[0], this.item.frame.coords[2]) + (int)(item.Multipliers[2] * width);
                item.frame.coords[3] = Math.Max(this.item.frame.coords[1], this.item.frame.coords[3]) + (int)(item.Multipliers[3] * height);
            }
            

        }

        private void ActualPoints()
        {
            List<Point> points = new List<Point>();
            /*
            List<Frame> frames = new List<Frame>();
            foreach (GraphItem item in group.items)
                frames.Add(item.frame);
            Frame frame = Frame.FrameSum(frames);
            */
            Frame frame = item.frame;

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
            return item;
        }

    }

    internal class EllipseSelection : Selection // заполнить бы
    {

        public EllipseSelection(Ellipse ellipse)
        {
            this.item = ellipse;
            ActualPoints();
        }

        public override bool TryGrab(int x, int y)
        {
            int delta = (item as Figure).pl.ContourProps.LineWidth / 2;
            Frame frame = item.frame;
            double a = Math.Abs(frame.coords[0] - frame.coords[2]) / 2 + delta;
            double b = Math.Abs(frame.coords[1] - frame.coords[3]) / 2 + delta;
            int x0 = (frame.coords[0] + frame.coords[2]) / 2;
            int y0 = (frame.coords[1] + frame.coords[3]) / 2;
            double ellipseEq = (Math.Pow((x - x0), 2)) / (Math.Pow(a, 2)) + (Math.Pow((y - y0), 2)) / (Math.Pow(b, 2));
            if (ellipseEq <= 1)
                return true;
            return false;
        }

        public override bool TryDragTo(int x, int y)
        {
            int delta = 5;
            foreach (Point p in points)
            {
                if (x >= p.X - delta && x <= p.X + delta &&
                    y >= p.Y - delta && y <= p.Y + delta)
                {
                    GrabbedPoint = p;
                    return true;
                }
            }
            return false;
        }

        public override void ReleaseGrab(int x, int y)
        {
            if (GrabbedPoint.X == -1 || GrabbedPoint.Y == -1)
                return;
            int coordX = 0;
            int coordY = 1;
            for (int coord = 0; coord < item.frame.coords.Count; coord += 2)
                if (GrabbedPoint.X == item.frame.coords[coord])
                {
                    coordX = coord;
                }
            for (int coord = 1; coord < item.frame.coords.Count; coord += 2)
                if (GrabbedPoint.Y == item.frame.coords[coord])
                {
                    coordY = coord;
                }
            item.frame.coords[coordX] = x;
            item.frame.coords[coordY] = y;
            GrabbedPoint = new Point(x, y);
        }

        private void ActualPoints()
        {
            List<Point> points = new List<Point>();
            points.Add(new Point(item.frame.coords[0], item.frame.coords[1]));
            points.Add(new Point(item.frame.coords[0], item.frame.coords[3]));
            points.Add(new Point(item.frame.coords[2], item.frame.coords[1]));
            points.Add(new Point(item.frame.coords[2], item.frame.coords[3]));

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
            return item;
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
            foreach (Selection sel in this)
            {
                if (sel.TryGrab(x, y))
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

        public void Draw(GraphSystem gs) // как оно должно быть
        {
            if (Selected != null)
                foreach (Selection sel in Selected)
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
            Selected.Add(group.selection);
        }

        public void DeleteSelection(Selection sel) // Удалить селекшены элементов группы
        {
            this.Remove(sel);
            Selected.Remove(sel);
        }

        public List<GraphItem> SelItems()
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
            if (TryGrab(x, y, false))
                DragSelTo(x, y);
        }

        public bool TryGrab(int x, int y, bool isCtrl)
        {
            Selection sel = selStore.TryGrab(x, y);
            if (sel != null)
            {
                if (!isCtrl)
                    selStore.Release();
                selStore.Selected.Add(sel);
                return true;
            }
            return false;
        }

        public bool DragSelTo(int x, int y) // Было bool
        {
            Selection selection = null;
            foreach (Selection sel in selStore.Selected)
                if (sel.TryDragTo(x, y))
                {
                    selection = sel;
                    selStore.GrabbedSelection = sel;
                    break;
                }
            if (selection == null)
                return false;
            return true;
        }

        public bool ReleaseGrab(int x, int y)
        {
            Selection selection = selStore.GrabbedSelection;
            if (selection == null)
                return false;
            selStore.GrabbedSelection.ReleaseGrab(x, y);
            return true;
        }

        public void Release()
        {
            selStore.Release();
        }

        public int Count()
        {
            return selStore.Count;
        }

        public void DelSelectedItems()
        {
            List<GraphItem> Items = new List<GraphItem>();
            foreach (Selection sel in selStore.Selected)
            {
                Items.Add(sel.GetItem());
            }
            while (selStore.Selected.Count != 0)
                selStore.DeleteSelection(selStore.Selected[0]);
            Store.Delete(Items);
        }

        public bool Grouping() // Вроде работает
        {
            Group group = factory.CreateGroup(selStore.SelItems());
            Selection sel = group.CreateSelection();
            selStore.Add(sel);
            selStore.DeleteSelections(group);
            if (group != null)
                return true;
            return false;
        }

        public bool Ungrouping() // Вроде работает
        {
            if (selStore.Selected.Count == 1 && selStore.Selected[0] is GroupSelection) {
                GroupSelection groupSel = selStore.Selected[0] as GroupSelection;
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
        bool DragSelTo(int x, int y); // bool
        bool ReleaseGrab(int x, int y);
        void Release();
        int Count();
        void DelSelectedItems();
        bool Grouping();
        bool Ungrouping();
    }
}
