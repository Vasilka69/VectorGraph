using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VectorGraph
{
    public partial class Form1 : Form
    {
        GraphSystem graphSystem;
        PictureBox pictureBox;
        Graphics gr;

        public Form1()
        {
            InitializeComponent();

            // PictureBox {
            pictureBox = new PictureBox();

            this.panel3.Controls.Add(pictureBox);

            //pictureBox.Dock = DockStyle.Fill;
            //pictureBox.SizeMode
            //pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox.Anchor = AnchorStyles.None;
            pictureBox.BackColor = Color.White;
            pictureBox.Location = new System.Drawing.Point(0, 0);
            pictureBox.Name = "pictureBox1";
            pictureBox.Size = new System.Drawing.Size(604, 425);
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;


            pictureBox.MouseMove += this.UpdateCoordinates;

            gr = pictureBox.CreateGraphics(); /////////

            // PictureBox }

            textBox2.Text = pictureBox.Width.ToString();
            textBox3.Text = pictureBox.Height.ToString();

            comboBox1.DataSource = Enum.GetValues(typeof(FigureType));
            comboBox1.SelectedValueChanged += ComboBox1_SelectedValueChanged;

            textBox1.TextChanged += TextBox1_TextChanged;


            ContourProps cp = new ContourProps(this.pictureBox1.BackColor, 5);
            FillProps fp = new FillProps(this.pictureBox2.BackColor);
            PropList pl = new PropList(cp, fp);

            graphSystem = new GraphSystem(pl);
            graphSystem.ChoosenFigure = (FigureType) comboBox1.SelectedValue;
            graphSystem.UpdateEvent += DrawFigure;
            if (graphSystem.pl != null && graphSystem.pl.Count > 1)
                textBox1.Text = ((ContourProps)graphSystem.pl[0]).LineWidth.ToString(); ///////
            switch (comboBox1.SelectedValue) ////
            {
                case FigureType.Line:
                    graphSystem.CurrFigure = new Line(graphSystem.frame, pl);
                    break;
                case FigureType.Rect:
                    graphSystem.CurrFigure = new Rect(graphSystem.frame, pl);
                    break;
            }

            

            pictureBox.Paint += PictureBox_Paint;
            /*
            pictureBox.Invalidated += PictureBox_Invalidated;
            pictureBox.VisibleChanged += PictureBox_VisibleChanged;
            this.ResizeEnd += PictureBox_VisibleChanged;
            pictureBox.StyleChanged += PictureBox_VisibleChanged;*/

            pictureBox.MouseDown += PictureBox_MouseDown;
            pictureBox.MouseUp += PictureBox_MouseUp;

            pictureBox.Focus();

            graphSystem.Str += MessageBox.Show;

            //pictureBox1.


            //pictureBox.Paint += new PaintEventHandler(this.plusevent);
        }

        private void ComboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            graphSystem.ChoosenFigure = (FigureType) comboBox1.SelectedValue;
            switch((FigureType)comboBox1.SelectedValue)
            {
                case (FigureType.Rect):
                    graphSystem.CurrFigure = new Rect(graphSystem.frame, graphSystem.pl);
                break;
                case (FigureType.Line):
                    graphSystem.CurrFigure = new Line(graphSystem.frame, graphSystem.pl);
                    break;
            }

        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            graphSystem.frame.coords[0] = e.X;
            graphSystem.frame.coords[1] = e.Y;
            //pictureBox.MouseMove += PictureBox_MouseMove;
        }
        /*
        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            graphSystem.frame.coords[2] = e.X;
            graphSystem.frame.coords[3] = e.Y;
            this.UpdateStyles();
            graphSystem.DrawFigures();
            DrawFigure(graphSystem.CurrFigure);
        }*/
        
        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            //pictureBox.MouseMove -= PictureBox_MouseMove;
            
            graphSystem.frame.coords[2] = e.X;
            graphSystem.frame.coords[3] = e.Y;
            
            graphSystem.AddFigure();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox1.BackColor = this.colorDialog1.Color;
                ((ContourProps)graphSystem.pl[0]).Color = this.colorDialog1.Color;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox2.BackColor = this.colorDialog1.Color;
                ((FillProps)graphSystem.pl[1]).Color = this.colorDialog1.Color;
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if ((ContourProps)graphSystem.pl[0] != null) 
            {
                if ((textBox1.Text.Length == 0)
                    || (int.Parse(textBox1.Text) > 100)
                    || (int.Parse(textBox1.Text) < 0))
                    textBox1.Text = ((ContourProps)graphSystem.pl[0]).LineWidth.ToString();
                else
                    ((ContourProps)graphSystem.pl[0]).LineWidth = int.Parse(textBox1.Text);
            }
        }

        private void PictureBox_VisibleChanged(object sender, EventArgs e)
        {
            graphSystem.DrawFigures();
        }

        private void PictureBox_Invalidated(object sender, InvalidateEventArgs e)
        {
            graphSystem.DrawFigures();
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            graphSystem.DrawFigures();
        }



        private void DrawFigure(Figure f)
        {
            /*
            MessageBox.Show(f.type.ToString() + " - " + f.pl[0].Color.ToString()
                + " - " + f.frame.coords[0].ToString()
                + " - " + f.frame.coords[1].ToString()
                + " - " + f.frame.coords[2].ToString()
                + " - " + f.frame.coords[3].ToString());
            */
            ContourProps cp = f.pl[0] as ContourProps;
            FillProps fp = null;
            if (f.pl.Count > 1)
                fp = f.pl[1] as FillProps;
            Pen pen = new Pen(cp.Color, cp.LineWidth);

            int x1 = f.frame.coords[0];
            int y1 = f.frame.coords[1];
            int width = Math.Abs(f.frame.coords[0] - f.frame.coords[2]);
            int height = Math.Abs(f.frame.coords[1] - f.frame.coords[3]);

            int x2 = f.frame.coords[2];
            int y2 = f.frame.coords[3];

            switch (f.type)
            {
                case (FigureType.Rect):
                    int rectx;
                    int recty;
                    if (x1 - x2 < 0)
                        rectx = x1;
                    else
                        rectx = x2;
                    if (y1 - y2 < 0)
                        recty = y1;
                    else
                        recty = y2;
                    Rectangle rect = new Rectangle(rectx, recty, Math.Abs(width), Math.Abs(height));
                    gr.DrawRectangle(pen, rect);
                    if (fp != null)
                    {
                        int contourWidth = cp.LineWidth / 2;
                        if (contourWidth % 2 != 0)
                            contourWidth--;
                        gr.FillRectangle(new SolidBrush(fp.Color),
                        new Rectangle(rectx + contourWidth + 1, recty + contourWidth + 1,
                                        width - cp.LineWidth, height - cp.LineWidth));
                    }
                    break;
                case (FigureType.Line):
                    gr.DrawLine(pen, x1, y1, x2, y2);
                    break;
            }
        }

        private void UpdateCoordinates(object sender, MouseEventArgs e)
        {
            this.toolStripStatusLabel1.Text = e.X + "," + e.Y;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            int width = 0;
            int height = 0;
            if (textBox2.Text.Length != 0 && textBox3.Text.Length != 0)
            {
                width = int.Parse(textBox2.Text);
                height = int.Parse(textBox3.Text);
            }
            if (width > 0 && height > 0)
            {
                graphSystem.Figures = new List<Figure>();
                this.UpdateStyles();
                pictureBox.BackColor = Color.White;
                pictureBox.Size = new Size(width, height);
            }

            //MessageBox.Show(width.ToString() + " " + height.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            graphSystem.Figures = new List<Figure>();
            this.UpdateStyles();
            graphSystem.DrawFigures();
        }

        
        /*
        private void button3_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            int width = pictureBox.Width;
            int height = pictureBox.Height;

            Graphics gr = pictureBox.CreateGraphics();
            for (int i = 0; i < 1000; i++)
            {
                Pen pen = new Pen(Color.FromArgb(255, (int)random.Next(0, 255),
                    (int)random.Next(0, 255), (int)random.Next(0, 255)), (int)random.Next(0, 10));
                gr.DrawRectangle(pen, (int)random.Next(0, width), (int)random.Next(0, height),
                    (int)random.Next(0, 150), (int)random.Next(0, 150));
                Application.DoEvents();
                Thread.Sleep(10);
            }
            //this.UpdateStyles();
        }*/



        /*
        int delta = 0;

        public void plusevent(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0), 5);
            e.Graphics.DrawRectangle(pen, 5+delta, 5+delta, 50, 100);
            delta += 10;
        }
        */
    }
}
