using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.IO.Compression;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace Eden_World_Maniputor_2._0
{
    public partial class Form1 : Form
    {
        private Bitmap newCanvas, tempCanvas;
        public static string rotation;
        public static int cut = 0;
        public static World world, world2;
        private Bitmap m_Canvas;
        Point startPos;
        Point currentPos;
        public string text;
        bool drawing, pressed, isMouseDown = false, move = false, rotate = false;
        double zoom = 1;
        public static List<KeyValuePair<string, string>> files = new List<KeyValuePair<string, string>>();
        RectangleF Moved = new RectangleF();
        Rectangle Rotate = new Rectangle();
        RectangleF Temp = new RectangleF(150, 200, 25, 25);
        List<Rectangle> rectangles = new List<Rectangle>();
        public string filename, offilename;

        public Form1(string[] args)
        {
            InitializeComponent();

            if (args.Length != 0)
            {
                world = World.LoadWorld(args[0]);
            }

            trackBar1.Enabled = false;
            trackBar3.Enabled = false;
            TrackBar2.Enabled = false;
            comboBox7.Enabled = false;
            comboBox8.Enabled = false;
            comboBox9.Enabled = false;
            comboBox6.Enabled = false;
            button10.Enabled = false;
            button7.Enabled = false;
            textBox1.Enabled = false;
            checkedListBox1.Enabled = false;
            checkedListBox2.Enabled = false;

            pictureBox1.MouseWheel += Form1_MouseWheel;
            pictureBox2.MouseWheel += Form1_MouseWheel;

            comboBox6.Items.Add("Normal Map");
            comboBox6.Items.Add("Z Slice");
            //comboBox1.Items.Add("Y Slice");
            //comboBox1.Items.Add("X Slice");
            comboBox6.Items.Add("Treasure Map");
            button9.Enabled = false;
            comboBox6.SelectedIndex = 0;

            Size size = new Size(Manipulator.X2, Manipulator.Y2);
            pictureBox1.Size = size;
            panel1.Controls.Add(pictureBox1);
            panel2.Controls.Add(pictureBox2);

            foreach (string s in Manipulator.Manipulations.Keys)
            {
                manipulations.Items.Add(s);
            }

            foreach (Painting p in Enum.GetValues(typeof(Painting)))
            {
                comboBox2.Items.Add(p);
                comboBox4.Items.Add(p);
            }
            comboBox2.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;

            foreach (BlockType b in Enum.GetValues(typeof(BlockType)))
            {
                comboBox3.Items.Add(b);
                comboBox5.Items.Add(b);
            }
            comboBox3.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;

            for (int i = 0; i <= 63; i++)
            {
                comboBox7.Items.Add(i);
                comboBox8.Items.Add(i);
            }
            comboBox7.SelectedIndex = 0;
            comboBox8.SelectedIndex = 63;

            checkedListBox1.Items.Insert(0, "Short Pines");
            checkedListBox1.Items.Insert(1, "Tall Pines");
            checkedListBox1.Items.Insert(2, "Normal Terrain Trees");
            checkedListBox1.Items.Insert(3, "Normal Trees");

            checkedListBox2.Items.Insert(0, "Very Dark Cyan");
            checkedListBox2.Items.Insert(1, "Very Dark Green");
            checkedListBox2.Items.Insert(2, "Dark Green");
            checkedListBox2.Items.Insert(3, "Medium Dark Green");
            checkedListBox2.Items.Insert(4, "Green");
            checkedListBox2.Items.Insert(5, "Medium Light Green");
            checkedListBox2.Items.Insert(6, "Light Green");
            checkedListBox2.Items.Insert(7, "Red");
            checkedListBox2.Items.Insert(8, "Orange");
            checkedListBox2.Items.Insert(9, "Yellow");

            comboBox9.Items.Add("Right 90");
            comboBox9.Items.Add("Left 90");
            comboBox9.Items.Add("180");
            comboBox9.SelectedIndex = 0;

            if (world != null)
            {
                offilename = Path.GetFileName(openFileDialog1.FileName);
                filename = Path.GetFileName(openFileDialog1.FileName);
                button15.Enabled = true;
                comboBox6.Enabled = true;
                button10.Enabled = true;
                saveworld.Enabled = true;
                drawmap.Enabled = true;
                groupBox1.Enabled = true;
                textBox1.Enabled = true;
                manipulations.SelectedIndex = 0;
                Manipulator.xYxY.Clear();
                rectangles.Clear();
                Temp.X = Moved.X = Rotate.X = 0;
                Temp.Y = Moved.Y = Rotate.X = 0;
                Temp.Width = Moved.Width = Rotate.Width = 0;
                Temp.Height = Moved.Height = Rotate.Height = 0;
                MessageBox.Show(world.Name + " loaded successfully!", "World loaded");
            }
        }

        void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                zoom = zoom + (e.Delta / 120) * .5;
                if (zoom < 1)
                {
                    zoom = .5;
                }
                else if (zoom > 5)
                {
                    zoom = 5;
                }
                if (tabControl1.Controls[0] == tabControl1.SelectedTab)
                {
                    pictureBox1.Image = ResizeImage(m_Canvas, (int)Math.Round(m_Canvas.Width * zoom), (int)Math.Round(m_Canvas.Height * zoom));
                    trackBar1.Value = (int)(zoom * 2);
                    label4.Text = string.Format("x {0}", zoom.ToString());
                }
                else if (tabControl1.Controls[2] == tabControl1.SelectedTab)
                {
                    pictureBox2.Image = ResizeImage(newCanvas, (int)Math.Round(newCanvas.Width * zoom), (int)Math.Round(newCanvas.Height * zoom));
                    trackBar3.Value = (int)(zoom * 2);
                    label21.Text = string.Format("x {0}", zoom.ToString());
                }
            }
        }

        private void OpenWorld_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    world = World.LoadWorld(openFileDialog1.FileName);

                    offilename = Path.GetFileName(openFileDialog1.FileName);
                    filename = Path.GetFileName(openFileDialog1.FileName);

                    MessageBox.Show(world.Name + " loaded successfully!", "World loaded");

                    Size size = new Size(world.WorldArea.Width * 16, world.WorldArea.Height * 16);
                    pictureBox1.Size = size;
                    panel1.Controls.Add(pictureBox1);
                    if (pictureBox1.Image != null)
                    {
                        pictureBox1.Image.Dispose();
                        pictureBox1.Image = null;
                    }
                    if (pictureBox2.Image != null)
                    {
                        pictureBox2.Image.Dispose();
                        pictureBox2.Image = null;
                    }

                    button15.Enabled = true;
                    comboBox6.Enabled = true;
                    button10.Enabled = true;
                    saveworld.Enabled = true;
                    drawmap.Enabled = true;
                    groupBox1.Enabled = true;
                    textBox1.Enabled = true;
                    manipulations.SelectedIndex = 0;
                    Manipulator.xYxY.Clear();
                    rectangles.Clear();
                    Temp.X = Moved.X = Rotate.X = 0;
                    Temp.Y = Moved.Y = Rotate.X = 0;
                    Temp.Width = Moved.Width = Rotate.Width = 0;
                    Temp.Height = Moved.Height = Rotate.Height = 0;
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Unable to load file:\r\n" + exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void SetCanvasAsImage()
        {
            pictureBox1.Image = m_Canvas;
        }

        private void SaveWorld_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = offilename;
            saveFileDialog1.Filter = ".eden|*.eden";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (filename != offilename)
                {
                    File.Delete(filename);
                }
                if (File.Exists(saveFileDialog1.FileName))
                {
                    File.Delete(saveFileDialog1.FileName);
                }
                try
                {
                    world.SaveWorld(saveFileDialog1.FileName, world);
                    MessageBox.Show(World.worldName + " saved succesfully!");
                }
                catch
                {
                    MessageBox.Show("Unable to save file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Manipulate_Click(object sender, EventArgs e)
        {
            Manipulator.Density = TrackBar2.Value / 10;
            Manipulator.Xmin = world.WorldArea.Width * 16; Manipulator.Xmax = 0; Manipulator.Ymin = world.WorldArea.Height * 16; Manipulator.Ymax = 0;

            Tree.pine.Add(0); Tree.normal.Add(0);

            if (manipulations.SelectedItem != null)
            {
                if (Manipulator.xYxY.Count == 0)
                {
                    Manipulator.xYxY.Add(Manipulator.X1); Manipulator.xYxY.Add(Manipulator.X2); Manipulator.xYxY.Add(Manipulator.Y1); Manipulator.xYxY.Add(Manipulator.Y2);
                }
                int length = Manipulator.xYxY.Count;
                var item = manipulations.SelectedItem.ToString();
                if (item != "Move Area Horizontally")
                {
                    for (int i = 0; i < length; i += 4)
                    {
                        if (Manipulator.xYxY[i] < Manipulator.Xmin)
                        {
                            Manipulator.Xmin = Manipulator.xYxY[i];
                        }
                        if (Manipulator.xYxY[i + 1] > Manipulator.Xmax)
                        {
                            Manipulator.Xmax = Manipulator.xYxY[i + 1];
                        }
                        if (Manipulator.xYxY[i + 2] < Manipulator.Ymin)
                        {
                            Manipulator.Ymin = Manipulator.xYxY[i + 2];
                        }
                        if (Manipulator.xYxY[i + 3] > Manipulator.Ymax)
                        {
                            Manipulator.Ymax = Manipulator.xYxY[i + 3];
                        }
                    }
                }
            }

            foreach (int selected in checkedListBox1.CheckedIndices)
            {
                if (selected == 0)
                {
                    Manipulator.smallpine = true;
                }
                else if (selected == 1)
                {
                    Manipulator.largepine = true;
                }
                else if (selected == 2)
                {
                    Manipulator.normalterrain = true;
                }
                else if (selected == 3)
                {
                    Manipulator.normal = true;
                }
            }

            foreach (int selected in checkedListBox2.CheckedIndices)
            {
                if (selected == 0)
                {
                    Tree.pine.Add(50);
                }
                else if (selected == 1)
                {
                    Tree.pine.Add(49);
                }
                else if (selected == 2)
                {
                    Tree.pine.Add(40); Tree.normal.Add(40);
                }
                else if (selected == 3)
                {
                    Tree.pine.Add(31); Tree.normal.Add(31);
                }
                else if (selected == 4)
                {
                    Tree.pine.Add(22);
                }
                else if (selected == 5)
                {
                    Tree.pine.Add(13); Tree.normal.Add(13);
                }
                else if (selected == 6)
                {
                    Tree.pine.Add(4); Tree.normal.Add(4);
                }
                else if (selected == 7)
                {
                    Tree.pine.Add(19); Tree.normal.Add(19);
                }
                else if (selected == 8)
                {
                    Tree.pine.Add(20); Tree.normal.Add(20);
                }
                else if (selected == 9)
                {
                    Tree.pine.Add(21); Tree.normal.Add(21);
                }
            }

            rotation = comboBox9.SelectedItem.ToString();

            comboBox3.SelectedItem = Enum.GetName(typeof(BlockType), Manipulator.BlockChoice);
            Manipulator.BlockChoice = (BlockType)comboBox3.SelectedItem;

            comboBox5.SelectedItem = Enum.GetName(typeof(BlockType), (BlockType)Manipulator.BlockChoice);
            Manipulator.BlockChoice2 = (BlockType)comboBox5.SelectedItem;

            comboBox2.SelectedItem = Enum.GetName(typeof(Painting), (Painting)Manipulator.PaintColor);
            Manipulator.PaintColor = (Painting)comboBox2.SelectedItem;

            comboBox4.SelectedItem = Enum.GetName(typeof(Painting), (Painting)Manipulator.PaintColor2);
            Manipulator.PaintColor2 = (Painting)comboBox4.SelectedItem;

            Manipulator.XMove = ((world.WorldArea.Width * 16) - (int)Math.Round(Moved.X / zoom)) - Manipulator.X2;
            Manipulator.YMove = ((world.WorldArea.Height * 16) - (int)Math.Round(Moved.Y / zoom)) - Manipulator.Y2;

            Manipulator.Xr1 += Manipulator.XMove; Manipulator.Xr2 += Manipulator.XMove; Manipulator.Yr1 += Manipulator.YMove; Manipulator.Yr2 += Manipulator.YMove;

            Manipulator.Z1 = int.Parse(comboBox7.SelectedItem.ToString());
            Manipulator.Z2 = int.Parse(comboBox8.SelectedItem.ToString());
            
            tabPage2.Hide();
            tabPage2.Hide();
            tabPage3.Hide();
            button4.Enabled = true;
            tabControl1.TabPages.Remove(tabPage2);
            tabControl1.TabPages.Remove(tabPage3);
            string select = manipulations.SelectedItem.ToString();
            Application.UseWaitCursor = true;
            Task.Factory.StartNew(() => Manipulator.Manipulate(world, Manipulator.Manipulations[select], openFileDialog1.FileName))
                .ContinueWith(t => { MessageBox.Show("Manipulation done!"); tabControl1.TabPages.Add(tabPage2); tabControl1.TabPages.Add(tabPage3); Manipulator.smallpine = false; Manipulator.largepine = false; Manipulator.normalterrain = false; Manipulator.normal = false;
                    Tree.pine.Clear(); Tree.normal.Clear(); Application.UseWaitCursor = false;
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Manipulations_TextChanged(object sender, EventArgs e)
        {
            manipulate.Enabled = Manipulator.Manipulations.Keys.Contains(manipulations.Text);
        }

        private void DrawMap_Click(object sender, EventArgs e)
        {
            trackBar1.Enabled = true;
            trackBar1.Value = 2;
            Mapping.block = 0;
            move = false; rotate = false;
            button7.Enabled = false;
            Manipulator.Xmin = world.WorldArea.Width * 16; Manipulator.Xmax = 0; Manipulator.Ymin = world.WorldArea.Height * 16; Manipulator.Ymax = 0;
            Manipulator.xYxY.Clear();
            rectangles.Clear();
            Temp.X = Moved.X = Rotate.X = 0;
            Temp.Y = Moved.Y = Rotate.X = 0;
            Temp.Width = Moved.Width = Rotate.Width = 0;
            Temp.Height = Moved.Height = Rotate.Height = 0;
            Draw(world);
            zoom = 1;
            MessageBox.Show("Map Drawn!");
            label4.Text = "x 1";
            savemap.Enabled = true;
        }

        private void Draw(World world)
        {
            Manipulator.X1 = 0; Manipulator.X2 = world.WorldArea.Width * 16; Manipulator.Y1 = 0; Manipulator.Y2 = world.WorldArea.Height * 16;
            m_Canvas = new Bitmap(Manipulator.X2, Manipulator.Y2);
            m_Canvas = Mapping.NormalMap(world, m_Canvas);
            m_Canvas.RotateFlip(RotateFlipType.Rotate180FlipNone);
            SetCanvasAsImage();
            label15.Text = Mapping.block.ToString("N0");
            label15.Visible = true;
        }

        private Rectangle GetRectangle()
        {
            Manipulator.X1 = Convert.ToInt32(Math.Round((world.WorldArea.Width * 16) - ((Math.Max(startPos.X, currentPos.X)) / zoom)));
            Manipulator.Y1 = Convert.ToInt32(Math.Round((world.WorldArea.Height * 16) - ((Math.Max(startPos.Y, currentPos.Y)) / zoom)));
            Manipulator.X2 = Convert.ToInt32(Math.Round(Manipulator.X1 + (Math.Abs(startPos.X - currentPos.X)) / zoom));
            Manipulator.Y2 = Convert.ToInt32(Math.Round(Manipulator.Y1 + (Math.Abs(startPos.Y - currentPos.Y)) / zoom));

            Moved.X = Math.Min(startPos.X, currentPos.X);
            Moved.Y = Math.Min(startPos.Y, currentPos.Y);
            Moved.Width = Math.Abs(startPos.X - currentPos.X);
            Moved.Height = Math.Abs(startPos.Y - currentPos.Y);

            Rotate.X = Convert.ToInt32(Moved.X);
            Rotate.Y = Convert.ToInt32(Moved.Y);
            Rotate.Width = Convert.ToInt32(Moved.Width);
            Rotate.Height = Convert.ToInt32(Moved.Height);

            return new Rectangle(
                Math.Min(startPos.X, currentPos.X),
                Math.Min(startPos.Y, currentPos.Y),
                Math.Abs(startPos.X - currentPos.X),
                Math.Abs(startPos.Y - currentPos.Y));
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (move) isMouseDown = true;
            if (pressed)
            {
                currentPos = startPos = e.Location;
                drawing = true;
            }
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            currentPos = e.Location;
            if (drawing) pictureBox1.Invalidate();

            if (isMouseDown && move && rotate)
            {
                Rotate.Location = e.Location;

                if (Rotate.Right > pictureBox1.Width)
                {
                    Rotate.X = pictureBox1.Width - Rotate.Width;
                }
                if (Rotate.Top < 0)
                {
                    Rotate.Y = 0;
                }
                if (Rotate.Left < 0)
                {
                    Rotate.X = 0;
                }
                if (Rotate.Bottom > pictureBox1.Height)
                {
                    Rotate.Y = pictureBox1.Height - Rotate.Height;
                }
                Refresh();
            }
            if (isMouseDown && move)
            {
                Moved.Location = e.Location;

                if (Moved.Right > pictureBox1.Width)
                {
                    Moved.X = pictureBox1.Width - Moved.Width;
                }
                if (Moved.Top < 0)
                {
                    Moved.Y = 0;
                }
                if (Moved.Left < 0)
                {
                    Moved.X = 0;
                }
                if (Moved.Bottom > pictureBox1.Height)
                {
                    Moved.Y = pictureBox1.Height - Moved.Height;
                }
                Refresh();
            }
            if (move)
            {
                label14.Text = (((world.WorldArea.Width * 16) - (int)Math.Round(Moved.X / zoom)) - Manipulator.X2).ToString();
                label11.Text = (((world.WorldArea.Height * 16) - (int)Math.Round(Moved.Y / zoom)) - Manipulator.Y2).ToString();
                label14.Visible = true;
                label11.Visible = true;
            }
        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (move) isMouseDown = false;
            if (drawing && pressed)
            {
                drawing = false;
                pressed = false;
                Manipulator.xYxY.Add(Manipulator.X1); Manipulator.xYxY.Add(Manipulator.X2); Manipulator.xYxY.Add(Manipulator.Y1); Manipulator.xYxY.Add(Manipulator.Y2);
                var rc = GetRectangle();
                if (rc.Width > 0 && rc.Height > 0) rectangles.Add(rc);
                pictureBox1.Invalidate();
            }
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (rectangles.Count > 0 && manipulations.SelectedItem.ToString() != "Move Area Horizontally" && manipulations.SelectedItem.ToString() != "Rotate and Move")
            {
                e.Graphics.DrawRectangles(Pens.Red, rectangles.ToArray());
            }
            else
            {
                Temp.X = Moved.X;
                Temp.Y = Moved.Y;
                Temp.Width = Moved.Width;
                Temp.Height = Moved.Height;
                e.Graphics.DrawRectangles(Pens.Red, new[] { Temp });
            }
            if (rotate)
            {
                RotateRectangle(e.Graphics, Rotate, 90);
                Manipulator.Xr1 = Manipulator.X1 + ((Manipulator.X2 - Manipulator.X1) / 2 - (Manipulator.Y2 - Manipulator.Y1) / 2); Manipulator.Xr2 = Manipulator.Xr1 + (Manipulator.Y2 - Manipulator.Y1); Manipulator.Yr1 = Manipulator.Y1 + ((Manipulator.Y2 - Manipulator.Y1) / 2 - (Manipulator.X2 - Manipulator.X1) / 2); Manipulator.Yr2 = Manipulator.Yr1 + (Manipulator.X2 - Manipulator.X1);
                e.Graphics.DrawRectangles(Pens.Red, rectangles.ToArray());
                e.Graphics.DrawRectangles(Pens.Red, new[] { Temp });
            }
            if (drawing) e.Graphics.DrawRectangle(Pens.Red, GetRectangle());
        }

        public void RotateRectangle(Graphics g, Rectangle r, float angle)
        {
            using (Matrix m = new Matrix())
            {
                m.RotateAt(angle, new PointF(r.Left + (r.Width / 2),
                                          r.Top + (r.Height / 2)));
                g.Transform = m;
                g.DrawRectangle(Pens.Black, r);
                g.ResetTransform();
            }
        }

        private void Rectangle_Click(object sender, EventArgs e)
        {
            if (manipulations.SelectedItem.ToString() == "Rotate and Move") button7.Enabled = true;
            trackBar1.Enabled = false;
            pressed = true;
        }

        private void Erase_Click(object sender, EventArgs e)
        {
            move = false; rotate = false;
            button7.Enabled = false;
            Manipulator.Xmin = world.WorldArea.Width * 16; Manipulator.Xmax = 0; Manipulator.Ymin = world.WorldArea.Height * 16; Manipulator.Ymax = 0;
            Manipulator.xYxY.Clear();
            rectangles.Clear();
            trackBar1.Enabled = true;
            Temp.X = Moved.X = Rotate.X = 0;
            Temp.Y = Moved.Y = Rotate.X = 0;
            Temp.Width = Moved.Width = Rotate.Width = 0;
            Temp.Height = Moved.Height = Rotate.Height = 0;
            pictureBox1.Invalidate();
        }

        private void SaveMap_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                FileName = string.Format(world.Name + " zoom = {0}", zoom),
                Filter = ".png|*.png"
            };

            var result = dialog.ShowDialog(); //shows save file dialog
            if (result == DialogResult.OK)
            {
                Console.WriteLine("writing to: " + dialog.FileName); //prints the file to save

                pictureBox1.Image.Save(dialog.FileName, ImageFormat.Png);
                MessageBox.Show("Saved " + world.Name);
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (manipulations.SelectedItem != null)
            {
                var item = manipulations.SelectedItem.ToString();
                if (item == "Block and Color Change" || item == "Block Change" || item == "Color Change")
                {
                    if (item == "Block and Color Change")
                    {
                        groupBox2.Enabled = true;
                        groupBox3.Enabled = true;
                        groupBox4.Enabled = true;
                        groupBox5.Enabled = true;
                    }
                    else if (item == "Color Change")
                    {
                        groupBox2.Enabled = true;
                        groupBox3.Enabled = false;
                        groupBox4.Enabled = true;
                        groupBox5.Enabled = false;
                    }
                    else
                    {
                        groupBox2.Enabled = false;
                        groupBox3.Enabled = true;
                        groupBox4.Enabled = false;
                        groupBox5.Enabled = true;
                    }
                    label2.Text = "Paint Colors";
                    label12.Text = "Paint Colors";
                    label8.Text = "Zmin";
                    label9.Text = "Zmax";
                    label3.Text = "Block Types";
                    comboBox3.SelectedIndex = 0;
                    comboBox7.SelectedIndex = 0;
                    comboBox8.SelectedIndex = 63;
                    rectangle.Enabled = true;
                    erase.Enabled = true;
                    comboBox7.Enabled = true;
                    comboBox8.Enabled = true;
                    TrackBar2.Enabled = false;
                    comboBox9.Enabled = false;
                    button5.Enabled = false;
                    button7.Enabled = false;
                    checkedListBox1.Enabled = false;
                    checkedListBox2.Enabled = false;
                }
                else if (item == "Lower or Raise")
                {
                    label8.Text = "Lower Amount";
                    label9.Text = "Raise Amount";
                    label3.Text = "Block Types";
                    comboBox3.SelectedIndex = 0;
                    comboBox8.SelectedIndex = 0;
                    comboBox7.SelectedIndex = 0;
                    rectangle.Enabled = true;
                    erase.Enabled = true;
                    comboBox7.Enabled = true;
                    comboBox8.Enabled = true;
                    TrackBar2.Enabled = false;
                    groupBox5.Enabled = false;
                    groupBox2.Enabled = false;
                    groupBox3.Enabled = false;
                    groupBox4.Enabled = false;
                    button5.Enabled = false;
                    comboBox9.Enabled = false;
                    button7.Enabled = false;
                    checkedListBox1.Enabled = false;
                    checkedListBox2.Enabled = false;
                }
                else if (item == "Move Area Horizontally")
                {
                    label2.Text = "Paint Colors";
                    label12.Text = "Paint Colors";
                    label8.Text = "Zmin";
                    label9.Text = "Zmax";
                    label3.Text = "Block Types";
                    comboBox3.SelectedIndex = 0;
                    comboBox7.SelectedIndex = 0;
                    comboBox8.SelectedIndex = 63;
                    button5.Enabled = true;
                    comboBox7.Enabled = true;
                    comboBox8.Enabled = true;
                    TrackBar2.Enabled = false;
                    groupBox5.Enabled = false;
                    groupBox2.Enabled = false;
                    groupBox3.Enabled = false;
                    groupBox4.Enabled = false;
                    rectangle.Enabled = true;
                    erase.Enabled = true;
                    comboBox9.Enabled = false;
                    button7.Enabled = false;
                    checkedListBox1.Enabled = false;
                    checkedListBox2.Enabled = false;
                }
                else if (item == "Rotate and Move")
                {
                    label2.Text = "Paint Colors";
                    label12.Text = "Paint Colors";
                    label8.Text = "Zmin";
                    label9.Text = "Zmax";
                    label3.Text = "Block Types";
                    comboBox3.SelectedIndex = 0;
                    comboBox7.SelectedIndex = 0;
                    comboBox8.SelectedIndex = 63;
                    button5.Enabled = true;
                    button7.Enabled = true;
                    comboBox7.Enabled = true;
                    comboBox8.Enabled = true;
                    TrackBar2.Enabled = false;
                    groupBox5.Enabled = false;
                    groupBox2.Enabled = false;
                    groupBox3.Enabled = false;
                    groupBox4.Enabled = false;
                    comboBox9.Enabled = true;
                    rectangle.Enabled = true;
                    erase.Enabled = true;
                    checkedListBox1.Enabled = false;
                    checkedListBox2.Enabled = false;
                }
                else if (item == "Remove Dirt,Stone,Grass")
                {
                    label2.Text = "Paint Colors";
                    label12.Text = "Paint Colors";
                    label8.Text = "Zmin";
                    label9.Text = "Zmax";
                    label3.Text = "Block Types";
                    comboBox3.SelectedIndex = 0;
                    comboBox7.SelectedIndex = 0;
                    comboBox8.SelectedIndex = 63;
                    comboBox7.Enabled = true;
                    comboBox8.Enabled = true;
                    groupBox5.Enabled = false;
                    groupBox2.Enabled = false;
                    groupBox3.Enabled = false;
                    groupBox4.Enabled = false;
                    rectangle.Enabled = true;
                    erase.Enabled = true;
                    TrackBar2.Enabled = false;
                    comboBox9.Enabled = false;
                    button5.Enabled = false;
                    button7.Enabled = false;
                    checkedListBox1.Enabled = false;
                    checkedListBox2.Enabled = false;
                }
                else if (item == "Create Ocean")
                {
                    label2.Text = "Sea Floor Color";
                    label12.Text = "Water Color";
                    label8.Text = "Sand Level";
                    label9.Text = "Water Level";
                    label3.Text = "Sea Floor";
                    comboBox3.SelectedIndex = 4;
                    comboBox7.SelectedIndex = 0;
                    comboBox8.SelectedIndex = 31;
                    comboBox7.Enabled = true;
                    comboBox8.Enabled = true;
                    groupBox2.Enabled = true;
                    groupBox4.Enabled = true;
                    groupBox5.Enabled = false;
                    erase.Enabled = true;
                    rectangle.Enabled = true;
                    groupBox3.Enabled = true;
                    TrackBar2.Enabled = false;
                    comboBox9.Enabled = false;
                    button5.Enabled = false;
                    button7.Enabled = false;
                    checkedListBox1.Enabled = false;
                    checkedListBox2.Enabled = false;
                }
                else if (item == "Erase")
                {
                    label2.Text = "Paint Colors";
                    label12.Text = "Paint Colors";
                    label8.Text = "Zmin";
                    label9.Text = "Zmax";
                    label3.Text = "Block Types";
                    comboBox3.SelectedIndex = 0;
                    comboBox7.SelectedIndex = 0;
                    comboBox8.SelectedIndex = 63;
                    comboBox7.Enabled = false;
                    comboBox8.Enabled = false;
                    groupBox2.Enabled = false;
                    groupBox4.Enabled = false;
                    erase.Enabled = true;
                    rectangle.Enabled = true;
                    TrackBar2.Enabled = false;
                    groupBox3.Enabled = false;
                    comboBox9.Enabled = false;
                    button5.Enabled = false;
                    groupBox5.Enabled = false;
                    button7.Enabled = false;
                    checkedListBox1.Enabled = false;
                    checkedListBox2.Enabled = false;
                }
                else if (item == "Add Land")
                {
                    label2.Text = "Paint Colors";
                    label12.Text = "Paint Colors";
                    label8.Text = "Zmin";
                    label9.Text = "Zmax";
                    label3.Text = "Block Types";
                    comboBox3.SelectedIndex = 0;
                    comboBox7.SelectedIndex = 0;
                    comboBox8.SelectedIndex = 63;
                    comboBox7.Enabled = true;
                    comboBox8.Enabled = true;
                    groupBox2.Enabled = false;
                    groupBox4.Enabled = false;
                    erase.Enabled = true;
                    rectangle.Enabled = true;
                    TrackBar2.Enabled = false;
                    comboBox9.Enabled = false;
                    groupBox3.Enabled = false;
                    button5.Enabled = false;
                    groupBox5.Enabled = false;
                    button7.Enabled = false;
                    checkedListBox1.Enabled = false;
                    checkedListBox2.Enabled = false;
                }
                else if (item == "Generate Random Trees")
                {
                    label2.Text = "Paint Colors";
                    label12.Text = "Paint Colors";
                    label8.Text = "Zmin";
                    label9.Text = "Zmax";
                    label3.Text = "Block Types";
                    comboBox3.SelectedIndex = 0;
                    comboBox7.SelectedIndex = 0;
                    comboBox8.SelectedIndex = 63;
                    checkedListBox1.Enabled = true;
                    checkedListBox2.Enabled = true;
                    comboBox7.Enabled = false;
                    erase.Enabled = true;
                    rectangle.Enabled = true;
                    TrackBar2.Enabled = true;
                    comboBox8.Enabled = false;
                    groupBox5.Enabled = false;
                    comboBox9.Enabled = false;
                    groupBox2.Enabled = false;
                    groupBox3.Enabled = false;
                    groupBox4.Enabled = false;
                    button5.Enabled = false;
                    button7.Enabled = false;
                }
                else
                {
                    label2.Text = "Paint Colors";
                    label12.Text = "Paint Colors";
                    label8.Text = "Zmin";
                    label9.Text = "Zmax";
                    label3.Text = "Block Types";
                    comboBox3.SelectedIndex = 0;
                    comboBox7.SelectedIndex = 0;
                    comboBox8.SelectedIndex = 63;
                    checkedListBox1.Enabled = false;
                    checkedListBox2.Enabled = false;
                    comboBox9.Enabled = false;
                    TrackBar2.Enabled = false;
                    comboBox7.Enabled = false;
                    erase.Enabled = false;
                    rectangle.Enabled = false;
                    comboBox8.Enabled = false;
                    groupBox5.Enabled = false;
                    groupBox2.Enabled = false;
                    groupBox3.Enabled = false;
                    groupBox4.Enabled = false;
                    button5.Enabled = false;
                    button7.Enabled = false;
                }
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            world.Bytes = Manipulator.undo;
            MessageBox.Show("Undo Completed!");
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            World.worldName = textBox1.Text;
            World.worldName = new string(World.worldName.Where(c => (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '\'')).ToArray());
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            if (comboBox9.SelectedIndex == 0 || comboBox9.SelectedIndex == 1) rotate = true;
            pictureBox1.Invalidate();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            move = true;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (filename != offilename)
            {
                File.Delete(filename);
            }
        }

        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            zoom = trackBar1.Value * .5;
            label4.Text = string.Format("x {0}", zoom.ToString());

            pictureBox1.Image = ResizeImage(m_Canvas, (int)Math.Round(m_Canvas.Width * zoom), (int)Math.Round(m_Canvas.Height * zoom));

        }

        private void Button15_Click(object sender, EventArgs e)
        {
            Manipulator.AddChunks(world);
            MessageBox.Show("All Chunk Added");
        }

        // Tab 2

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
            text = textBox2.Text;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            listView1.Refresh();
            files.Clear();
            List<string> temp = new List<string>();
            var url = "http://app.edengame.net/list2.php?search=" + text;
            var client = new WebClient();
            using (var stream = client.OpenRead(url))
            using (var reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    temp.Add(line);
                }
            }
            for (int i = temp.Count - 2; i >= 0; i -= 2)
            {
                files.Insert(0, new KeyValuePair<string, string>(temp[i + 1].Substring(0, temp[i + 1].Length - 5), temp[i]));
            }
            long time;
            for (int i = 0; i < files.Count(); i ++)
            {
                time = (long)Convert.ToDouble(files[i].Value.Substring(0, files[i].Value.Length - 5));
                ListViewItem worldfilename = new ListViewItem(files[i].Key);
                worldfilename.SubItems.Add((new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddSeconds(time).ToString("MM/dd/yyyy"));
                listView1.Items.Add(worldfilename);
            }
            groupBox6.Enabled = true;
            button1.Enabled = true;
            button8.Enabled = true;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string selectedText;
            WebClient webClient = new WebClient();
            selectedText = files[listView1.FocusedItem.Index].Value;

            string urlString = "http://files.edengame.net/" + selectedText;

            byte[] data = webClient.DownloadData(urlString);
            byte[] decompressed = Decompress(data);
            world = World.LoadDownload(decompressed);

            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
                pictureBox1.Image = null;
            }
            if (pictureBox2.Image != null)
            {
                pictureBox2.Image.Dispose();
                pictureBox2.Image = null;
            }

            offilename = selectedText;
            filename = selectedText;
            button15.Enabled = true;
            comboBox6.Enabled = true;
            button10.Enabled = true;
            saveworld.Enabled = true;
            drawmap.Enabled = true;
            groupBox1.Enabled = true;
            textBox1.Enabled = true;
            manipulations.SelectedIndex = 0;
            Manipulator.xYxY.Clear();
            rectangles.Clear();
            Temp.X = Moved.X = Rotate.X = 0;
            Temp.Y = Moved.Y = Rotate.X = 0;
            Temp.Width = Moved.Width = Rotate.Width = 0;
            Temp.Height = Moved.Height = Rotate.Height = 0;
            tabControl1.SelectedIndex = 0;
            MessageBox.Show(world.Name + " loaded successfully!", "World loaded");
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            string selectedText;
            WebClient webClient = new WebClient();
            selectedText = files[listView1.FocusedItem.Index].Value;

            string urlString = "http://files.edengame.net/" + selectedText;

            byte[] data = webClient.DownloadData(urlString);
            byte[] decompressed = Decompress(data);

            var dialog = new SaveFileDialog
            {
                FileName = selectedText,
                Filter = ".eden|*.eden"
            };

            var result = dialog.ShowDialog(); //shows save file dialog
            if (result == DialogResult.OK)
            {
                Console.WriteLine("writing to: " + dialog.FileName); //prints the file to save

                File.WriteAllBytes(dialog.FileName, decompressed);
                MessageBox.Show("Download and Save Completed");
            }
        }

        static byte[] Decompress(byte[] gzip)
        {
            // Create a GZIP stream with decompression mode.
            // Then create a buffer and write into while reading from the GZIP stream.
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip),
                CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }

        // Tab 3

        private void Button10_Click(object sender, EventArgs e)
        {
            Drawing(world);
            button9.Enabled = true;
            zoom = 1;
            label21.Text = "x 1";
            MessageBox.Show(world.Name + " Map Drawn!");
        }

        private void Drawing(World world)
        {
            Manipulator.X1 = 0; Manipulator.X2 = world.WorldArea.Width * 16; Manipulator.Y1 = 0; Manipulator.Y2 = world.WorldArea.Height * 16;

            if (comboBox6.SelectedItem != null)
            {
                var item = comboBox6.SelectedItem.ToString();
                if (item == "Y Slice")
                {
                    tempCanvas = new Bitmap(Manipulator.X2, 64);
                    newCanvas = new Bitmap(Manipulator.X2, 64);
                    Size size2 = new Size(Manipulator.X2, 64);
                    pictureBox2.Size = size2;
                    if (cut > Manipulator.Y2 - 1) cut = Manipulator.Y2 - 1;
                    else if (cut < 0) cut = 0;
                    Mapping.YSlice(world, newCanvas, tempCanvas, cut);

                }
                else if (item == "X Slice")
                {
                    tempCanvas = new Bitmap(Manipulator.Y2, 64);
                    newCanvas = new Bitmap(Manipulator.Y2, 64);
                    Size size2 = new Size(Manipulator.Y2, 64);
                    pictureBox2.Size = size2;
                    if (cut > Manipulator.X2 - 1) cut = Manipulator.X2 - 1;
                    else if (cut < 0) cut = 0;
                    Mapping.XSlice(world, newCanvas, tempCanvas, cut);
                }
                else if (item == "Z Slice")
                {
                    Size size = new Size(Manipulator.X2, Manipulator.Y2);
                    pictureBox2.Size = size;
                    newCanvas = new Bitmap(Manipulator.X2, Manipulator.Y2);
                    if (cut > 63) cut = 63;
                    else if (cut < 0) cut = 0;
                    newCanvas = Mapping.ZSlice(world, newCanvas, cut);
                }
                else if (item == "Normal Map")
                {
                    Size size = new Size(Manipulator.X2, Manipulator.Y2);
                    pictureBox2.Size = size;
                    tempCanvas = new Bitmap(Manipulator.X2, Manipulator.Y2);
                    newCanvas = new Bitmap(Manipulator.X2, Manipulator.Y2);
                    newCanvas = Mapping.BlendNormalMap(world, newCanvas, tempCanvas);
                }
                else if (item == "Treasure Map")
                {
                    Size size = new Size(Manipulator.X2, Manipulator.Y2);
                    pictureBox2.Size = size;
                    newCanvas = new Bitmap(Manipulator.X2, Manipulator.Y2);
                    newCanvas = Mapping.Treasure(world, newCanvas);
                }
                trackBar3.Enabled = true;
                trackBar3.Value = 2;
                newCanvas.RotateFlip(RotateFlipType.Rotate180FlipNone);
                SetCanvasAsImage2();

            }
        }

        private void TrackBar3_Scroll(object sender, EventArgs e)
        {
            zoom = trackBar3.Value * .5;
            label21.Text = string.Format("x {0}", zoom.ToString());

            pictureBox2.Image = ResizeImage(newCanvas, (int)Math.Round(newCanvas.Width * zoom), (int)Math.Round(newCanvas.Height * zoom));
        }

        public void SetCanvasAsImage2()
        {
            pictureBox2.Image = newCanvas;
        }

        private void TextBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text != "" & textBox3.Text != "-")
            {
                cut = int.Parse(textBox3.Text);
            }
        }

        private void Button14_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                FileName = world.Name,
                Filter = ".png|*.png"
            };

            var item = comboBox6.SelectedItem.ToString();
            if (item == "Y Slice")
            {
                dialog.FileName = string.Format(world.Name + " Slice@Y={0} zoom={1}", cut, zoom);
            }
            else if (item == "X Slice")
            {
                dialog.FileName = string.Format(world.Name + " Slice@X={0} zoom={1}", cut, zoom);
            }
            else if (item == "Z Slice")
            {
                dialog.FileName = string.Format(world.Name + " Slice@Z={0} zoom={1}", cut, zoom);
            }
            else if (item == "Normal Map")
            {
                dialog.FileName = string.Format(world.Name + " zoom={0}", zoom);
            }
            else if (item == "Treasure Map")
            {
                dialog.FileName = string.Format(world.Name + "TreasureMap zoom={0}", zoom);
            }

            var result = dialog.ShowDialog(); //shows save file dialog
            if (result == DialogResult.OK)
            {
                Console.WriteLine("writing to: " + dialog.FileName); //prints the file to save

                pictureBox2.Image.Save(dialog.FileName, ImageFormat.Png);

                MessageBox.Show("Saved " + world.Name);
            }
        }

        private void ComboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = comboBox6.SelectedItem.ToString();

            if (item == "Normal Map") textBox3.Enabled = false;
            else textBox3.Enabled = true;
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            newCanvas.RotateFlip(RotateFlipType.Rotate180FlipNone);
            SetCanvasAsImage2();
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}