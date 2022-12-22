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

namespace VectorGraph
{
    internal abstract class Selection
    {
        private protected int delta = 5;
        private protected GraphItem item;
        private protected List<Point> points;
        private protected Point DragPoint { get; set; }

        private protected List<Point> BeforeGrabPoints;
        private protected Point GrabPoint { get; set; }
        public bool isNullDrag { get; set; }
        public bool isNullGrab { get; set; }

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
                    isNullDrag = false;
                    return true;
                }
            }
            isNullDrag = true;
            return false;
        }

        public virtual void ReleaseDrag(int x, int y)
        {
            /*
            if ( (x < item.frame.coords[0] || y < item.frame.coords[1]) &&
                (GrabbedPoint.X != item.frame.coords[0] && GrabbedPoint.Y != item.frame.coords[1]) )
                return;
            */
            if (isNullDrag)
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
        public abstract bool TryGrab(int x, int y);
        public virtual void ReleaseGrab(int x, int y)
        {

            if (isNullGrab)
                return;
            int diffX = x - GrabPoint.X;
            int diffY = y - GrabPoint.Y;
            //MessageBox.Show(diffX.ToString());
            /*
            MessageBox.Show(GrabPoint.ToString());
            MessageBox.Show(x.ToString() + " " + y.ToString());
            */
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
            isNullDrag = true;
            isNullGrab = true;
            item = line;
            ActualPoints();
        }
        public override bool TryGrab(int x, int y)
        {
            ActualPoints();
            BeforeGrabPoints.Clear();

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
            {
                isNullGrab = false;
                GrabPoint = new Point(x, y);
                for (int coord = 0; coord < item.frame.coords.Count; coord += 2)
                    BeforeGrabPoints.Add(new Point(item.frame.coords[coord], item.frame.coords[coord + 1]));
                return true;
            }
            isNullGrab = true;
            //MessageBox.Show("ne popal2");
            return false;
            /*
            foreach (Point p in points)
                if (x > p.X - delta && x < p.X + delta &&
                    y > p.Y - delta && y < p.Y + delta)
                    return true;
            */

        }

        public override void ReleaseDrag(int x, int y)
        {
            /*
            if ((x < item.frame.coords[0] || y < item.frame.coords[1]) &&
                (GrabbedPoint.X != item.frame.coords[0] && GrabbedPoint.Y != item.frame.coords[1]))
                return;
            */
            if (isNullDrag)
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
            isNullDrag = true;
            isNullGrab = true;
            item = rect;
            ActualPoints();
        }

        public override bool TryGrab(int x, int y)
        {
            ActualPoints();
            BeforeGrabPoints.Clear();

            int delta = (item as Figure).pl.ContourProps.LineWidth / 2;
            int minX = Math.Min(item.frame.coords[0], item.frame.coords[2]);
            int maxX = Math.Max(item.frame.coords[0], item.frame.coords[2]);
            int minY = Math.Min(item.frame.coords[1], item.frame.coords[3]);
            int maxY = Math.Max(item.frame.coords[1], item.frame.coords[3]);
            if (x >= minX - delta &&
                x <= maxX + delta &&
                y >= minY - delta &&
                y <= maxY + delta)
            {
                isNullGrab = false;
                GrabPoint = new Point(x, y);
                //MessageBox.Show(DragPoint.ToString());
                for (int coord = 0; coord < item.frame.coords.Count; coord += 2)
                    BeforeGrabPoints.Add(new Point(item.frame.coords[coord], item.frame.coords[coord + 1]));
                return true;
            }
            //MessageBox.Show("ne popal 3");
            isNullGrab = true;
            BeforeGrabPoints.Clear();
            return false;
        }

    }
    internal class EllipseSelection : Selection
    {

        public EllipseSelection(Ellipse ellipse)
        {
            BeforeGrabPoints = new List<Point>();
            isNullDrag = true;
            isNullGrab = true;
            this.item = ellipse;
            ActualPoints();
        }

        public override bool TryGrab(int x, int y)
        {
            ActualPoints();
            BeforeGrabPoints.Clear();
            int delta = (item as Figure).pl.ContourProps.LineWidth / 2;
            Frame frame = item.frame;
            double a = Math.Abs(frame.coords[0] - frame.coords[2]) / 2 + delta;
            double b = Math.Abs(frame.coords[1] - frame.coords[3]) / 2 + delta;
            int x0 = (frame.coords[0] + frame.coords[2]) / 2;
            int y0 = (frame.coords[1] + frame.coords[3]) / 2;
            double ellipseEq = (Math.Pow((x - x0), 2)) / (Math.Pow(a, 2)) + (Math.Pow((y - y0), 2)) / (Math.Pow(b, 2));
            if (ellipseEq <= 1)
            {
                isNullGrab = false;
                GrabPoint = new Point(x, y);
                for (int coord = 0; coord < item.frame.coords.Count; coord += 2)
                    BeforeGrabPoints.Add(new Point(item.frame.coords[coord], item.frame.coords[coord + 1]));
                return true;
            }
            isNullGrab = true;
            BeforeGrabPoints.Clear();
            return false;
        }

    }

    internal class GroupSelection : Selection
    {

        public GroupSelection(Group group)
        {
            BeforeGrabPoints = new List<Point>();
            isNullDrag = true;
            isNullGrab = true;
            this.item = group;
            ActualPoints();
        }

        public override bool TryGrab(int x, int y)
        {
            ActualPoints();
            BeforeGrabPoints.Clear();

            foreach (GraphItem item in (item as Group).items)
            {
                if (item.selection.TryGrab(x, y))
                {
                    isNullGrab = false;
                    GrabPoint = new Point(x, y);
                    for (int coord = 0; coord < item.frame.coords.Count; coord += 2)
                        BeforeGrabPoints.Add(new Point(item.frame.coords[coord], item.frame.coords[coord + 1]));
                    return true;
                }
            }
            isNullGrab = true;
            BeforeGrabPoints.Clear();
            return false;
        }

        public override bool TryDrag(int x, int y) // Пофиксить
        {
            int delta = 5;
            foreach (Point p in points)
            {
                if (x >= p.X - delta && x <= p.X + delta &&
                    y >= p.Y - delta && y <= p.Y + delta)
                {
                    DragPoint = p;
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
                    isNullDrag = false;
                    return true;
                }
            }
            isNullDrag = true;
            return false;
        }

        public override void ReleaseDrag(int x, int y) // Пофиксить // Довести до ума, мб оно ваще не надо
        {
            int delta = 15;
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

            if (isNullDrag)
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
            int width = Math.Abs(item.frame.coords[0] - item.frame.coords[2]);
            int height = Math.Abs(item.frame.coords[1] - item.frame.coords[3]);
            /*
            foreach (GraphItem item in (item as Group).items)
                for (int coord = 0; coord < item.frame.coords.Count; coord++)
                {
                    //item.frame.coords[coord] = group.frame.coords[coord] + (int)(item.Multipliers[coord] * group.frame.coords[coord]);
                    if (coord % 2 == 0) // X
                        item.frame.coords[coord] = item.frame.coords[coord] + (int)(item.Multipliers[coord] * width);
                    if (coord % 2 == 1) // Y
                        item.frame.coords[coord] = item.frame.coords[coord] + (int)(item.Multipliers[coord] * height);
                }
            */

            foreach (GraphItem item in (item as Group).items)
            {
                item.frame.coords[0] = Math.Min(this.item.frame.coords[0], this.item.frame.coords[2]) + (int)(item.Multipliers[0] * width);
                item.frame.coords[1] = Math.Min(this.item.frame.coords[1], this.item.frame.coords[3]) + (int)(item.Multipliers[1] * height);
                item.frame.coords[2] = Math.Max(this.item.frame.coords[0], this.item.frame.coords[2]) + (int)(item.Multipliers[2] * width);
                item.frame.coords[3] = Math.Max(this.item.frame.coords[1], this.item.frame.coords[3]) + (int)(item.Multipliers[3] * height);
            }

            //ActualPoints();
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

        public void SelectAndDrag(GraphItem item, int x, int y)
        {
            Selection sel = item.CreateSelection();
            selStore.Add(sel);
            TryGrab(x, y, false);
            TryDragGrabbed(x, y);
        }

        public bool TryDragGrabbed(int x, int y)
        {
            Selection selection = selStore.GrabbedSelection;
            if (selection == null)
                return false;
            if (selection.TryDrag(x, y))
                return true;
            return false;
        }

        public bool ReleaseDrag(int x, int y) // модифицирован
        {
            List<Selection> selection = selStore.Selected;

            if (selection == null)
                return false;
            foreach (Selection sel in selection)
            {
                if (!sel.isNullDrag)
                    sel.ReleaseDrag(x, y);
                else if (!sel.isNullGrab)
                    sel.ReleaseGrab(x, y);
            }
            return true;
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
                selStore.Selected.Add(sel);
                return true;
            }
            return false;
        }
        public bool TryGrabSelected(int x, int y)
        {
            List<Selection> selections = selStore.Selected;
            Selection sel = null;
            foreach (Selection selection in selections)
                if (selection.TryGrab(x, y))
                {
                    sel = selection;
                    return true;
                }
            return false;
        }
        /*
        public bool ReleaseGrab(int x, int y) // мб удалить
        {
            Selection selection = selStore.GrabbedSelection;
            if (selection == null)
                return false;
            selStore.GrabbedSelection.ReleaseGrab(x, y);
            return true;
        }
        */
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

        public bool Grouping()
        {
            Group group = factory.CreateGroup(selStore.SelItems());
            Selection sel = group.CreateSelection();
            selStore.Add(sel);
            selStore.DeleteSelections(group);
            if (group != null)
                return true;
            return false;
        }

        public bool Ungrouping()
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
        void SelectAndDrag(GraphItem item, int x, int y);
        bool TryDragGrabbed(int x, int y);
        bool ReleaseDrag(int x, int y);
        bool TryGrab(int x, int y, bool isCtrl);
        bool TryGrabSelected(int x, int y);
        //bool ReleaseGrab(int x, int y);
        void Release();
        int Count();
        void DelSelectedItems();
        bool Grouping();
        bool Ungrouping();
    }
}
