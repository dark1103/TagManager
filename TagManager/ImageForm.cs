using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TagManager
{
    public partial class ImageForm : Form
    {
        public ImageForm(Form1 parent)
        {
            this.parent = parent;
            InitializeComponent();
        }
        List<ImageFileSizePair> images = new List<ImageFileSizePair>();
        private Form1 parent;
        int selectedIndex;
        public bool closed;
        public void Show(IEnumerable<string> list)
        {
            images.Clear();
            foreach (var f in list)
            {
                Image image = Image.FromFile(f);
                images.Add(new ImageFileSizePair(f, image.Size));
                image.Dispose();
            }
            selectedIndex = 0;
            SelectImage(selectedIndex);
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }
            bool v = Visible;
            if (Visible)
            {
                Focus();
                parent.Focus();
            }
            else
            {
                Show();
            }
        }
        void SelectImage(int index)
        {
            if (images.Count > 0)
            {
                index = Math.Max(Math.Min(index, images.Count - 1), 0);
                selectedIndex = index;
                maxSize = images[index].size;
                if (useMaxSize)
                {
                    pictureBox1.MaximumSize = maxSize;
                }
                pictureBox1.ImageLocation = images[index].file;
            }
        }
        bool useMaxSize = true;
        Size maxSize = Size.Empty; 
        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Focus();
            SelectImage(selectedIndex - 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Focus();
            SelectImage(selectedIndex + 1);
        }


        private void ImageForm_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            switch (e.KeyCode)
            {
                case Keys.Left:
                SelectImage(selectedIndex - 1);
                break;
                case Keys.Right:
                SelectImage(selectedIndex + 1);
                break;
                case Keys.Escape:
                Close();
                break;
            }
        }

        private void ImageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            closed = true;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            ToolStripMenuItem owner = (ToolStripMenuItem)item.OwnerItem;
            foreach (ToolStripMenuItem i in owner.DropDownItems)
            {
                i.Checked = false;
            }
            item.Checked = true;
            switch (item.Text)
            {
                case "Zoom":
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                break;
                case "Stretch":
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                break;
                case "Center":
                pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                break;
                case "Normal":
                pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
                break;
                case "Auto":
                pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                break;
            }
        }

        private void limitSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            item.Checked = !item.Checked;
            useMaxSize = item.Checked;
            if (useMaxSize)
            {
                pictureBox1.MaximumSize = maxSize;
            }
            else
            {
                pictureBox1.MaximumSize = Size.Empty;
            }
        }
    }
    public class ImageFileSizePair
    {
        public string file;
        public Size size;
        public ImageFileSizePair(string file, Size size)
        {
            this.file = file;
            this.size = size;
        }
    }
}
