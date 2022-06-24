using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace HiddenImage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog imageDialog = new OpenFileDialog();
            imageDialog.Title = "Choose the image you want to decode";
            imageDialog.Filter = "Only PNG files (*.png)|*.png";

            Bitmap discoveredImage = null;

            if (imageDialog.ShowDialog() == DialogResult.OK)
                discoveredImage = HiddenImage.DecodeAnImage(new Bitmap(imageDialog.FileName));
            else return;

            SaveFileDialog discoveredImageDialog = new SaveFileDialog();
            discoveredImageDialog.Title = "Choose where you wanna store new image";
            discoveredImageDialog.Filter = "Only PNG files (*.png)|*.png";

            if (discoveredImageDialog.ShowDialog() == DialogResult.OK)
            {
                discoveredImage.Save(discoveredImageDialog.FileName);
                MessageBox.Show("New image has been saved");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog visibleImageDialog = new OpenFileDialog();
            visibleImageDialog.Title = "Choose the image you want to hide into";
            visibleImageDialog.Filter = "Only PNG files (*.png)|*.png";

            OpenFileDialog hiddenImageDialog = new OpenFileDialog();
            hiddenImageDialog.Title = "Choose the image you want to hide";
            hiddenImageDialog.Filter = "Only PNG files (*.png)|*.png";

            Bitmap finalImage = null;

            //Need a check for images. Are images's size okay?
            if (visibleImageDialog.ShowDialog() == DialogResult.OK &&
                hiddenImageDialog.ShowDialog() == DialogResult.OK)
                finalImage = HiddenImage.HideAnImage(new Bitmap(visibleImageDialog.FileName), new Bitmap(hiddenImageDialog.FileName));
            else return;

            SaveFileDialog newImageDialog = new SaveFileDialog();
            newImageDialog.Title = "Choose where you wanna store new image";
            newImageDialog.Filter = "Only PNG files (*.png)|*.png";

            if (newImageDialog.ShowDialog() == DialogResult.OK)
            {
                finalImage.Save(newImageDialog.FileName);
                MessageBox.Show("New image has been saved");
            }
        }
    }
}
