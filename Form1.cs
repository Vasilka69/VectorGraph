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
        //IModel model;
        IController controller;
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

            textBox2.Text = pictureBox.Width.ToString();
            textBox3.Text = pictureBox.Height.ToString();
            // PictureBox }
            ContourProps cp = new ContourProps(this.pictureBox1.BackColor, 5);
            FillProps fp = new FillProps(this.pictureBox2.BackColor);
            PropList pl = new PropList(cp, fp);
            gr = pictureBox.CreateGraphics();
            // Model
            //model = new Model(gr, pl);
            //Controller controller = new Controller(new Model(gr, pl), null);
            controller = new Controller(new Model(gr, pl));//, null);


            comboBox1.DataSource = Enum.GetValues(typeof(FigureType));
            comboBox1.SelectedValueChanged += ComboBox1_SelectedValueChanged;
            comboBox1.SelectedIndex = 0; //
            
            controller.Model.Factory.ChoosenFigure = (FigureType)comboBox1.SelectedValue;

            textBox1.TextChanged += TextBox1_TextChanged;

            //graphSystem.UpdateEvent += graphSystem.DrawFigure;
            if (controller.Model.Factory.pl.ContourProps != null)
                textBox1.Text = (controller.Model.Factory.pl.ContourProps).LineWidth.ToString(); ///////
            /*
             * switch (comboBox1.SelectedValue) ////
            {
                case FigureType.Line:
                    model.factory.CurrFigure = new Line(graphSystem.frame, pl);
                    break;
                case FigureType.Rect:
                    graphSystem.CurrFigure = new Rect(graphSystem.frame, pl);
                    break;
            }
            */

            pictureBox.Paint += PictureBox_Paint;
            pictureBox.Resize += PictureBox_VisibleChanged;

            /*
            pictureBox.Invalidated += PictureBox_Invalidated;
            pictureBox.VisibleChanged += PictureBox_VisibleChanged;
            this.ResizeEnd += PictureBox_VisibleChanged;
            pictureBox.StyleChanged += PictureBox_VisibleChanged;*/
            /*
            pictureBox.MouseMove += PictureBox_MouseUp;
            pictureBox.MouseDown += PictureBox_MouseDown;
            pictureBox.MouseUp += PictureBox_MouseUp;
            */

            pictureBox.MouseMove += controller.Model.EventHandler.MouseMove;
            pictureBox.MouseDown += controller.Model.EventHandler.MouseDown;
            pictureBox.MouseUp += controller.Model.EventHandler.MouseUp;

            // graphSystem.Str += MessageBox.Show; вероятно хлам

            //pictureBox.Paint += new PaintEventHandler(this.plusevent);

            pictureBox.Focus();
            /*
            this.Resize += Form1_Resize; /////////
            this.LostFocus += Form1_Resize;
            */

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            //MessageBox.Show("");
            if (this.WindowState == FormWindowState.Minimized || this.Focused == false)
                this.Dispose();
        }

        private void ComboBox1_SelectedValueChanged(object sender, EventArgs e) // Выбор типа фигуры
        {
            controller.Model.Factory.ChoosenFigure = (FigureType) comboBox1.SelectedValue;
        }
        /*
        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            controller.Model.Factory.frame.coords[0] = e.X;
            controller.Model.Factory.frame.coords[1] = e.Y;

            //pictureBox.MouseMove += PictureBox_MouseMove;
        }
        */
        /*
        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            
            controller.Model.Factory.frame.coords[2] = e.X;
            controller.Model.Factory.frame.coords[3] = e.Y;

            controller.Model.Factory.AddFigure();
        }
        */
        /*
        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            //pictureBox.MouseMove -= PictureBox_MouseMove;

            controller.Model.Factory.frame.coords[2] = e.X;
            controller.Model.Factory.frame.coords[3] = e.Y;

            controller.Model.Factory.AddFigure();
            
        }
        */
        private void pictureBox1_Click(object sender, EventArgs e) // Цвет контура
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox1.BackColor = this.colorDialog1.Color;
                (controller.Model.Factory.pl.ContourProps).Color = this.colorDialog1.Color;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e) // Цвет заливки
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox2.BackColor = this.colorDialog1.Color;
                (controller.Model.Factory.pl.FillProps).Color = this.colorDialog1.Color;
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e) // Толщина линии
        {
            if (controller.Model.Factory.pl.ContourProps != null) 
            {
                if ((textBox1.Text.Length == 0)
                    || (int.Parse(textBox1.Text) > 100)
                    || (int.Parse(textBox1.Text) < 0))
                    textBox1.Text = (controller.Model.Factory.pl.ContourProps).LineWidth.ToString();
                else
                    (controller.Model.Factory.pl.ContourProps).LineWidth = int.Parse(textBox1.Text);
            }
        }

        private void PictureBox_VisibleChanged(object sender, EventArgs e) // Обнова картинки
        {
            controller.Model.GrController.Repaint();
        }

        private void PictureBox_Invalidated(object sender, InvalidateEventArgs e) // Обнова картинки
        {
            controller.Model.GrController.Repaint();
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e) // Обнова картинки
        {
            controller.Model.GrController.Repaint();
        }
        
        private void button1_Click(object sender, EventArgs e)  // Кнопка "Создать"
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
                controller.Model.StoreClear();
                this.UpdateStyles();
                pictureBox.BackColor = Color.White;
                pictureBox.Size = new Size(width, height);
                controller.Model.GrController.SetPort(pictureBox.CreateGraphics());//, width, height);
            }
        }

        private void button2_Click(object sender, EventArgs e)  // Кнопка "F5"
        {
            controller.f5();
            /*
            for (int i = 0; i < 1000; i++)
            {
                controller.f5();
                Thread.Sleep(5);
            }
            */
            /*
            graphSystem.Figures = new List<Figure>();
            this.UpdateStyles();
            graphSystem.DrawFigures();
            */
        }

        private void UpdateCoordinates(object sender, MouseEventArgs e)
        {
            this.toolStripStatusLabel1.Text = e.X + "," + e.Y;
        }
    }
}
