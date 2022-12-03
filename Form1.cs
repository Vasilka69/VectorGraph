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
        Model model;
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
            model = new Model(gr, pl);


            comboBox1.DataSource = Enum.GetValues(typeof(FigureType));
            comboBox1.SelectedValueChanged += ComboBox1_SelectedValueChanged;
            comboBox1.SelectedIndex = 0; //
            model.factory.ChoosenFigure = (FigureType)comboBox1.SelectedValue;

            textBox1.TextChanged += TextBox1_TextChanged;

            //graphSystem.UpdateEvent += graphSystem.DrawFigure;
            if (model.factory.pl != null && model.factory.pl.Count > 1)
                textBox1.Text = ((ContourProps)model.factory.pl[0]).LineWidth.ToString(); ///////
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

            pictureBox.MouseDown += PictureBox_MouseDown;
            pictureBox.MouseUp += PictureBox_MouseUp;


            // graphSystem.Str += MessageBox.Show; вероятно хлам

            //pictureBox.Paint += new PaintEventHandler(this.plusevent);

            pictureBox.Focus();
        }

        private void ComboBox1_SelectedValueChanged(object sender, EventArgs e) // Выбор типа фигуры
        {
            model.factory.ChoosenFigure = (FigureType) comboBox1.SelectedValue;
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            model.factory.frame.coords[0] = e.X;
            model.factory.frame.coords[1] = e.Y;
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

            model.factory.frame.coords[2] = e.X;
            model.factory.frame.coords[3] = e.Y;

            model.factory.AddFigure();
        }

        private void pictureBox1_Click(object sender, EventArgs e) // Цвет контура
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox1.BackColor = this.colorDialog1.Color;
                ((ContourProps)model.factory.pl[0]).Color = this.colorDialog1.Color;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e) // Цвет заливки
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox2.BackColor = this.colorDialog1.Color;
                ((FillProps)model.factory.pl[1]).Color = this.colorDialog1.Color;
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e) // Толщина линии
        {
            if ((ContourProps)model.factory.pl[0] != null) 
            {
                if ((textBox1.Text.Length == 0)
                    || (int.Parse(textBox1.Text) > 100)
                    || (int.Parse(textBox1.Text) < 0))
                    textBox1.Text = ((ContourProps)model.factory.pl[0]).LineWidth.ToString();
                else
                    ((ContourProps)model.factory.pl[0]).LineWidth = int.Parse(textBox1.Text);
            }
        }

        private void PictureBox_VisibleChanged(object sender, EventArgs e) // Обнова картинки
        {
            model.scene.Repaint();
        }

        private void PictureBox_Invalidated(object sender, InvalidateEventArgs e) // Обнова картинки
        {
            model.scene.Repaint();
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e) // Обнова картинки
        {
            model.scene.Repaint();
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
                model.st.Clear();
                this.UpdateStyles();
                pictureBox.BackColor = Color.White;
                pictureBox.Size = new Size(width, height);
            }
        }

        private void button2_Click(object sender, EventArgs e)  // Кнопка "F5"
        {
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
