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
        IController controller;
        PictureBox pictureBox;

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
            pictureBox.Size = new System.Drawing.Size(671, 461);
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = true;

            pictureBox.MouseMove += this.UpdateCoordinates;

            textBox2.Text = pictureBox.Width.ToString();
            textBox3.Text = pictureBox.Height.ToString();
            // PictureBox }


            ContourProps cp = new ContourProps(this.pictureBox1.BackColor, 5, LineType.SolidColor);
            FillProps fp = new FillProps(this.pictureBox2.BackColor, FillType.SolidColor);
            PropList pl = new PropList(cp, fp);

            IGrProperties GrProperties = new GrPropChannel(pl);

            Graphics gr = pictureBox.CreateGraphics();

            controller = new Controller(new Model(gr, GrProperties));

            comboBox1.DataSource = Enum.GetValues(typeof(FigureType));
            comboBox1.SelectedValueChanged += ComboBox1_SelectedValueChanged;
            comboBox1.SelectedIndex = 0; //

            comboBox2.DataSource = Enum.GetValues(typeof(LineType));
            comboBox2.SelectedValueChanged += ComboBox2_SelectedValueChanged;
            comboBox2.SelectedIndex = 0; //

            comboBox3.DataSource = Enum.GetValues(typeof(FillType));
            comboBox3.SelectedValueChanged += ComboBox3_SelectedValueChanged;
            comboBox3.SelectedIndex = 0; //

            controller.Model.Factory.ChoosenFigure = (FigureType)comboBox1.SelectedValue;

            textBox1.TextChanged += TextBox1_TextChanged;
            /*
            if (controller.Model.Factory.pl.ContourProps != null)
                textBox1.Text = (controller.Model.Factory.pl.ContourProps).LineWidth.ToString(); ///////
            */
            if (controller.Model.Factory.GrProperties.Contour != null)
                textBox1.Text = controller.Model.Factory.GrProperties.Contour.LineWidth.ToString(); ///////

            pictureBox.Paint += PictureBox_Paint;
            pictureBox.Resize += PictureBox_VisibleChanged;

            pictureBox.MouseMove += controller.EventHandler.MouseMove;
            pictureBox.MouseDown += controller.EventHandler.LeftMouseDown;
            pictureBox.MouseUp += controller.EventHandler.LeftMouseUp;
            this.KeyDown += controller.EventHandler.KeyDown;
            this.KeyUp += controller.EventHandler.KeyUp;



            //this.Focus();

        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized || this.Focused == false)
                this.Dispose();
        }

        private void ComboBox1_SelectedValueChanged(object sender, EventArgs e) // Выбор типа фигуры
        {
            controller.Model.Factory.ChoosenFigure = (FigureType) comboBox1.SelectedValue;
        }
        private void ComboBox2_SelectedValueChanged(object sender, EventArgs e) // Выбор типа линии
        {
            controller.Model.Factory.GrProperties.Contour.Type = (LineType)comboBox2.SelectedValue;
        }

        private void ComboBox3_SelectedValueChanged(object sender, EventArgs e) // Выбор типа заливки
        {
            controller.Model.Factory.GrProperties.Fill.Type = (FillType)comboBox3.SelectedValue;
        }

        private void pictureBox1_Click(object sender, EventArgs e) // Цвет контура
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox1.BackColor = this.colorDialog1.Color;
                controller.Model.Factory.GrProperties.Contour.Color = this.colorDialog1.Color;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e) // Цвет заливки
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox2.BackColor = this.colorDialog1.Color;
                controller.Model.Factory.GrProperties.Fill.Color = this.colorDialog1.Color;
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e) // Толщина линии
        {
            if (controller.Model.GrProperties.Contour != null) 
            {
                try
                {
                    int.Parse(textBox1.Text);
                }
                catch (Exception)
                {
                    textBox1.Text = controller.Model.Factory.GrProperties.Contour.LineWidth.ToString();
                    return;
                }
                if ((textBox1.Text.Length == 0)
                    || (int.Parse(textBox1.Text) > 100)
                    || (int.Parse(textBox1.Text) < 0))
                    textBox1.Text = controller.Model.Factory.GrProperties.Contour.LineWidth.ToString();
                else
                    controller.Model.Factory.GrProperties.Contour.LineWidth = int.Parse(textBox1.Text);
            }
        }

        private void PictureBox_VisibleChanged(object sender, EventArgs e) // Обнова картинки
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
                controller.Model.Factory.selController.selStore.Clear();
                controller.Model.Factory.selController.selStore.Release();
                this.UpdateStyles();
                pictureBox.BackColor = Color.White;
                pictureBox.Size = new Size(width, height);
                controller.Model.GrController.SetPort(pictureBox.CreateGraphics());//, width, height);
            }
        }

        private void button2_Click(object sender, EventArgs e)  // Кнопка "F5"
        {
            controller.f5();
        }

        private void UpdateCoordinates(object sender, MouseEventArgs e)
        {
            this.toolStripStatusLabel1.Text = e.X + "," + e.Y;
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}
