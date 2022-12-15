using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
        Line line;

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
            Frame linefr = line.frame;
            Frame frame = new Frame(0, 0, 0, 0);
            ContourProps cp = new ContourProps(Color.Gray, 1);
            FillProps fp = new FillProps(Color.Gray);
            PropList pl = new PropList(cp, fp);

            Figure marker = new Rect(frame, pl);
        }
    }
    
    internal class RectSelection : Selection // заполнить бы
    {
        int delta = 5;
        Rect rect;

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
        Selection grabbedSelection { set; get; }

        public Selection TryGrab(int x, int y)
        {
            foreach (Selection sel in this)
            {
                if (sel.TryGrab(x, y))
                    return sel;
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

        public void DeleteSelection()
        {

        }
    }

    internal class SelectionController
    {
        public SelectionStore selStore;

        public SelectionController()
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
